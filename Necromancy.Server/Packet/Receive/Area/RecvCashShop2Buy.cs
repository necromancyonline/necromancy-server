using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShop2Buy : PacketResponse
    {
        public RecvCashShop2Buy()
            : base((ushort) AreaPacketId.recv_cash_shop2_buy_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            return res;
        }
    }
}