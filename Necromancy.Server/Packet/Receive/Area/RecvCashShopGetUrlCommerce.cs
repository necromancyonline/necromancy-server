using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopGetUrlCommerce : PacketResponse
    {
        public RecvCashShopGetUrlCommerce()
            : base((ushort)AreaPacketId.recv_cash_shop_get_url_commerce_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); //"0x801"
            res.WriteCString(""); //"0x801"
            res.WriteCString(""); //"0x11"
            return res;
        }
    }
}
