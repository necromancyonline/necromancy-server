using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_exhibit : ClientHandler
    {
        public send_auction_exhibit(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_auction_exhibit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte exhibitSlot = packet.Data.ReadByte();
            ItemZoneType zone = (ItemZoneType)packet.Data.ReadByte();
            byte bag = packet.Data.ReadByte();
            short slot = packet.Data.ReadInt16();
            byte quantity = packet.Data.ReadByte();
            int time = packet.Data.ReadInt32(); //0:4hours 1:8 hours 2:16 hours 3:24 hours
            ulong minBid = packet.Data.ReadUInt64();
            ulong buyoutPrice = packet.Data.ReadUInt64();
            string comment = packet.Data.ReadCString();

            ItemLocation auctionLoc = new ItemLocation(ItemZoneType.TempAuctionZone, 0, exhibitSlot);
            ItemLocation fromLoc = new ItemLocation(zone, bag, slot);
            ItemService itemService = new ItemService(client.Character);

            AuctionService auctionService = new AuctionService(client);
            ItemInstance auctionItemInstance = itemService.GetItem(fromLoc);
            int auctionError = 0;
            try
            {
                auctionService.Exhibit(auctionItemInstance.InstanceID, quantity, time, minBid, buyoutPrice, comment);
            }
            catch (AuctionException e) { auctionError = (int)e.Type; }

            if (auctionError == 0)
            {
                int moveError = 0;
                try
                {
                    MoveResult moveResult = itemService.Move(fromLoc, auctionLoc, quantity);
                    List<PacketResponse> responses = itemService.GetMoveResponses(client, moveResult);
                    Router.Send(client, responses);
                }
                catch (ItemException e) { moveError = (int)e.Type; }
                if (moveError != 0) auctionError = (int)AuctionExceptionType.Generic;
            }
            

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError); //error check.
            res.WriteUInt64(buyoutPrice);
            res.WriteUInt64(auctionItemInstance.InstanceID);
            Router.Send(client.Map, (ushort)AreaPacketId.recv_auction_exhibit_r, res, ServerType.Area);
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
