using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
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

            //AuctionItemSearchConditions searchCriteria = new AuctionItemSearchConditions();
            //searchCriteria.SoulRankMin = packet.Data.ReadByte();
            //searchCriteria.SoulRankMax = packet.Data.ReadByte();
            //searchCriteria.ForgePriceMin = packet.Data.ReadByte();
            //searchCriteria.ForgePriceMax = packet.Data.ReadByte();
            //searchCriteria.Quality = (ItemQualities)packet.Data.ReadInt16();
            //searchCriteria.Class = (Classes)packet.Data.ReadInt16();            

            //ItemService itemService = new ItemService(client.Character);
            //List<ItemInstance> auctionList = itemService.GetItemsUpForAuction();

            //foreach(ItemInstance auctionItem in auctionList)
            //{
            //    RecvItemInstance recvItemInstance = new RecvItemInstance(client, auctionItem);
            //    Router.Send(recvItemInstance);
            //}

            //IBuffer res = BufferProvider.Provide();
            //res.WriteInt32(0);
            //res.WriteInt32(auctionList.Count); // cmp to 0x64 = 100
            //int i = 0;
            //foreach(ItemInstance auctionItem in auctionList)
            //{
            //    res.WriteInt32(i); //row identifier 
            //    res.WriteUInt64(auctionItem.InstanceID);
            //    res.WriteUInt64(auctionItem.MinimumBid); 
            //    res.WriteUInt64(auctionItem.BuyoutPrice); 
            //    res.WriteFixedString(auctionItem.ConsignerSoulName, 49); 
            //    res.WriteByte(0); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
            //    res.WriteFixedString(auctionItem.Comment, 385); 
            //    res.WriteInt32(auctionItem.CurrentBid); 
            //    res.WriteInt32(auctionItem.SecondsUntilExpiryTime); 
            //}

            //Router.Send(client.Map, (ushort)AreaPacketId.recv_auction_search_r, res, ServerType.Area);
        }
    }
}
