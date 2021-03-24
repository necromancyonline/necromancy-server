using System;

namespace Necromancy.Server.Systems.Item
{
    public class AuctionSearchCriteria
    {

        private const int MIN_SOUL_RANK = 0;
        private const int MAX_SOUL_RANK = 99;
        private const int MIN_FORGE_PRICE = 0;
        private const int MAX_FORGE_PRICE = 99;

        public int SoulRankMin {get; set; }
        public int SoulRankMax { get; set; }
        public int ForgePriceMin { get; set; }
        public int ForgePriceMax { get; set; }
        public ItemQualities Quality { get; set; }
        public Classes Class { get;set; }

        public bool HasdValidClass()
        {
            return (Class & Classes.All) == Class;
        }

        public bool HasValidQuality()
        {
            return (Quality & ItemQualities.All) == Quality;
        }

        public bool HasValidSoulRankMin()
        {
            return (SoulRankMin >= MIN_SOUL_RANK) && (SoulRankMin <= MAX_SOUL_RANK);
        }

        public bool HasValidSoulRankMax()
        {
            return (SoulRankMax >= MIN_SOUL_RANK) && (SoulRankMax <= MAX_SOUL_RANK);
        }

        public bool HasValidForgePriceMin()
        {
            return (ForgePriceMin >= MIN_FORGE_PRICE) && (ForgePriceMin <= MIN_FORGE_PRICE);
        }

        public bool HasValidForgePriceMax()
        {
            return (ForgePriceMax >= MAX_FORGE_PRICE) && (ForgePriceMax <= MAX_FORGE_PRICE);
        }
    }
}
