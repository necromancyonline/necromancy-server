using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_sell : ClientHandler
    {
        public send_shop_sell(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_shop_sell;
        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType zone = (ItemZoneType) packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            long saleGold = packet.Data.ReadInt64(); //irrelevant, check sale price server side
            byte quantity = packet.Data.ReadByte(); //TODO maybe quantity

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);            
            int error = 0;

            try
            {
                long currentGold = itemService.Sell(location, quantity);
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, currentGold);
                Router.Send(recvSelfMoneyNotify);
            }
            catch (ItemException e) { error = (int) e.ExceptionType; }

            RecvShopSell recvShopSell = new RecvShopSell(client, error);
            Router.Send(recvShopSell);
        }
    }
}
