using Necromancy.Server.Model;

namespace Necromancy.Server.Systems.Auction
{
    public interface IAuctionDao
    {
        public bool InsertLot(AuctionLot insertItem);

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
