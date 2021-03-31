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
            ItemLocation itemlocation = client.Character.ItemManager.TradeRemoveItem(fromSlot);

            if (targetClient != null)
            {
                ItemInstance itemInstance = client.Character.ItemManager.GetItem(itemlocation);
                RecvItemRemove itemRemove = new RecvItemRemove(targetClient, itemInstance);
                Router.Send(itemRemove);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            Router.Send(client, (ushort)AreaPacketId.recv_trade_remove_item_r, res, ServerType.Area);
        }
    }
}
