using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_repair : ClientHandler
    {
        public send_shop_repair(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_shop_repair;
        public override void Handle(NecClient client, NecPacket packet)
        {
            List<ItemLocation> itemLocations = new List<ItemLocation>();
            int repairCount = packet.Data.ReadInt32();
            for (int i = 0; i < repairCount; i++)
            {
                ItemZoneType zone = (ItemZoneType)packet.Data.ReadByte();
                byte bag = packet.Data.ReadByte();
                short slot = packet.Data.ReadInt16();

                ItemLocation location = new ItemLocation(zone, bag, slot);
                itemLocations.Add(location);                
            }
            long repairFee = packet.Data.ReadInt64();

            ItemService itemService = new ItemService(client.Character);            
            int error = 0;
            try
            {                
                long currentGold = itemService.SubtractGold(repairFee); //TODO ignore the "repair fee" and check server side
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, currentGold);
                Router.Send(recvSelfMoneyNotify);

                List<ItemInstance> repairedItems = itemService.Repair(itemLocations);
                foreach (ItemInstance repairedItem in repairedItems)
                {
                    repairedItem.CurrentDurability = repairedItem.MaximumDurability;
                    RecvItemUpdateDurability recvItemUpdateDurability = new RecvItemUpdateDurability(client, repairedItem);
                    Router.Send(recvItemUpdateDurability);
                }                                
            } catch (ItemException e) { error = (int) e.ExceptionType; }

            RecvShopRepair recvShopRepair = new RecvShopRepair(client, error);
            Router.Send(recvShopRepair);
        }
    }
}
