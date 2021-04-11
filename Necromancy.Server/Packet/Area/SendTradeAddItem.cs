using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeAddItem : ClientHandler
    {
        private ItemInstance _itemInstance;
        public SendTradeAddItem(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_trade_add_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.data.ReadByte();
            byte fromContainer = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            short toSlot = packet.data.ReadInt16();
            byte quantity = packet.data.ReadByte();

            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);
            ItemService itemService = new ItemService(client.character);
            _itemInstance = itemService.GetIdentifiedItem(fromLoc);//To do; get regular item instead of identified item. Mark item as in trade.
            client.character.tradeWindowSlot[toSlot] = _itemInstance.instanceId;

            ItemLocation originalLocation = _itemInstance.location;
            _itemInstance.location = new ItemLocation(ItemZoneType.TradeWindow, 0, toSlot); //This is bad. it changes the stored location?

            NecClient targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            router.Send(client, (ushort) AreaPacketId.recv_trade_add_item_r, res, ServerType.Area);

            if (targetClient != null)
            {
                RecvItemInstance itemInstance = new RecvItemInstance(targetClient, _itemInstance);
                router.Send(itemInstance);
            }
            _itemInstance.location = originalLocation;
        }
    }
}
