using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopGetUrlCommon : PacketResponse
    {
        public RecvCashShopGetUrlCommon()
            : base((ushort)AreaPacketId.recv_cash_shop_get_url_common_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteCString(""); //Length is 0x801
            return res;
        }
    }
}
