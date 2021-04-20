using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopFittingItemUpdateEqmask : PacketResponse
    {
        public RecvCashShopFittingItemUpdateEqmask()
            : base((ushort)AreaPacketId.recv_cash_shop_fitting_item_update_eqmask, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            res.WriteInt32(0);
            return res;
        }
    }
}
