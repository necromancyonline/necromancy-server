using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerStorageCashShopCloseR : PacketResponse
    {
        public RecvSoulPartnerStorageCashShopCloseR()
            : base((ushort)AreaPacketId.recv_soul_partner_storage_cash_shop_close_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //errcheck
            return res;
        }
    }
}
