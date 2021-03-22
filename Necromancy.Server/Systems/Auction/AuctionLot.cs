

namespace Necromancy.Server.Systems.Auction
{
    public class AuctionLot
    {
        public int Id { get; set; }
        public int ConsignerId { get; set; }
        public string ConsignerName { get; set; }
        public byte Slot { get; internal set; }
        public int ItemID { get; set; }
        public ulong ItemInstanceId { get; set; }
        public int Quantity { get; set; }
        public int SecondsUntilExpiryTime { get; set; }
        public ulong MinimumBid { get; set; }
        public ulong BuyoutPrice { get; set; }
        public int BidderId { get; set; }          
        public string BidderName { get; set; }
        public int CurrentBid { get; set; }
        public string Comment { get; set; }
        public bool IsCancellable { get; set; }
    }
}
