using System;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;

namespace Necromancy.Server.Logging
{
    public class NecLogPacket : NecPacket
    {
        public NecLogPacket(string clientIdentity, NecPacket packet, NecLogType logType, ServerType serverType)
            : base(packet.id, packet.data.Clone(), serverType)
        {
            header = packet.header;
            this.logType = logType;
            timeStamp = DateTime.Now;
            this.clientIdentity = clientIdentity;
        }

        public string clientIdentity { get; }
        public NecLogType logType { get; }
        public DateTime timeStamp { get; }
        public string hex => data.ToHexString(" ");
        public string ascii => data.ToAsciiString("  ");
        public string headerHex => Util.ToHexString(header, '-');

        public string ToLogText()
        {
            String log = $"{clientIdentity} Packet Log";
            log += Environment.NewLine;
            log += "----------";
            log += Environment.NewLine;
            log += $"[{timeStamp:HH:mm:ss}][Typ:{logType}]";
            log += $"[{serverType}]";
            log += Environment.NewLine;
            log += $"[Id:0x{id:X2}|{id}][Len(Data/Total):{data.Size}/{data.Size + header.Length}][Header:{headerHex}]";
            log += $"[{packetIdName}]";
            log += Environment.NewLine;
            log += "ASCII:";
            log += Environment.NewLine;
            log += ascii;
            log += Environment.NewLine;
            log += "HEX:";
            log += Environment.NewLine;
            log += hex;
            log += Environment.NewLine;
            log += "----------";
            return log;
        }
    }
}
