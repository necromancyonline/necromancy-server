using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using System;
using System.Text;

namespace Necromancy.Server.Systems.Auction
{
    public class send_auction_search : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_auction_search));

        public send_auction_search(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_search;

        public override void Handle(NecClient client, NecPacket packet)
        {
            SearchCriteria searchCriteria = new SearchCriteria();
            searchCriteria.SoulRankMin = packet.Data.ReadByte();
            searchCriteria.SoulRankMax = packet.Data.ReadByte();
            searchCriteria.ForgePriceMin = packet.Data.ReadByte();
            searchCriteria.ForgePriceMax = packet.Data.ReadByte();
            searchCriteria.Quality = (SearchCriteria.Qualities) packet.Data.ReadInt16();
            searchCriteria.Class = (SearchCriteria.Classes) packet.Data.ReadInt16();

            Logger.Info("YEFAS2F");
            int NUMBER_OF_ITEMS_DEBUG = 20;

            
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // error

            res.WriteInt32(NUMBER_OF_ITEMS_DEBUG); // number of loops
            
            for (int i = 0; i < NUMBER_OF_ITEMS_DEBUG; i++)
            {
                string hellothere = i.ToString() + " " + Convert.ToString(i, 2).PadLeft(8, '0'); ;
                res.WriteInt32(i); //row identifier 
                res.WriteInt64(i + 300); //spawned item id
                res.WriteInt32(17); // Lowest
                res.WriteInt32(500); // Buy Now
                res.WriteFixedString(hellothere, 49); // Soul Name Of Sellers
                res.WriteByte(8); // 0 = nothing.    Other = Logo appear. maybe it's effect or rank, or somethiung else ?
                res.WriteFixedString("", 385); // Item Comment
                res.WriteInt16(0); // Bid
                res.WriteInt32(1000); // Item remaining time
            }

            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_search_r, res, ServerType.Area);


        }

        
             
    }
}
