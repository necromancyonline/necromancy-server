using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendItemDrop : ClientHandler
    {
        public SendItemDrop(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_item_drop;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            byte quantity = packet.data.ReadByte();

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.character);
            int error = 0;

            try
            {
                ItemInstance item = itemService.Remove(location, quantity);
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, item);
                router.Send(recvItemRemove);
            }
            catch (ItemException e)
            {
                error = (int)e.type;
            }

            RecvItemDrop recvItemDrop = new RecvItemDrop(client, error);
            router.Send(recvItemDrop);
        }
    }
}
