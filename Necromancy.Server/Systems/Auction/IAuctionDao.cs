using Necromancy.Server.Model;
using Necromancy.Server.Systems.Auction_House.Logic;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Auction_House.Data_Access
{
    public interface IAuctionDao
    {
        public bool InsertItem(AuctionLot insertItem);

        public bool UpdateBid(AuctionLot updateBidItem);

        public AuctionLot SelectItem(int itemId);

        public AuctionLot[] SelectBids(Character character);

        public AuctionLot[] SelectLots(Character character);

        public int SelectGold(Character character);

        public void AddGold(Character character, int amount);

        public void SubtractGold(Character character, int amount);

        public AuctionLot[] SelectItemsByCriteria(SearchCriteria searchCriteria);
        
    }
}
