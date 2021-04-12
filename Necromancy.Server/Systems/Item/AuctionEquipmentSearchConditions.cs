using System;

namespace Necromancy.Server.Systems.Item
{
    public class AuctionEquipmentSearchConditions
    {
        public const int MAX_TEXT_LENGTH = 73;
        public const int MAX_DESCRIPTION_LENGTH = 193;
        private const int MIN_SOUL_RANK = 0;
        private const int MAX_SOUL_RANK = 99;
        private const int MIN_FORGE_PRICE = 0;
        private const int MAX_FORGE_PRICE = 99;

        public string Text { get; set; }
        public byte SoulRankMin { get; set; }
        public byte SoulRankMax { get; set; }
        public byte ForgePriceMin { get; set; }
        public byte ForgePriceMax { get; set; }
        public ItemQualities Qualities { get; set; }
        public Classes Class { get; set; }
        public Races Race { get; set; }
        public ulong GoldCost { get; set; }
        public bool IsLessThanGoldCost { get; set; }
        public bool HasGemSlot { get; set; }
        public GemType GemSlotType1 { get; set; }
        public GemType GemSlotType2 { get; set; }
        public GemType GemSlotType3 { get; set; }
        public string Description { get; set; }

        public bool HasValidText()
        {
            return Text.Length <= MAX_TEXT_LENGTH;
        }

        public bool HasValidClass()
        {
            return (Class & Classes.All) == Class;
        }

        public bool HasValidQuality()
        {
            return (Qualities & ItemQualities.All) == Qualities;
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
