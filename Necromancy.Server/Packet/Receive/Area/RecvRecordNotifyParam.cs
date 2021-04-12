using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRecordNotifyParam : PacketResponse
    {
        public RecvRecordNotifyParam()
            : base((ushort)AreaPacketId.recv_record_notify_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); // must be under 0x12c

            int numEntries = 0x1;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("----", 0x5B);
                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("----", 0x5B);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("----", 0x5B);
            }

            return res;
        }
    }
}
