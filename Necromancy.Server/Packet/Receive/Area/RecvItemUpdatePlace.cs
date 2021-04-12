using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePlace : PacketResponse
    {
        private readonly ItemInstance _movedItem;

        public RecvItemUpdatePlace(NecClient client, ItemInstance movedItem)
            : base((ushort)AreaPacketId.recv_item_update_place, ServerType.Area)
        {
            _movedItem = movedItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_movedItem.instanceId);
            res.WriteByte((byte)_movedItem.location.zoneType);
            res.WriteByte(_movedItem.location.container);
            res.WriteInt16(_movedItem.location.slot);
            return res;
        }
    }
}
