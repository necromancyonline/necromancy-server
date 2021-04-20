using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopGetUrlCommonSteam : PacketResponse
    {
        public RecvCashShopGetUrlCommonSteam()
            : base((ushort)AreaPacketId.recv_cash_shop_get_url_common_steam_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteCString("ToBeFound");

            res.WriteCString("ToBeFound");

            res.WriteCString("ToBeFound");
            return res;
        }
    }
}
