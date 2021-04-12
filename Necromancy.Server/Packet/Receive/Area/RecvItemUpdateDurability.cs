using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateDurability : PacketResponse
    {
        private readonly ItemInstance _durabilityChangedItem;

        public RecvItemUpdateDurability(NecClient client, ItemInstance durabilityChangedItem)
            : base((ushort)AreaPacketId.recv_item_update_durability, ServerType.Area)
        {
            _durabilityChangedItem = durabilityChangedItem;
            Clients.Add(client);
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_durabilityChangedItem.instanceId);
            res.WriteInt32(_durabilityChangedItem.currentDurability);
            return res;
        }
    }
}
