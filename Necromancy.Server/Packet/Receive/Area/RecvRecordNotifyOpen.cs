using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRecordNotifyOpen : PacketResponse
    {
        public RecvRecordNotifyOpen()
            : base((ushort)AreaPacketId.recv_record_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            int numEntries = 0xE;
            for (int i = 0; i < numEntries; i++) res.WriteInt16(0);

            int numEntries2 = 0x4;
            for (int i = 0; i < numEntries2; i++) res.WriteInt32(0);
            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt32(0);

            int numEntries3 = 0x6;
            for (int i = 0; i < numEntries3; i++) res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt64(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
