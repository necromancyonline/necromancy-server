using System;

namespace Necromancy.Server.Systems.Item
{
    public class AuctionItemSearchConditions
    {

        private const int _MinSoulRank = 0;
        private const int _MaxSoulRank = 99;
        private const int _MinForgePrice = 0;
        private const int _MaxForgePrice = 99;

        public int soulRankMin {get; set; }
        public int soulRankMax { get; set; }
        public int forgePriceMin { get; set; }
        public int forgePriceMax { get; set; }
        public ItemQualities quality { get; set; }
        public Classes @class { get;set; }

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
            return (soulRankMin >= _MinSoulRank) && (soulRankMin <= _MaxSoulRank);
        }

        public bool HasValidSoulRankMax()
        {
            return (soulRankMax >= _MinSoulRank) && (soulRankMax <= _MaxSoulRank);
        }

        public bool HasValidForgePriceMin()
        {
            return (forgePriceMin >= _MinForgePrice) && (forgePriceMin <= _MinForgePrice);
        }

        public bool HasValidForgePriceMax()
        {
            return (forgePriceMax >= _MaxForgePrice) && (forgePriceMax <= _MaxForgePrice);
        }
    }
}
