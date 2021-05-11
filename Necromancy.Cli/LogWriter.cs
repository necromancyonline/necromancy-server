using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Cli
{
    public class LogWriter : ISwitchConsumer
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(LogWriter));

        private readonly object _consoleLock;
        private readonly Queue<Log> _logQueue;
        private readonly HashSet<ushort> _packetIdBlacklist;
        private readonly HashSet<ushort> _packetIdWhitelist;
        private readonly Dictionary<ServerType, HashSet<ushort>> _serverTypeBlacklist;
        private readonly Dictionary<ServerType, HashSet<ushort>> _serverTypeWhitelist;
        private bool _continue;
        private bool _paused;

        public LogWriter()
        {
            _serverTypeBlacklist = new Dictionary<ServerType, HashSet<ushort>>();
            _serverTypeWhitelist = new Dictionary<ServerType, HashSet<ushort>>();
            _packetIdWhitelist = new HashSet<ushort>();
            _packetIdBlacklist = new HashSet<ushort>();
            _logQueue = new Queue<Log>();
            _consoleLock = new object();
            switches = new List<ISwitchProperty>();
            _paused = false;
            _continue = false;
            Reset();
            LoadSwitches();
            LogProvider.OnLogWrite += LogProviderOnGlobalLogWrite;
        }

        /// <summary>
        ///     --max-packet-size=64
        /// </summary>
        public int maxPacketSize { get; set; }

        /// <summary>
        ///     --no-data=true
        /// </summary>
        public bool noData { get; set; }

        /// <summary>
        ///     --log-level=2
        /// </summary>
        public int minLogLevel { get; set; }

        public List<ISwitchProperty> switches { get; }

        public void Reset()
        {
            maxPacketSize = -1;
            noData = false;
            minLogLevel = (int)LogLevel.Debug;
            _serverTypeBlacklist.Clear();
            _serverTypeWhitelist.Clear();
            _packetIdWhitelist.Clear();
            _packetIdBlacklist.Clear();
        }

        public void WhitelistPacket(ushort packetId)
        {
            if (_packetIdWhitelist.Contains(packetId))
            {
                _Logger.Error($"PacketId:{packetId} is already whitelisted");
                return;
            }

            _packetIdWhitelist.Add(packetId);
        }

        public void BlacklistPacket(ushort packetId)
        {
            if (_packetIdBlacklist.Contains(packetId))
            {
                _Logger.Error($"PacketId:{packetId} is already blacklisted");
                return;
            }

            _packetIdBlacklist.Add(packetId);
        }

        public void WhitelistPacket(ServerType serverType, ushort packetId)
        {
            if (!AddToServerTypeList(_serverTypeWhitelist, serverType, packetId)) _Logger.Error($"WhitelistPacket: ServerType:{serverType} PacketId:{packetId} is already added");
        }

        public void BlacklistPacket(ServerType serverType, ushort packetId)
        {
            if (!AddToServerTypeList(_serverTypeBlacklist, serverType, packetId)) _Logger.Error($"BlacklistPacket: ServerType:{serverType} PacketId:{packetId} is already added");
        }

        public void Pause()
        {
            _paused = true;
        }

        public void Continue()
        {
            _continue = true;
            while (_logQueue.TryDequeue(out Log log)) WriteLog(log);

            _paused = false;
            _continue = false;
        }

        private void LoadSwitches()
        {
            switches.Add(
                new SwitchProperty<bool>(
                    "--no-data",
                    "--no-data=true (true|false)",
                    "Don't display packet data",
                    bool.TryParse,
                    result => noData = result
                )
            );
            switches.Add(
                new SwitchProperty<int>(
                    "--max-packet-size",
                    "--max-packet-size=64 (integer)",
                    "Don't display packet data",
                    int.TryParse,
                    result => maxPacketSize = result
                )
            );
            switches.Add(
                new SwitchProperty<int>(
                    "--log-level",
                    "--log-level=20 (integer) [Debug=10, Info=20, Error=30]",
                    "Only display logs of the same level or above",
                    int.TryParse,
                    result => minLogLevel = result
                )
            );
            switches.Add(
                new SwitchProperty<object>(
                    "--clear",
                    "--clear",
                    "Resets all switches to default",
                    SwitchProperty<object>.noOp,
                    result => Reset()
                )
            );
            switches.Add(
                new SwitchProperty<List<Tuple<ServerType?, ushort>>>(
                    "--b-list",
                    "--b-list=1:1000,2000,3:0xAA (ServerType:PacketId[0xA|10] | PacketId[0xA|10]) [Auth=1, Msg=2, Area=3]",
                    "A blacklist that does not logs packets specified",
                    TryParsePacketIdList,
                    results => { AssinPacketIdList(results, BlacklistPacket, BlacklistPacket); }
                )
            );
            switches.Add(
                new SwitchProperty<List<Tuple<ServerType?, ushort>>>(
                    "--w-list",
                    "--w-list=1:1000,2000,3:0xAA (ServerType:PacketId[0xA|10] | PacketId[0xA|10]) [Auth=1, Msg=2, Area=3]",
                    "A whitelist that only logs packets specified",
                    TryParsePacketIdList,
                    results => { AssinPacketIdList(results, WhitelistPacket, WhitelistPacket); }
                )
            );
        }

        /// <summary>
        ///     parses strings like "1:1000,2000,3:4000" into ServerType and PacketId
        /// </summary>
        private bool TryParsePacketIdList(string value, out List<Tuple<ServerType?, ushort>> result)
        {
            if (string.IsNullOrEmpty(value))
            {
                result = null;
                return false;
            }

            string[] values = value.Split(",");

            result = new List<Tuple<ServerType?, ushort>>();
            foreach (string entry in values)
                if (entry.Contains(":"))
                {
                    string[] keyValue = entry.Split(":");
                    if (keyValue.Length != 2) return false;

                    if (!byte.TryParse(keyValue[0], out byte serverTypeValue)) return false;

                    if (!Enum.IsDefined(typeof(ServerType), serverTypeValue)) return false;

                    ServerType serverType = (ServerType)serverTypeValue;

                    NumberStyles numberStyles;
                    if (keyValue[1].StartsWith("0x"))
                    {
                        keyValue[1] = keyValue[1].Substring(2);
                        numberStyles = NumberStyles.HexNumber;
                    }
                    else
                    {
                        numberStyles = NumberStyles.Integer;
                    }

                    if (!ushort.TryParse(keyValue[1], numberStyles, null, out ushort val)) return false;

                    result.Add(new Tuple<ServerType?, ushort>(serverType, val));
                }
                else
                {
                    NumberStyles numberStyles;
                    string entryStr;
                    if (entry.StartsWith("0x"))
                    {
                        entryStr = entry.Substring(2);
                        numberStyles = NumberStyles.HexNumber;
                    }
                    else
                    {
                        entryStr = entry;
                        numberStyles = NumberStyles.Integer;
                    }


                    if (!ushort.TryParse(entryStr, numberStyles, null, out ushort val)) return false;

                    result.Add(new Tuple<ServerType?, ushort>(null, val));
                }

            return true;
        }

        private void AssinPacketIdList(List<Tuple<ServerType?, ushort>> results,
            Action<ServerType, ushort> addToServerTypeList,
            Action<ushort> addToPacketList)
        {
            foreach (Tuple<ServerType?, ushort> entry in results)
                if (entry.Item1.HasValue)
                    addToServerTypeList(entry.Item1.Value, entry.Item2);
                else
                    addToPacketList(entry.Item2);
        }

        private bool AddToServerTypeList(Dictionary<ServerType, HashSet<ushort>> dictionary, ServerType serverType,
            ushort packetId)
        {
            HashSet<ushort> hashSet;
            if (dictionary.ContainsKey(serverType))
            {
                hashSet = dictionary[serverType];
            }
            else
            {
                hashSet = new HashSet<ushort>();
                dictionary.Add(serverType, hashSet);
            }

            if (hashSet.Contains(packetId)) return false;

            return hashSet.Add(packetId);
        }

        private void LogProviderOnGlobalLogWrite(object sender, LogWriteEventArgs logWriteEventArgs)
        {
            while (_continue) Thread.Sleep(10000);

            if (_paused)
            {
                _logQueue.Enqueue(logWriteEventArgs.Log);
                return;
            }

            WriteLog(logWriteEventArgs.Log);
        }

        private void WriteLog(Log log)
        {
            ConsoleColor consoleColor;
            string text;

            object tag = log.Tag;
            if (tag is NecLogPacket logPacket)
            {
                switch (logPacket.logType)
                {
                    case NecLogType.PacketIn:
                        consoleColor = ConsoleColor.Green;
                        break;
                    case NecLogType.PacketOut:
                        consoleColor = ConsoleColor.Blue;
                        break;
                    case NecLogType.PacketUnhandled:
                        consoleColor = ConsoleColor.Red;
                        break;
                    case NecLogType.PacketError:
                        consoleColor = ConsoleColor.Yellow;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = CreatePacketLog(logPacket);
            }
            else
            {
                LogLevel logLevel = log.LogLevel;
                if ((int)logLevel < minLogLevel) return;

                switch (logLevel)
                {
                    case LogLevel.Debug:
                        consoleColor = ConsoleColor.DarkCyan;
                        break;
                    case LogLevel.Info:
                        consoleColor = ConsoleColor.Cyan;
                        break;
                    case LogLevel.Error:
                        consoleColor = ConsoleColor.Red;
                        break;
                    default:
                        consoleColor = ConsoleColor.Gray;
                        break;
                }

                text = log.ToString();
            }

            if (text == null) return;

            lock (_consoleLock)
            {
                Console.ForegroundColor = consoleColor;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        private bool ExcludeLog(ServerType serverType, ushort packetId)
        {
            bool useWhitelist = _serverTypeWhitelist.Count > 0 || _packetIdWhitelist.Count > 0;

            bool whitelisted = _serverTypeWhitelist.ContainsKey(serverType)
                               && _serverTypeWhitelist[serverType].Contains(packetId)
                               || _packetIdWhitelist.Contains(packetId);
            bool blacklisted = _serverTypeBlacklist.ContainsKey(serverType)
                               && _serverTypeBlacklist[serverType].Contains(packetId)
                               || _packetIdBlacklist.Contains(packetId);

            if (useWhitelist && whitelisted) return false;

            if (useWhitelist) return true;

            if (blacklisted) return true;

            return false;
        }

        private string CreatePacketLog(NecLogPacket logPacket)
        {
            ServerType serverType = logPacket.serverType;
            ushort packetId = logPacket.id;

            if (ExcludeLog(serverType, packetId)) return null;

            int dataSize = logPacket.data.Size;

            StringBuilder sb = new StringBuilder();
            sb.Append($"{logPacket.clientIdentity} Packet Log");
            sb.Append(Environment.NewLine);
            sb.Append("----------");
            sb.Append(Environment.NewLine);
            sb.Append($"[{logPacket.timeStamp:HH:mm:ss}][Typ:{logPacket.logType}]");
            sb.Append($"[{serverType}]");
            sb.Append(Environment.NewLine);
            sb.Append(
                $"[Id:0x{packetId:X2}|{packetId}][Len(Data/Total):{dataSize}/{dataSize + logPacket.header.Length}][Header:{logPacket.headerHex}]");
            sb.Append($"[{logPacket.packetIdName}]");
            sb.Append(Environment.NewLine);

            if (!noData)
            {
                IBuffer data = logPacket.data;
                int maxPacketSize = this.maxPacketSize;
                if (maxPacketSize > 0 && dataSize > maxPacketSize)
                {
                    data = data.Clone(0, maxPacketSize);

                    sb.Append($"- Truncated Data showing {maxPacketSize} of {dataSize} bytes");
                    sb.Append(Environment.NewLine);
                }

                sb.Append("ASCII:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToAsciiString("  "));
                sb.Append(Environment.NewLine);
                sb.Append("HEX:");
                sb.Append(Environment.NewLine);
                sb.Append(data.ToHexString(" "));
                sb.Append(Environment.NewLine);
            }

            sb.Append("----------");

            return sb.ToString();
        }
    }
}
