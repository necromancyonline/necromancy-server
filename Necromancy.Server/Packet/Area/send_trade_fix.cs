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
            foreach (ItemLocation itemLocation in targetClient.Character.ItemManager.GetTradeItemLocations().Values)
            {
                Logger.Debug($"Client trading : {itemLocation.ZoneType}{itemLocation.Container}{itemLocation.Slot}");
                //if (!itemLocation.Equals(ItemLocation.InvalidLocation))
                {
                    ItemInstance iteminstance = targetItemService.GetIdentifiedItem(itemLocation);
                    //remove the icon from the deadClient's inventory if they are online.
                    RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, iteminstance);
                    if (targetClient != null) Router.Send(recvItemRemove);

                    //this is important.
                    //iteminstance.Location = new ItemLocation(ItemZoneType.InvalidZone, 0, 0);

                    //put the item in the new owners inventory
                    iteminstance = itemService.PutLootedItem(iteminstance);

                    RecvItemInstance recvItemInstance = new RecvItemInstance(client, iteminstance);
                    Router.Send(client, recvItemInstance.ToPacket());
                }
            }
            //give stuff to targetClient
            foreach (ItemLocation itemLocation in client.Character.ItemManager.GetTradeItemLocations().Values)
            {
                Logger.Debug($"targetClient trading : {itemLocation.ZoneType}{itemLocation.Container}{itemLocation.Slot}");
                ///if (!itemLocation.Equals(ItemLocation.InvalidLocation))
                {
                    ItemInstance iteminstance = itemService.GetIdentifiedItem(itemLocation);
                    //remove the icon from the deadClient's inventory if they are online.
                    RecvItemRemove recvItemRemove = new RecvItemRemove(client, iteminstance);
                    if (client != null) Router.Send(recvItemRemove);

                    //this is important.
                    //iteminstance.Location = new ItemLocation(ItemZoneType.InvalidZone, 0, 0);

                    //put the item in the new owners inventory
                    iteminstance = itemService.PutLootedItem(iteminstance);

                    RecvItemInstance recvItemInstance = new RecvItemInstance(targetClient, iteminstance);
                    if (targetClient != null) Router.Send(targetClient, recvItemInstance.ToPacket());
                }
            }

            RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
            if (targetClient != null) Router.Send(targetClient, recvSituationEnd.ToPacket());
            Router.Send(client, recvSituationEnd.ToPacket());

            targetClient.Character.eventSelectExecCode = 0;
            client.Character.eventSelectExecCode = 0;

            client.Character.ItemManager.TradeEnd();
            targetClient.Character.ItemManager.TradeEnd();
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
