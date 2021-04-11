using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_item_drop : ClientHandler
    {
        public send_item_drop(NecServer server) : base(server)  { } 
        public override ushort Id => (ushort) AreaPacketId.send_item_drop;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType) packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);
            int error = 0;

            try { 
                ItemInstance item = itemService.Remove(location, quantity);
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, item);
                Router.Send(recvItemRemove);
            } catch(ItemException e) { error = (int) e.Type; }

            RecvItemDrop recvItemDrop = new RecvItemDrop(client, error);
            Router.Send(recvItemDrop);
        }
    }
}
