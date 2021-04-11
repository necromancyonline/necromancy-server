using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvQuestNotifyOrderBonusRate : PacketResponse
    {
        public RecvQuestNotifyOrderBonusRate()
            : base((ushort) AreaPacketId.recv_quest_notify_order_bonus_rate, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
