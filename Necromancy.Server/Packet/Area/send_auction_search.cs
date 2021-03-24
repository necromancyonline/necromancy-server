using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_search : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_auction_search));

        public send_auction_search(NecServer server) : base(server)        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {

            AuctionSearchCriteria searchCriteria = new AuctionSearchCriteria();
            searchCriteria.SoulRankMin = packet.Data.ReadByte();
            searchCriteria.SoulRankMax = packet.Data.ReadByte();
            searchCriteria.ForgePriceMin = packet.Data.ReadByte();
            searchCriteria.ForgePriceMax = packet.Data.ReadByte();
            searchCriteria.Quality = (ItemQualities)packet.Data.ReadInt16();
            searchCriteria.Class = (Classes)packet.Data.ReadInt16();

            Logger.Info("YEFAS2F");
            int NUMBER_OF_ITEMS_DEBUG = 20;

            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> auctionList = itemService.SearchAuction(searchCriteria);

            //IBuffer res = BufferProvider.Provide();
            //res.WriteInt32(0);
            //res.WriteInt32(auctionList.Count); // cmp to 0x64 = 100
            //int i = 0;
            //foreach(AuctionLot auctionLot in auctionList)
            //{
            //    res.WriteInt32(i); // 0 = bid, 1 = re-bid 
            //    res.WriteUInt64(auctionLot.ItemInstance.InstanceID); // 1 = add, 2 blue icons, what is this ?
            //    res.WriteUInt64(auctionLot.MinimumBid); // Lowest
            //    res.WriteUInt64(auctionLot.BuyoutPrice); // Buy Now
            //    res.WriteFixedString(auctionLot.ConsignerName, 49); // Soul Name Of Sellers
            //    res.WriteByte(
            //        0); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
            //    res.WriteFixedString(auctionLot.Comment, 385); // Item Comment
            //    res.WriteInt16(8); // Bid?
            //    res.WriteInt32(auctionLot.SecondsUntilExpiryTime); // Item remaining time
            //}

            //Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);
        }
    }
}
