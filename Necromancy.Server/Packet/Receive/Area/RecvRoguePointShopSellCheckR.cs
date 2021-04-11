using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguePointShopSellCheckR : PacketResponse
    {
        public RecvRoguePointShopSellCheckR()
            : base((ushort) AreaPacketId.recv_rogue_point_shop_sell_check_r, ServerType.Area)
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
