using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerExpeditionCashShopCloseR : PacketResponse
    {
        public RecvSoulPartnerExpeditionCashShopCloseR()
            : base((ushort) AreaPacketId.recv_soul_partner_expedition_cash_shop_close_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            return res;
        }
    }
}
