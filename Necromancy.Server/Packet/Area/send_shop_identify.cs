using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_identify : ClientHandler
    {
        public send_shop_identify(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_shop_identify;
        public override void Handle(NecClient client, NecPacket packet)
        {         
            ItemZoneType zone = (ItemZoneType) packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16(); 
            //9 bytes left TODO investigate, probably one is identify price which is irrelevant, check price server side

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);
            ItemInstance identifiedItem;
            int error = 0;

            try {  
                identifiedItem = itemService.GetIdentifiedItem(location);
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, identifiedItem);
                Router.Send(recvItemInstance);
            } catch(ItemException e) { error = (int) e.Type; }

            RecvShopIdentify recvShopIdentify = new RecvShopIdentify(client, error);
            Router.Send(recvShopIdentify);            
        }
    }
}
