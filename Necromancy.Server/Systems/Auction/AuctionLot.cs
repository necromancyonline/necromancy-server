

namespace Necromancy.Server.Systems.Auction
{
    public class AuctionLot
    {
        public int Id { get; set; }
        public int ConsignerId { get; set; }
        public string ConsignerName { get; set; }
        public int ItemID { get; set; }
        public long ItemInstanceId { get; set; }
        public int Quantity { get; set; }
        public int SecondsUntilExpiryTime { get; set; }
        public int MinimumBid { get; set; }
        public int BuyoutPrice { get; set; }
        public int BidderId { get; set; }          
        public string BidderName { get; set; }
        public int CurrentBid { get; set; }
        public string Comment { get; set; }
        public bool IsCancellable { get; set; }
    }
}
