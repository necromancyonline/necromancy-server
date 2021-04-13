using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopSell : ClientHandler
    {
        public SendShopSell(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_shop_sell;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType)packet.data.ReadByte();
            byte bag = packet.data.ReadByte();
            short slot = packet.data.ReadInt16();
            ulong saleGold = packet.data.ReadUInt64(); //irrelevant, check sale price server side
            byte quantity = packet.data.ReadByte();

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.character);
            int error = 0;

            try
            {
                //ulong currentGold = itemService.Sell(location, quantity);
                ulong currentGold = itemService.AddGold(saleGold);
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, currentGold);
                router.Send(recvSelfMoneyNotify);

                ItemInstance itemInstance = itemService.Remove(new ItemLocation(zone, bag, slot), quantity);
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, itemInstance);
                router.Send(recvItemRemove);
            }
            catch (ItemException e)
            {
                error = (int)e.type;
            }

            RecvShopSell recvShopSell = new RecvShopSell(client, error);
            router.Send(recvShopSell);
        }
    }
}
