using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopGetUrl : PacketResponse
    {
        public RecvCashShopGetUrl()
            : base((ushort)AreaPacketId.recv_cash_shop_get_url_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("ToBeFound"); // find max size
            return res;
        }
    }
}
