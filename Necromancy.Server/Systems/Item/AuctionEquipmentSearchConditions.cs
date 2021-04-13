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

        public string text { get; set; }
        public byte soulRankMin { get; set; }
        public byte soulRankMax { get; set; }
        public byte forgePriceMin { get; set; }
        public byte forgePriceMax { get; set; }
        public ItemQualities qualities { get; set; }
        public Classes @class { get; set; }
        public Races race { get; set; }
        public ulong goldCost { get; set; }
        public bool isLessThanGoldCost { get; set; }
        public bool hasGemSlot { get; set; }
        public GemType gemSlotType1 { get; set; }
        public GemType gemSlotType2 { get; set; }
        public GemType gemSlotType3 { get; set; }
        public string description { get; set; }

        public bool HasValidText()
        {
            return text.Length <= MAX_TEXT_LENGTH;
        }

        public bool HasValidClass()
        {
            return (@class & Classes.All) == @class;
        }

        public bool HasValidQuality()
        {
            return (qualities & ItemQualities.All) == qualities;
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
