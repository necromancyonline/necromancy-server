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
            Logger.Debug($"Transferred items {clientItemCount}:{targetClientItemcount}");
            Logger.Debug($"Free Space{client.Character.ItemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag)}:{targetClient.Character.ItemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag)}");

            //ToDo:  improve this logic when GetTotalFreeSpace can check all bags
            if (client.Character.ItemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag) < (targetClientItemcount-clientItemCount)) notifyProblemSysMsg = 1; //doesnt work. GetTotalFreeSpace is not accurate. problem with itemService.PutLootedItem updating count.
            if (targetClient.Character.ItemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag) < (clientItemCount-targetClientItemcount)) notifyProblemSysMsg = 1;
            //ToDo:  add other trade preventing scenarios here


            RecvTradeNotifyProblem recvTradeNotifyProblem = new RecvTradeNotifyProblem(objectId:0, notifyProblemSysMsg);
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
                    ItemInstance itemInstance = targetClient.Character.ItemLocationVerifier.GetItemByInstanceId(targetClient.Character.TradeWindowSlot[i]);
                    if (itemInstance != null)
                    {
                        RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, itemInstance);
                        if (targetClient != null) Router.Send(recvItemRemove);

                        targetClient.Character.ItemLocationVerifier.RemoveItem(itemInstance);

                        //put the item in the new owners inventory
                        itemInstance = itemService.PutLootedItem(itemInstance);

                        RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                        Router.Send(client, recvItemInstance.ToPacket());
                    }
                //}
                //give stuff to targetClient
                //for (int i = 0; i < 20; i++)
                //{
                    ItemInstance itemInstance2 = client.Character.ItemLocationVerifier.GetItemByInstanceId(client.Character.TradeWindowSlot[i]);
                    if (itemInstance2 != null)
                    {
                        RecvItemRemove recvItemRemove2 = new RecvItemRemove(client, itemInstance2);
                        if (client != null) Router.Send(recvItemRemove2);

                        client.Character.ItemLocationVerifier.RemoveItem(itemInstance2);

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
                        if (targetClient != null) Router.Send(targetClient, eventEnd.ToPacket());
                        Router.Send(client, eventEnd.ToPacket());

                    }
                );
            }
        }

    }
}
