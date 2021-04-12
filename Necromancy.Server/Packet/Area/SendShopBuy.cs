using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopBuy : ClientHandler
    {
        public SendShopBuy(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_shop_buy;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte index = packet.data.ReadByte();
            ulong price = packet.data.ReadUInt64();
            byte count = packet.data.ReadByte();

            client.character.adventureBagGold -= count * price;

            RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, client.character.adventureBagGold);
            router.Send(recvSelfMoneyNotify, client);

            if (client.character.shopItemIndex != null)
            {
                int itemId = client.character.shopItemIndex[index];
                ItemSpawnParams spawmParam = new ItemSpawnParams();
                spawmParam.itemStatuses = ItemStatuses.Identified;
                spawmParam.quantity = count;
                ItemService itemService = new ItemService(client.character);
                ItemInstance itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, itemId, spawmParam);

                RecvSituationStart recvSituationStart = new RecvSituationStart(2);
                router.Send(client, recvSituationStart.ToPacket());

                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                router.Send(client, recvItemInstance.ToPacket());

                RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
                router.Send(client, recvSituationEnd.ToPacket());
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_shop_buy_r, res, ServerType.Area);
        }
    }
}
