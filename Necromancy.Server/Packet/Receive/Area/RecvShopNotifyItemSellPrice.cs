using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopNotifyItemSellPrice : PacketResponse
    {
        public RecvShopNotifyItemSellPrice()
            : base((ushort)AreaPacketId.recv_shop_notify_item_sell_price, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0); // id?

            res.WriteInt64(0); // price?

            res.WriteInt64(0); // identify?

            res.WriteInt64(0); // curse?

            res.WriteInt64(0); // repair?
            return res;
        }
    }
}
