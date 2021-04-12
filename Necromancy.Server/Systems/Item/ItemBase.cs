namespace Necromancy.Server.Systems.Item
{
    public class ItemBase
    {
        public string EquipSlot2 = "WhoKnows"; //TODO
        public string Lore = "";

        public string ObjectType = "NONE"; //TODO
        public int baseId { get; set; }
        public ItemType type { get; set; }
        public ItemQualities quality { get; set; }
        public byte maxStackSize { get; set; } = 1;
        public ItemEquipSlots equipAllowedSlots { get; set; }
        public Races requiredRaces { get; set; }
        public Classes requiredClasses { get; set; }
        public Alignments requiredAlignments { get; set; }

        public short requiredStrength { get; set; }
        public short requiredVitality { get; set; }
        public short requiredDexterity { get; set; }
        public short requiredAgility { get; set; }
        public short requiredIntelligence { get; set; }
        public short requiredPiety { get; set; }
        public short requiredLuck { get; set; }

        public byte requiredSoulRank { get; set; }
        public byte requiredLevel { get; set; }

        public byte physicalSlash { get; set; }
        public byte physicalStrike { get; set; }
        public byte physicalPierce { get; set; }

        public byte physicalDefenseFire { get; set; }
        public byte physicalDefenseWater { get; set; }
        public byte physicalDefenseWind { get; set; }
        public byte physicalDefenseEarth { get; set; }
        public byte physicalDefenseLight { get; set; }
        public byte physicalDefenseDark { get; set; }

        public byte magicalAttackFire { get; set; }
        public byte magicalAttackWater { get; set; }
        public byte magicalAttackWind { get; set; }
        public byte magicalAttackEarth { get; set; }
        public byte magicalAttackLight { get; set; }
        public byte magicalAttackDark { get; set; }

        public byte hp { get; set; }
        public byte mp { get; set; }
        public byte str { get; set; }
        public byte vit { get; set; }
        public byte dex { get; set; }
        public byte agi { get; set; }
        public byte @int { get; set; }
        public byte pie { get; set; }
        public byte luk { get; set; }

        public byte resistPoison { get; set; }
        public byte resistParalyze { get; set; }
        public byte resistPetrified { get; set; }
        public byte resistFaint { get; set; }
        public byte resistBlind { get; set; }
        public byte resistSleep { get; set; }
        public byte resistSilence { get; set; }
        public byte resistCharm { get; set; }
        public byte resistConfusion { get; set; }
        public byte resistFear { get; set; }

        public ItemStatusEffect statusMalus { get; set; }

        public int statusMalusPercent { get; set; }

        //public string IconType = "eh"; //TODO
        public byte bagSize { get; set; }
        public bool isUseableInTown { get; set; }
        public bool isStorable { get; set; }
        public bool isDiscardable { get; set; }
        public bool isSellable { get; set; }
        public bool isTradeable { get; set; }
        public bool isTradableAfterUse { get; set; }
        public bool isStealable { get; set; }
        public bool isGoldBorder { get; set; }
        public int iconId { get; set; }
    }
}
