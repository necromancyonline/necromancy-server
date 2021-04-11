using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopRepair : ClientHandler
    {
        public SendShopRepair(NecServer server) : base(server) { }
        public override ushort id => (ushort) AreaPacketId.send_shop_repair;
        public override void Handle(NecClient client, NecPacket packet)
        {
            List<ItemLocation> itemLocations = new List<ItemLocation>();
            int repairCount = packet.data.ReadInt32();
            for (int i = 0; i < repairCount; i++)
            {
                ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
                byte bag = packet.data.ReadByte();
                short slot = packet.data.ReadInt16();

                ItemLocation location = new ItemLocation(zone, bag, slot);
                itemLocations.Add(location);
            }
            ulong repairFee = packet.data.ReadUInt64();

            ItemService itemService = new ItemService(client.character);
            int error = 0;
            try
            {
                ulong currentGold = itemService.SubtractGold(repairFee); //TODO ignore the "repair fee" and check server side
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, currentGold);
                router.Send(recvSelfMoneyNotify);

                List<ItemInstance> repairedItems = itemService.Repair(itemLocations);
                foreach (ItemInstance repairedItem in repairedItems)
                {
                    repairedItem.currentDurability = repairedItem.maximumDurability;
                    RecvItemUpdateDurability recvItemUpdateDurability = new RecvItemUpdateDurability(client, repairedItem);
                    router.Send(recvItemUpdateDurability);
                }
            } catch (ItemException e) { error = (int) e.type; }

            RecvShopRepair recvShopRepair = new RecvShopRepair(client, error);
            router.Send(recvShopRepair);
        }
    }
}
