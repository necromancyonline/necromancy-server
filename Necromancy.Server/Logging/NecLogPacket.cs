using System;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Logging
{
    public class NecLogPacket : NecPacket
    {
        public NecLogPacket(NecClient client, NecPacket packet, NecLogType logType, string identity)
            : base(packet.Id, packet.Data.Clone())
        {
            Identity = identity;
            Header = packet.Header;
            LogType = logType;
            TimeStamp = DateTime.UtcNow;
            Client = client;
        }

        public NecLogPacket(NecClient client, NecPacket packet, NecLogType logType)
            : this(client, packet, logType, null)
        {
        }

        public NecClient Client { get; }
        public NecLogType LogType { get; }
        public DateTime TimeStamp { get; }
        public string Identity { get; }
        public string Hex => Data.ToHexString('-');
        public string Ascii => Data.ToAsciiString(true);
        public string HeaderHex => Util.ToHexString(Header, '-');

        public string ToLogText()
        {
            String log = $"{Client.Identity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{TimeStamp:HH:mm:ss}]";
            if (Identity != null)
            {
                log += $"[{Identity}]";
            }

            log += $"[Typ:{LogType}][Id:0x{Id:X2}|{Id}][Len:{Data.Size}][Header:{HeaderHex}]";
            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += Ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += Hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}