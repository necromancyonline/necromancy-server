using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopTitlePush : PacketResponse
    {
        public RecvShopTitlePush()
            : base((ushort)AreaPacketId.recv_shop_title_push, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ShopTitle");
            return res;
        }
    }
}
