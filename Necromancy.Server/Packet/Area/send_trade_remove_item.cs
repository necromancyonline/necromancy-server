using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_trade_remove_item : ClientHandler
    {
        public send_trade_remove_item(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_trade_remove_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            short fromSlot = packet.Data.ReadInt16();

            //The item zone type fails here.
            //The given key 'TradeWindow' was not present in the dictionary.
            ItemLocation fromLoc = new ItemLocation(ItemZoneType.TradeWindow, 0, fromSlot);
            ItemService itemService = new ItemService(client.Character);
            ItemInstance itemInstance = itemService.GetIdentifiedItem(fromLoc);
            

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client, (ushort) AreaPacketId.recv_trade_remove_item_r, res, ServerType.Area);

            RecvItemRemove itemRemove = new RecvItemRemove(targetClient, itemInstance);
            Router.Send(itemRemove, targetClient);
        }

    }
}
