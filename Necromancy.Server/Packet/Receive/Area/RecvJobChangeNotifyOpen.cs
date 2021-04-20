using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvJobChangeNotifyOpen : PacketResponse
    {
        public RecvJobChangeNotifyOpen()
            : base((ushort)AreaPacketId.recv_job_change_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x7;
            res.WriteInt32(numEntries); //less than or equal to 0x7
            for (int i = 0; i < numEntries; i++) res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            for (int i = 0; i < 0x7; i++) res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 0x7; i++) res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            for (int i = 0; i < 0x10; i++) res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
