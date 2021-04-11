using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvClassAdvancementNotifyAddExp : PacketResponse
    {
        public RecvClassAdvancementNotifyAddExp()
            : base((ushort) AreaPacketId.recv_class_advancement_notify_add_exp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
