using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfBonusMoneyNotify : PacketResponse
    {
        public RecvSelfBonusMoneyNotify()
            : base((ushort)AreaPacketId.recv_self_bonus_money_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);

            return res;
        }
    }
}
