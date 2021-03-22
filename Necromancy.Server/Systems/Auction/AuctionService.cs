using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using System;
using static Necromancy.Server.Systems.Auction.AuctionException;

namespace Necromancy.Server.Systems.Auction
{
    public class AuctionService
    {
        /*
        recv_auction_bid_r = 0x3F38,
        recv_auction_cancel_bid_r = 0xA0FC,
        recv_auction_cancel_exhibit_r = 0xFC28,
        recv_auction_close_r = 0xD1CB,
        recv_auction_exhibit_r = 0xB38D,
        recv_auction_notify_close = 0x7D2B,
        recv_auction_notify_open = 0xBA71,
        recv_auction_re_exhibit_r = 0xA549,
        recv_auction_receive_gold_r = 0x22C,
        recv_auction_receive_item_r = 0xB1CA,
        recv_auction_search_r = 0xF7E7,
        recv_auction_update_bid_item_state = 0x2E17,
        recv_auction_update_bid_num = 0x9BC6,
        recv_auction_update_exhibit_item_state = 0x5307,
        recv_auction_update_highest_bid = 0xBD99 

        send_auction_bid = 0x63BC, 
        send_auction_cancel_bid = 0xBC65,
        send_auction_cancel_exhibit = 0x375B,
        send_auction_close = 0xE732,
        send_auction_exhibit = 0xED52, 
        send_auction_re_exhibit = 0x61FA, 
        send_auction_receive_gold = 0x4657, 
        send_auction_receive_item = 0x7E18, 
        send_auction_search = 0x8865 
        */

        public const int MAX_BIDS = 8;
        public const int MAX_BIDS_NO_DIMENTO = 5;
        public const int MAX_LOTS = 5;
        public const int MAX_LOTS_NO_DIMENTO = 3;
        public const int SECONDS_IN_AN_HOUR = 60 * 60;
        public const int MAX_SEARCH_RESULTS = 100;

        private const int ITEM_NOT_FOUND_ID = -1;

        private const double LISTING_FEE_PERCENT = .05;

        private readonly NecClient _client;
        private readonly IAuctionDao _auctionDao;

        public AuctionService(NecClient nClient, IAuctionDao auctionDao)
        {
            _client = nClient;
            _auctionDao = auctionDao;
        }
        
        public AuctionService(NecClient nClient)
        {
            _client = nClient;
            _auctionDao = new AuctionDao();
        }        

        public void Bid(AuctionLot auctionItem, int bid)
        {
            auctionItem.BidderId = _client.Character.Id;
            auctionItem.CurrentBid = bid;

            AuctionLot currentItem = _auctionDao.SelectItem(auctionItem.Id);

            if (currentItem.Id == ITEM_NOT_FOUND_ID) throw new AuctionException(AuctionExceptionType.Generic);            

            if (auctionItem.CurrentBid < currentItem.CurrentBid) throw new AuctionException(AuctionExceptionType.NewBidLowerThanPrev);
            
            AuctionLot[] bids = _auctionDao.SelectBids(_client.Character);
            if(bids.Length >= MAX_BIDS) throw new AuctionException(AuctionExceptionType.BidSlotsFull);

            //TODO ADD CHECK FOR DIMENTO MEDAL / ROYAL after 5 bids
            if (false) throw new AuctionException(AuctionExceptionType.BidDimentoMedalExpired);

            int currentWealth = _auctionDao.SelectGold(_client.Character);
            if(currentWealth < bid) throw new AuctionException(AuctionExceptionType.Generic);

            _auctionDao.SubtractGold(_client.Character, bid);
            _auctionDao.UpdateBid(auctionItem);
        }

        public void CancelBid(AuctionLot auctionItem)
        {
            AuctionLot currentItem = _auctionDao.SelectItem(auctionItem.Id);
            if (currentItem.SecondsUntilExpiryTime < SECONDS_IN_AN_HOUR) throw new AuctionException(AuctionExceptionType.NoCancelExpiry);
            if (!currentItem.IsCancellable) throw new AuctionException(AuctionExceptionType.AnotherCharacterAlreadyBid);

            _auctionDao.AddGold(_client.Character, currentItem.CurrentBid);

            auctionItem.BidderId = 0;
            auctionItem.CurrentBid = 0;            
            _auctionDao.UpdateBid(auctionItem);            
        }

        public void CancelExhibit(AuctionLot auctionItem)
        {
            throw new NotImplementedException();
        }

        public void Close(AuctionLot auctionItem)
        {
            throw new NotImplementedException();
        }

        public void ReExhibit(AuctionLot auctionItem)
        {
            throw new NotImplementedException();
        }

        public AuctionLot Exhibit(ulong itemInstanceId, byte quantity, int auctionTimeSelector, ulong minBid, ulong buyoutPrice, string comment)
        {            
            int currentNumLots = GetLots().Length;
            if (currentNumLots >= MAX_LOTS)
                throw new AuctionException(AuctionExceptionType.LotSlotsFull);

            //TODO CHECK IF EQUIPPED
            if (false)
                throw new AuctionException(AuctionExceptionType.EquipListing);

            //TODO CHECK IF INVALID ITEM
            if (false)
                throw new AuctionException(AuctionExceptionType.InvalidListing);

            //TODO CHECK DIMETO MEDAL ROYAL ACCOUNT STATUS
            if (false)
                throw new AuctionException(AuctionExceptionType.LotDimentoMedalExpired);

            //TODO CHECK ITEM ALREADY_LISTED items must have a unique serial in item spawn!
            if (false)
                throw new AuctionException(AuctionExceptionType.ItemAlreadyListed);

            //int gold = _auctionDao.SelectGold(_client.Character);

            //InventoryService iManager = new InventoryService(_client); //remove this 
            //iManager.SubtractGold((int) Math.Ceiling(auctionItem.BuyoutPrice * LISTING_FEE_PERCENT)); 

            AuctionLot auctionLot = new AuctionLot();
            auctionLot.ItemInstanceId = itemInstanceId;
            auctionLot.MinimumBid = minBid;
            auctionLot.Quantity = quantity;
            auctionLot.BuyoutPrice = buyoutPrice;
            auctionLot.Comment = comment;

            int auctionTimeInSecondsFromNow = 0;
            const int SECONDS_PER_FOUR_HOURS = 60 * 60 * 4;
            for (int i = 0; i < auctionTimeSelector; i++)
            {
                auctionTimeInSecondsFromNow = (i + 1) * SECONDS_PER_FOUR_HOURS;
            }
            auctionLot.SecondsUntilExpiryTime = auctionTimeInSecondsFromNow;

            _auctionDao.InsertLot(auctionLot);

            return new AuctionLot();
        }

        public AuctionLot[] Search(SearchCriteria searchCritera)
        {
            throw new NotImplementedException();
        }

        public AuctionLot[]  GetBids() {
            return _auctionDao.SelectBids(_client.Character);
        }

        public AuctionLot[] GetLots()
        {
            return _auctionDao.SelectLots(_client.Character);
        }        

    }
}
