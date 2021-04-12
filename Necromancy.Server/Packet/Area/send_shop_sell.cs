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
            ulong saleGold = packet.Data.ReadUInt64(); //irrelevant, check sale price server side
            byte quantity = packet.Data.ReadByte(); 

            ItemLocation location = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);            
            int error = 0;

            try
            {
                //ulong currentGold = itemService.Sell(location, quantity);
                ulong currentGold = itemService.AddGold(saleGold);
                RecvSelfMoneyNotify recvSelfMoneyNotify = new RecvSelfMoneyNotify(client, currentGold);
                Router.Send(recvSelfMoneyNotify);

                ItemInstance itemInstance = itemService.Remove(new ItemLocation(zone, bag, slot), quantity);
                RecvItemRemove recvItemRemove = new RecvItemRemove(client, itemInstance);
                Router.Send(recvItemRemove);
            }
            catch (ItemException e) { error = (int) e.Type; }

            RecvShopSell recvShopSell = new RecvShopSell(client, error);
            Router.Send(recvShopSell);
        }
    }
}
