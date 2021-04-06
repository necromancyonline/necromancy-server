using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;
using System.Threading.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_fix : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_trade_fix));

        public send_trade_fix(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_fix;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte notifyProblemSysMsg = 0;
            int clientItemCount = 0;
            int targetClientItemcount = 0;

            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);
            ItemService itemService = new ItemService(client.Character);
            ItemService targetItemService = new ItemService(targetClient.Character);

            RecvTradeNotifyFixed notifyFixed = new RecvTradeNotifyFixed(0);
            RecvTradeFix recvTradeFix = new RecvTradeFix();

            for (int i = 0; i < 20; i++)
            {
                if (client.Character.TradeWindowSlot[i] > 0) clientItemCount++;
                if (targetClient.Character.TradeWindowSlot[i] > 0) targetClientItemcount++;
            }
            Logger.Debug($"{clientItemCount}:{targetClientItemcount}");
            Logger.Debug($"{client.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag)}:{targetClient.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag)}");

            //ToDo:  improve this logic when GetTotalFreeSpace can check all bags
            //if ((int)client.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag) < targetClientItemcount) notifyProblemSysMsg = 1; //doesnt work. GetTotalFreeSpace is not accurate. problem with itemService.PutLootedItem updating count.
            //if ((int)targetClient.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag) < clientItemCount) notifyProblemSysMsg = 1;
            //ToDo:  add other trade preventing scenarios here


            RecvTradeNotifyProblem recvTradeNotifyProblem = new RecvTradeNotifyProblem(0,notifyProblemSysMsg);
            Router.Send(client, recvTradeNotifyProblem.ToPacket());    
            if (targetClient != null) Router.Send(targetClient, recvTradeNotifyProblem.ToPacket());


            if (notifyProblemSysMsg == 0)
            {
                //player 1 sends
                Router.Send(client, notifyFixed.ToPacket());
                Router.Send(client, recvTradeFix.ToPacket());

                //player 2 sends
                if (targetClient != null)
                {
                    Router.Send(targetClient, notifyFixed.ToPacket());
                    Router.Send(targetClient, recvTradeFix.ToPacket());
                }

                //Get stuff from targetClient
                for (int i = 0; i < 20; i++)
                {
                    ItemInstance itemInstance = targetClient.Character.ItemManager.GetItemByInstanceId(targetClient.Character.TradeWindowSlot[i]);
                    if (itemInstance != null)
                    {
                        RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, itemInstance);
                        if (targetClient != null) Router.Send(recvItemRemove);

                        targetClient.Character.ItemManager.RemoveItem(itemInstance);

                        //put the item in the new owners inventory
                        itemInstance = itemService.PutLootedItem(itemInstance);

                        RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                        Router.Send(client, recvItemInstance.ToPacket());
                    }
                //}
                //give stuff to targetClient
                //for (int i = 0; i < 20; i++)
                //{
                    ItemInstance itemInstance2 = client.Character.ItemManager.GetItemByInstanceId(client.Character.TradeWindowSlot[i]);
                    if (itemInstance2 != null)
                    {
                        RecvItemRemove recvItemRemove2 = new RecvItemRemove(client, itemInstance2);
                        if (client != null) Router.Send(recvItemRemove2);

                        client.Character.ItemManager.RemoveItem(itemInstance2);

                        //put the item in the new owners inventory
                        itemInstance2 = targetItemService.PutLootedItem(itemInstance2);

                        RecvItemInstance recvItemInstance2 = new RecvItemInstance(targetClient, itemInstance2);
                        if (targetClient != null) Router.Send(targetClient, recvItemInstance2.ToPacket());
                    }
                }

                client.Character.TradeWindowSlot = new ulong[20];
                targetClient.Character.TradeWindowSlot = new ulong[20];

                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                (t1 =>
                    {
                        RecvEventEnd eventEnd = new RecvEventEnd(0);
                        if (targetClient != null) Router.Send(eventEnd, targetClient);
                        Router.Send(eventEnd, client);

                    }
                );
            }
        }
        /*
         * #trade,,,,
            TRADE_CANCEL,GENERIC,The trade failed.,SYSTEM_NOTIFY,
            TRADE_CANCEL,-34,"If you are in stealth"","" you cannot apply for a trade.",SYSTEM_NOTIFY,
            TRADE_CANCEL,-3002,You cannot trade during the event.,SYSTEM_NOTIFY,
            TRADE_CANCEL,-4000,You cannot start trading because you are too far away from your opponent.,SYSTEM_NOTIFY,
            TRADE_NOT,GENERIC,This item cannot be traded.,SYSTEM_WARNING,
            TRADE_NOT,0,I succeeded in trading.,SYSTEM_WARNING,
            TRADE_NOT,1,I have already applied for a trade.,SYSTEM_NOTIFY,
            TRADE_NOT,2,We are already applying for a trade.,SYSTEM_NOTIFY,
            TRADE_NOT,3,You have exceeded the number that can be traded at one time.,SYSTEM_WARNING,
            TRADE_NOT,4,Items that are equipped cannot be traded.,SYSTEM_WARNING,
            TRADE_NOT,5,This item has already been selected.,SYSTEM_WARNING,
            TRADE_NOT,6,There is nothing to trade.,SYSTEM_WARNING,
            TRADE_NOT,7,The amount of money you have in %s has exceeded the upper limit.,SYSTEM_WARNING,
            TRADE_NOT,8,There is not enough free inventory in %s.,SYSTEM_WARNING,
            TRADE_NOT,9,You cannot get gold that exceeds the maximum amount of money you have.,SYSTEM_WARNING,
            TRADE_NOT,10,There is not enough free inventory.,SYSTEM_WARNING,
            TRADE_NOT,11,This item is on sale at a stall.,SYSTEM_WARNING,
            TRADE_NOT,12,The amount of money you have in %s is less than the amount you are offering.,SYSTEM_WARNING,
            TRADE_NOT,13,The item slot in %s is invalid.,SYSTEM_WARNING,
            TRADE_NOT,14,Your money is less than what you are offering.,SYSTEM_WARNING,
            TRADE_NOT,15,The item slot is invalid.,SYSTEM_WARNING,
            TRADE_NOT,16,Trading has already started.,SYSTEM_WARNING,
            TRADE_NOT,17,It cannot be operated while the contents are being posted.,SYSTEM_WARNING,
            TRADE_NOT,18,You cannot trade items in different inventories.,SYSTEM_WARNING,
            TRADE_NOT,-215,It is not your property.,SYSTEM_WARNING,
            TRADE_NOT,-4001,Items in the Avatar Inventory cannot be traded.,SYSTEM_WARNING,
            TRADE_NOT,-4002,You cannot trade items in different inventories at the same time.,SYSTEM_WARNING,
        */
    }
}
