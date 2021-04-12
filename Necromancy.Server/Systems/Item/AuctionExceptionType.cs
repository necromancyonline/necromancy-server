namespace Necromancy.Server.Systems.Item
{
    public enum AuctionExceptionType
    {
        /// <summary>
        ///     This item may not be listed.
        /// </summary>
        InvalidListing = 1,

        /// <summary>
        ///     You may not list the equipped items.
        /// </summary>
        EquipListing = 2,

        /// <summary>
        ///     No space available in item list.
        /// </summary>
        LotSlotsFull = 3,

        /// <summary>
        ///     The minimum price is lower than the Buy Now price.
        /// </summary>
        MinPriceLowerThanBuyout = 4,

        /// <summary>
        ///     Please place a bid higher than the one you've already placed.
        /// </summary>
        NewBidLowerThanPrev = 5,

        /// <summary>
        ///     No space available in Bidding Item List.
        /// </summary>
        BidSlotsFull = 6,

        /// <summary>
        ///     Unable to change bid value as Dimento Medal has expired.
        /// </summary>
        BidDimentoMedalExpired = 7,

        /// <summary>
        ///     Unable to list item as Dimento Medal has expired.
        /// </summary>
        LotDimentoMedalExpired = 8,

        /// <summary>
        ///     Illegal search query.\nPlease set a minimum and maximum amount.
        /// </summary>
        IllegalSearchQuery = 9,

        //NEGATIVE EXCEPTIONS SHOULD NEVER HAPPEN BECAUSE CLIENT PREVENTS, STILL CHECK ERRORS

        /// <summary>
        ///     Item has already been listed.
        /// </summary>
        ItemAlreadyListed = -3700,

        /// <summary>
        ///     Illegal Status.
        /// </summary>
        IllegalStatus = -3701,

        /// <summary>
        ///     You have already bid on this item.
        /// </summary>
        AlreadyBid = -3702,

        /// <summary>
        ///     You are unable to cancel as there is less than one hour remaining.
        /// </summary>
        NoCancelExpiry = -3703,

        /// <summary>
        ///     You are unable to cancel because another character has already bid.
        /// </summary>
        AnotherCharacterAlreadyBid = -3704,

        /// <summary>
        ///     Listing time over.
        /// </summary>
        ListingTimeOvver = -3705,

        /// <summary>
        ///     Listing cancelled.
        /// </summary>
        ListingCancelled = -3706,

        /// <summary>
        ///     Bidding has already completed.
        /// </summary>
        BiddingCompleted = -3707,

        /// <summary>
        ///     Slot unavailable.
        /// </summary>
        SlotUnavailable = -203,

        /// <summary>
        ///     Illegal item amount
        /// </summary>
        IllegalItemAmount = -204,

        /// <summary>
        ///     Auction error.
        /// </summary>
        Generic = 9999
    }
}
