namespace Necromancy.Server.Systems.Item
{
    public class AuctionEquipmentSearchConditions
    {
        public const int MaxTextLength = 73;
        public const int MaxDescriptionLength = 193;
        private const int _MinSoulRank = 0;
        private const int _MaxSoulRank = 99;
        private const int _MinForgePrice = 0;
        private const int _MaxForgePrice = 99;

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
            return text.Length <= MaxTextLength;
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
            return soulRankMin >= _MinSoulRank && soulRankMin <= _MaxSoulRank;
        }

        public bool HasValidSoulRankMax()
        {
            return soulRankMax >= _MinSoulRank && soulRankMax <= _MaxSoulRank;
        }

        public bool HasValidForgePriceMin()
        {
            return forgePriceMin >= _MinForgePrice && forgePriceMin <= _MinForgePrice;
        }

        public bool HasValidForgePriceMax()
        {
            return forgePriceMax >= _MaxForgePrice && forgePriceMax <= _MaxForgePrice;
        }
    }
}
