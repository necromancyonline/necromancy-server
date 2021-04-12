using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvVenturerMedalShopNotifyRemoveItem : PacketResponse
    {
        public RecvVenturerMedalShopNotifyRemoveItem()
            : base((ushort)AreaPacketId.recv_venturer_medal_shop_notify_remove_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(1); //last chance if shop does not contain item idx

            return res;
        }
    }
}
