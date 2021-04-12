using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlaceChange : PacketResponse
    {
        private readonly ItemInstance _destItem;
        private readonly ItemInstance _originItem;

        public RecvItemUpdatePlaceChange(NecClient client, ItemInstance originItem, ItemInstance destItem)
            : base((ushort)AreaPacketId.recv_item_update_place_change, ServerType.Area)
        {
            _originItem = originItem;
            _destItem = destItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_originItem.instanceId);
            res.WriteByte((byte)_originItem.location.zoneType);
            res.WriteByte(_originItem.location.container);
            res.WriteInt16(_originItem.location.slot);
            res.WriteUInt64(_destItem.instanceId);
            res.WriteByte((byte)_destItem.location.zoneType);
            res.WriteByte(_destItem.location.container);
            res.WriteInt16(_destItem.location.slot);
            return res;
        }
    }
}
