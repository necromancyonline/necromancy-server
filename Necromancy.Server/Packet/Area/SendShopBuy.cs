using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_buy : ClientHandler
    {
        public send_shop_buy(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_shop_buy;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte index = packet.Data.ReadByte();
            ulong price = packet.Data.ReadUInt64();
            byte count = packet.Data.ReadByte();

            client.Character.AdventureBagGold -= (ulong)(count * price);

            RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, client.Character.AdventureBagGold);
            Router.Send(recvSelfMoneyNotify, client);

            if (client.Character.shopItemIndex != null)
            {
                int itemId = client.Character.shopItemIndex[index];
                ItemSpawnParams spawmParam = new ItemSpawnParams();
                spawmParam.ItemStatuses = ItemStatuses.Identified;
                spawmParam.Quantity = count;
                ItemService itemService = new ItemService(client.Character);
                ItemInstance itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, itemId, spawmParam);

                RecvSituationStart recvSituationStart = new RecvSituationStart(2);
                Router.Send(client, recvSituationStart.ToPacket());

                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                Router.Send(client, recvItemInstance.ToPacket());

                RecvSituationEnd recvSituationEnd = new RecvSituationEnd();
                Router.Send(client, recvSituationEnd.ToPacket());
            }    

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_shop_buy_r, res, ServerType.Area);
        }
    }
}
