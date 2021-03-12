using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlaceChange : PacketResponse
    {
        private readonly ItemInstance _originItem;
        private readonly ItemInstance _destItem;
        public RecvItemUpdatePlaceChange(NecClient client, ItemInstance originItem, ItemInstance destItem)
            : base((ushort) AreaPacketId.recv_item_update_place_change, ServerType.Area)
        {
            _originItem = originItem;
            _destItem = destItem;
            Clients.Add(client);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_originItem.InstanceID);
            res.WriteByte((byte)_originItem.Location.ZoneType);
            res.WriteByte(_originItem.Location.Container);
            res.WriteInt16(_originItem.Location.Slot);
            res.WriteUInt64(_destItem.InstanceID);
            res.WriteByte((byte)_destItem.Location.ZoneType);
            res.WriteByte(_destItem.Location.Container);
            res.WriteInt16(_destItem.Location.Slot);
            return res;
        }
    }
}
