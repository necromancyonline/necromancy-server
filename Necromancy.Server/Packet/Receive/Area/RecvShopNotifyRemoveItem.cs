using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopNotifyRemoveItem : PacketResponse
    {
        public RecvShopNotifyRemoveItem()
            : base((ushort) AreaPacketId.recv_shop_notify_remove_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            return res;
        }
    }
}
