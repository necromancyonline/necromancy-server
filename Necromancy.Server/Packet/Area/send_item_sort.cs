using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_sort : ClientHandler
    {
        public send_item_sort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_item_sort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zoneType = (ItemZoneType) packet.Data.ReadInt32();
            byte container = packet.Data.ReadByte();
            int itemsToSort = packet.Data.ReadInt32();
            short[] fromSlots = new short[itemsToSort];
            short[] toSlots = new short[itemsToSort];
            short[] quantities = new short[itemsToSort];
            for (int i = 0; i < itemsToSort; i++)
            {
                fromSlots[i] = packet.Data.ReadInt16();
                toSlots[i] = packet.Data.ReadInt16();
                quantities[i] = packet.Data.ReadInt16();
            }

            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try
            {
                ItemInstance[] sortedItems = itemService.Sort(zoneType, container, fromSlots, toSlots, quantities);
                foreach(ItemInstance item in sortedItems)
                {
                    RecvItemUpdatePlace recvItemUpdatePlace = new RecvItemUpdatePlace(client, item);
                    Router.Send(recvItemUpdatePlace);
                }
            }
            catch (ItemException e) { error = (int)e.ExceptionType; }
            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            Router.Send(client, (ushort) AreaPacketId.recv_item_sort_r, res, ServerType.Area);
        }
    }
}
