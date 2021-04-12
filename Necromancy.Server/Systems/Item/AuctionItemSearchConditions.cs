namespace Necromancy.Server.Systems.Item
{
    public class AuctionItemSearchConditions
    {
        private const int MIN_SOUL_RANK = 0;
        private const int MAX_SOUL_RANK = 99;
        private const int MIN_FORGE_PRICE = 0;
        private const int MAX_FORGE_PRICE = 99;

        public int soulRankMin { get; set; }
        public int soulRankMax { get; set; }
        public int forgePriceMin { get; set; }
        public int forgePriceMax { get; set; }
        public ItemQualities quality { get; set; }
        public Classes @class { get; set; }

        public bool HasValidClass()
        {
            return (@class & Classes.All) == @class;
        }

        public bool HasValidQuality()
        {
            return (quality & ItemQualities.All) == quality;
        }

        public bool HasValidSoulRankMin()
        {
            return soulRankMin >= MIN_SOUL_RANK && soulRankMin <= MAX_SOUL_RANK;
        }

        public bool HasValidSoulRankMax()
        {
            return soulRankMax >= MIN_SOUL_RANK && soulRankMax <= MAX_SOUL_RANK;
        }

        public bool HasValidForgePriceMin()
        {
            return forgePriceMin >= MIN_FORGE_PRICE && forgePriceMin <= MIN_FORGE_PRICE;
        }

        public bool HasValidForgePriceMax()
        {
            return forgePriceMax >= MAX_FORGE_PRICE && forgePriceMax <= MAX_FORGE_PRICE;
        }
    }
}
