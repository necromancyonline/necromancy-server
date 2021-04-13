using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopIdentify : ClientHandler
    {
        public SendShopIdentify(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_shop_identify;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            //9 bytes left TODO investigate, probably one is identify price which is irrelevant, check price server side

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.character);
            ItemInstance identifiedItem;
            int error = 0;

            try
            {
                identifiedItem = itemService.GetIdentifiedItem(location);
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, identifiedItem);
                router.Send(recvItemInstance);
            }
            catch (ItemException e)
            {
                error = (int)e.type;
            }

            RecvShopIdentify recvShopIdentify = new RecvShopIdentify(client, error);
            router.Send(recvShopIdentify);
        }
    }
}
