using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_add_item : ClientHandler
    {
        private ItemInstance _itemInstance;
        public send_trade_add_item(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_add_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.Data.ReadByte();
            byte fromContainer = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            short toSlot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();

            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);
            ItemService itemService = new ItemService(client.Character);
            _itemInstance = itemService.GetIdentifiedItem(fromLoc);//To do; get regular item instead of identified item. Mark item as in trade.
            client.Character.TradeWindowSlot[toSlot] = _itemInstance.InstanceID;

            ItemLocation originalLocation = _itemInstance.Location;
            _itemInstance.Location = new ItemLocation(ItemZoneType.TradeWindow, 0, toSlot); //This is bad. it changes the stored location?

            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client, (ushort) AreaPacketId.recv_trade_add_item_r, res, ServerType.Area);

            if (targetClient != null)
            {
                RecvItemInstance itemInstance = new RecvItemInstance(targetClient, _itemInstance);
                Router.Send(itemInstance);
            }
            _itemInstance.Location = originalLocation;
        }
    }
}
