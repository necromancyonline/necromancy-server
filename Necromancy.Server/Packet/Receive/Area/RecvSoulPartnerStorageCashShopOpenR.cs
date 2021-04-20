using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerStorageCashShopOpenR : PacketResponse
    {
        public RecvSoulPartnerStorageCashShopOpenR()
            : base((ushort)AreaPacketId.recv_soul_partner_storage_cash_shop_open_r, ServerType.Area)
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
