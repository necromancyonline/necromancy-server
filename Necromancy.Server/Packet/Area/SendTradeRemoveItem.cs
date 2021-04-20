using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendTradeRemoveItem : ClientHandler
    {
        public SendTradeRemoveItem(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_trade_remove_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient targetClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            short fromSlot = packet.data.ReadInt16();
            ulong itemId = client.character.tradeWindowSlot[fromSlot];
            ItemInstance itemInstance = client.character.itemLocationVerifier.GetItemByInstanceId(itemId);

            if (targetClient != null)
            {
                RecvItemRemove itemRemove = new RecvItemRemove(targetClient, itemInstance);
                router.Send(itemRemove);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error check?
            router.Send(client, (ushort)AreaPacketId.recv_trade_remove_item_r, res, ServerType.Area);
        }
    }
}
