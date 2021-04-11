using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_quest_notify_order_bonus_rate : PacketResponse
    {
        public recv_quest_notify_order_bonus_rate()
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
