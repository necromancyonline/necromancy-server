using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_fix : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_charabody_salvage_abort));

        public send_trade_fix(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_fix;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check?  to be tested
            Router.Send(client, (ushort) AreaPacketId.recv_trade_fix_r, res, ServerType.Area);


            {
                ItemService itemService = new ItemService(client.Character);
                ItemService targetItemService = new ItemService(targetClient.Character);
                //Get stuff from targetClient
                foreach (ItemLocation itemLocation in targetClient.Character.ItemManager.GetTradeItemLocations().Values)
                {
                    Logger.Debug("${ itemLocation}");
                    if (!itemLocation.Equals(ItemLocation.InvalidLocation))
                    {
                        ItemInstance iteminstance = targetItemService.GetIdentifiedItem(itemLocation);
                        //remove the icon from the deadClient's inventory if they are online.
                        RecvItemRemove recvItemRemove = new RecvItemRemove(targetClient, iteminstance);
                        if (targetClient != null) Router.Send(recvItemRemove);

                        //this is important.
                        iteminstance.Location = new ItemLocation(ItemZoneType.InvalidZone, 0, 0);

                        //put the item in the new owners inventory
                        itemService.PutLootedItem(iteminstance);

                        RecvItemInstance recvItemInstance = new RecvItemInstance(client, iteminstance);
                        Router.Send(client, recvItemInstance.ToPacket());
                    }
                }
                //give stuff to targetClient
                foreach (ItemLocation itemLocation in client.Character.ItemManager.GetTradeItemLocations().Values)
                {
                    if (!itemLocation.Equals(ItemLocation.InvalidLocation))
                    {
                        ItemInstance iteminstance = itemService.GetIdentifiedItem(itemLocation);
                        //remove the icon from the deadClient's inventory if they are online.
                        RecvItemRemove recvItemRemove = new RecvItemRemove(client, iteminstance);
                        if (client != null) Router.Send(recvItemRemove);

                        //this is important.
                        iteminstance.Location = new ItemLocation(ItemZoneType.InvalidZone,0,0);

                        //put the item in the new owners inventory
                        itemService.PutLootedItem(iteminstance);

                        RecvItemInstance recvItemInstance = new RecvItemInstance(targetClient, iteminstance);
                        Router.Send(targetClient, recvItemInstance.ToPacket());
                    }
                }
            }
            //Clean everything up and end everything
            RecvTradeNotifyFixed notifyFixed = new RecvTradeNotifyFixed();
            RecvTradeNotifyAborted notifyAborted = new RecvTradeNotifyAborted(); //Find a better recv for closing the trade windows at the end.  ToDo
            RecvEventEnd eventEnd = new RecvEventEnd(0);
            if (targetClient != null)
            {
                Router.Send(notifyFixed, targetClient);
                Router.Send(eventEnd, targetClient);
                Router.Send(notifyAborted, targetClient);
                targetClient.Character.ItemManager.TradeEnd();
                targetClient.Character.eventSelectExecCode = 0;
            }

            Router.Send(notifyFixed, client);
            Router.Send(eventEnd, client);
            Router.Send(notifyAborted, client);
            client.Character.ItemManager.TradeEnd();
            client.Character.eventSelectExecCode = 0;
        }
    }
}
