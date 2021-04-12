using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvClassAdvancementNotifyParam : PacketResponse
    {
        public RecvClassAdvancementNotifyParam()
            : base((ushort)AreaPacketId.recv_class_advancement_notify_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;

            res.WriteByte(0);
            res.WriteInt32(numEntries); // less than 0xA
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                for (int i = 0; i < 0x8; i++) res.WriteInt64(0);
                for (int i = 0; i < 0x3; i++) res.WriteInt32(0);
            }

            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
