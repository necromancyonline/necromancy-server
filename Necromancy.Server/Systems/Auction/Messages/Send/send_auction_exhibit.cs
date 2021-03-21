using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction.Logic;

namespace Necromancy.Server.Systems.Auction
{
    public class send_auction_exhibit : ClientHandler
    {
        public send_auction_exhibit(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_exhibit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte auctionSlot = packet.Data.ReadByte(); 
            short bagNumber = packet.Data.ReadInt16(); 
            short bagSlot = packet.Data.ReadInt16(); 

            byte quantity = packet.Data.ReadByte(); 
            int auctionTimeSelector = packet.Data.ReadInt32(); //0:4hours 1:8 hours 2:16 hours 3:24 hours
            int minBid = packet.Data.ReadInt32(); 
            int buyoutPrice = packet.Data.ReadInt32(); 
            string comment = packet.Data.ReadCString();
            
            AuctionLot auctionItem = new AuctionLot();

            auctionItem.Quantity = quantity;
            auctionItem.MinimumBid = minBid;
            auctionItem.BuyoutPrice = buyoutPrice;
            auctionItem.Comment = comment;
            
            int auctionTimeInSecondsFromNow = 0;
            const int SECONDS_PER_FOUR_HOURS = 60 * 60 * 4;
            for(int i = 0; i < auctionTimeSelector; i++)
            {
                auctionTimeInSecondsFromNow = (i + 1) * SECONDS_PER_FOUR_HOURS;
            }
            auctionItem.SecondsUntilExpiryTime = auctionTimeInSecondsFromNow;

            AuctionService auctionService = new AuctionService(client);
            int auctionException = 0;
            try { 
            auctionItem = auctionService.Exhibit(auctionItem, bagNumber, bagSlot);
            } catch(AuctionException e)
            {
                auctionException = (int) e.Type;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionException); //error check.
            res.WriteInt32(auctionItem.BuyoutPrice);
            res.WriteInt64(auctionItem.ItemID); //spawn id
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_exhibit_r, res, ServerType.Area);
        }

        /*
        AUCTION	1	This item may not be listed
        AUCTION	2	You may not list the equipped items
        AUCTION	3	No space available in item list
        AUCTION	4	The minimum price is lower than the Buy Now price
        AUCTION	5	Please place a bid higher than the one you've already placed
        AUCTION	6	No space available in Bidding Item List
        AUCTION	7	Unable to change bid value as Dimento Medal has expired
        AUCTION	8	Unable to list item as Dimento Medal has expired
        AUCTION	9	Illegal search query.\nPlease set a minimum and maximum amount.
        AUCTION	-3700	Item has already been listed
        AUCTION	-3701	Illegal status
        AUCTION	-3702	You have already bid on this item
        AUCTION	-3703	You are unable to cancel as there is less than one hour remaining
        AUCTION	-3704	You are unable to cancel because another character has already bid
        AUCTION	-3705	Listing time over
        AUCTION	-3706	Listing cancelled
        AUCTION -3707	Bidding has already completed
        AUCTION -203	Slot unavailable
        AUCTION -204	Illegal item amount
        AUCTION	-212	This item may not be listed
        AUCTION GENERIC Auction error
        */
    }
}
