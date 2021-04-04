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
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            ItemService itemService = new ItemService(client.Character);
            ItemService targetItemService = new ItemService(targetClient.Character);

            RecvTradeNotifyFixed notifyFixed = new RecvTradeNotifyFixed(0);
            if (targetClient != null) Router.Send(targetClient, notifyFixed.ToPacket());
            Router.Send(client, notifyFixed.ToPacket());

            RecvTradeFix recvTradeFix = new RecvTradeFix();
            if (targetClient != null) Router.Send(targetClient, recvTradeFix.ToPacket());
            Router.Send(client, recvTradeFix.ToPacket());

            RecvTradeNotifyReverted notifyReverted = new RecvTradeNotifyReverted();
            Router.Send(targetClient, notifyReverted.ToPacket());
            //Router.Send(client, recvTradeFix.ToPacket());

            RecvSituationStart recvSituationStart = new RecvSituationStart(2);
            if (targetClient != null) Router.Send(targetClient, recvSituationStart.ToPacket());
            Router.Send(client, recvSituationStart.ToPacket());



            //Get stuff from targetClient
            for (int i = 0; i < 20; i++)
            {
                ItemInstance itemInstance = targetClient.Character.ItemManager.GetItemByInstanceId(targetClient.Character.TradeWindowSlot[i]);
                if (itemInstance != null)
                {
                    //ItemInstance iteminstance = targetItemService.GetIdentifiedItem(itemLocation);
                    RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, itemInstance);
                    if (targetClient != null) Router.Send(recvItemRemove);

                    targetClient.Character.ItemManager.RemoveItem(itemInstance);

                    //put the item in the new owners inventory
                    itemInstance = itemService.PutLootedItem(itemInstance);

                    RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                    Router.Send(client, recvItemInstance.ToPacket());
                }
            }
            //give stuff to targetClient
            for (int i = 0; i < 20; i++)
            {
                ItemInstance itemInstance = client.Character.ItemManager.GetItemByInstanceId(client.Character.TradeWindowSlot[i]);
                if (itemInstance != null)
                {
                    RecvItemRemove recvItemRemove = new RecvItemRemove(client, itemInstance);
                    if (client != null) Router.Send(recvItemRemove);

                    client.Character.ItemManager.RemoveItem(itemInstance);

                    //put the item in the new owners inventory
                    itemInstance = targetItemService.PutLootedItem(itemInstance);

                    RecvItemInstance recvItemInstance = new RecvItemInstance(targetClient, itemInstance);
                    if (targetClient != null) Router.Send(targetClient, recvItemInstance.ToPacket());
                }
            }

            RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
            if (targetClient != null) Router.Send(targetClient, recvSituationEnd.ToPacket());
            Router.Send(client, recvSituationEnd.ToPacket());

            targetClient.Character.eventSelectExecCode = 0;
            client.Character.eventSelectExecCode = 0;

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
}
