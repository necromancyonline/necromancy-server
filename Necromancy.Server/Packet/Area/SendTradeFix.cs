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
    public class SendTradeFix : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendTradeFix));

        public SendTradeFix(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_trade_fix;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte notifyProblemSysMsg = 0;
            int clientItemCount = 0;
            int targetClientItemcount = 0;

            NecClient targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);
            ItemService itemService = new ItemService(client.character);
            ItemService targetItemService = new ItemService(targetClient.character);

            RecvTradeNotifyFixed notifyFixed = new RecvTradeNotifyFixed(0);
            RecvTradeFix recvTradeFix = new RecvTradeFix();

            for (int i = 0; i < 20; i++)
            {
                if (client.character.tradeWindowSlot[i] > 0) clientItemCount++;
                if (targetClient.character.tradeWindowSlot[i] > 0) targetClientItemcount++;
            }
            _Logger.Debug($"Transferred items {clientItemCount}:{targetClientItemcount}");
            _Logger.Debug($"Free Space{client.character.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag)}:{targetClient.character.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag)}");

            //ToDo:  improve this logic when GetTotalFreeSpace can check all bags
            if (client.character.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag) < (targetClientItemcount-clientItemCount)) notifyProblemSysMsg = 1; //doesnt work. GetTotalFreeSpace is not accurate. problem with itemService.PutLootedItem updating count.
            if (targetClient.character.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag) < (clientItemCount-targetClientItemcount)) notifyProblemSysMsg = 1;
            //ToDo:  add other trade preventing scenarios here


            RecvTradeNotifyProblem recvTradeNotifyProblem = new RecvTradeNotifyProblem(objectId:0, notifyProblemSysMsg);
            router.Send(client, recvTradeNotifyProblem.ToPacket());
            if (targetClient != null) router.Send(targetClient, recvTradeNotifyProblem.ToPacket());


            if (notifyProblemSysMsg == 0)
            {
                //player 1 sends
                router.Send(client, notifyFixed.ToPacket());
                router.Send(client, recvTradeFix.ToPacket());

                //player 2 sends
                if (targetClient != null)
                {
                    router.Send(targetClient, notifyFixed.ToPacket());
                    router.Send(targetClient, recvTradeFix.ToPacket());
                }

                //Get stuff from targetClient
                for (int i = 0; i < 20; i++)
                {
                    ItemInstance itemInstance = targetClient.character.itemLocationVerifier.GetItemByInstanceId(targetClient.character.tradeWindowSlot[i]);
                    if (itemInstance != null)
                    {
                        RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, itemInstance);
                        if (targetClient != null) router.Send(recvItemRemove);

                        targetClient.character.itemLocationVerifier.RemoveItem(itemInstance);

                        //put the item in the new owners inventory
                        itemInstance = itemService.PutLootedItem(itemInstance);

                        RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                        router.Send(client, recvItemInstance.ToPacket());
                    }
                //}
                //give stuff to targetClient
                //for (int i = 0; i < 20; i++)
                //{
                    ItemInstance itemInstance2 = client.character.itemLocationVerifier.GetItemByInstanceId(client.character.tradeWindowSlot[i]);
                    if (itemInstance2 != null)
                    {
                        RecvItemRemove recvItemRemove2 = new RecvItemRemove(client, itemInstance2);
                        if (client != null) router.Send(recvItemRemove2);

                        client.character.itemLocationVerifier.RemoveItem(itemInstance2);

                        //put the item in the new owners inventory
                        itemInstance2 = targetItemService.PutLootedItem(itemInstance2);

                        RecvItemInstance recvItemInstance2 = new RecvItemInstance(targetClient, itemInstance2);
                        if (targetClient != null) router.Send(targetClient, recvItemInstance2.ToPacket());
                    }
                }

                client.character.tradeWindowSlot = new ulong[20];
                targetClient.character.tradeWindowSlot = new ulong[20];

                Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                (t1 =>
                    {
                        RecvEventEnd eventEnd = new RecvEventEnd(0);
                        if (targetClient != null) router.Send(targetClient, eventEnd.ToPacket());
                        router.Send(client, eventEnd.ToPacket());

                    }
                );
            }
        }

    }
}
