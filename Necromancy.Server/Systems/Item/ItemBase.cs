using Necromancy.Server.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public class ItemBase
    {        
        public int BaseID { get; set; }
        public ItemType Type { get; set; }
        public ItemQualities Quality { get; set; }
        public byte MaxStackSize { get; set; } = 1;
        public ItemEquipSlots EquipAllowedSlots { get; set; }    
        public Races RequiredRaces { get; set; }
        public Classes RequiredClasses { get; set; }
        public Alignments RequiredAlignments { get; set; }

        public short RequiredStrength { get; set; }
        public short RequiredVitality { get; set; }
        public short RequiredDexterity { get; set; }
        public short RequiredAgility { get; set; }
        public short RequiredIntelligence { get; set; }
        public short RequiredPiety { get; set; }
        public short RequiredLuck { get; set; }

        public byte RequiredSoulRank { get; set; }
        public byte RequiredLevel { get; set; }

        public byte PhysicalSlash {get; set;}
        public byte PhysicalStrike { get; set; }
        public byte PhysicalPierce { get; set; }

        public byte PhysicalDefenseFire { get; set; }
        public byte PhysicalDefenseWater { get; set; }
        public byte PhysicalDefenseWind { get; set; }
        public byte PhysicalDefenseEarth { get; set; }
        public byte PhysicalDefenseLight { get; set; }
        public byte PhysicalDefenseDark { get; set; }

        public byte MagicalAttackFire { get; set; }
        public byte MagicalAttackWater { get; set; }
        public byte MagicalAttackWind { get; set; }
        public byte MagicalAttackEarth { get; set; }
        public byte MagicalAttackLight { get; set; }
        public byte MagicalAttackDark { get; set; }

        public byte Hp { get; set; }
        public byte Mp { get; set; }
        public byte Str { get; set; }
        public byte Vit { get; set; }
        public byte Dex { get; set; }
        public byte Agi { get; set; }
        public byte Int { get; set; }
        public byte Pie { get; set; }
        public byte Luk { get; set; }

        public byte ResistPoison { get; set; }
        public byte ResistParalyze { get; set; }
        public byte ResistPetrified { get; set; }
        public byte ResistFaint { get; set; }
        public byte ResistBlind { get; set; }
        public byte ResistSleep { get; set; }
        public byte ResistSilence { get; set; }
        public byte ResistCharm { get; set; }
        public byte ResistConfusion { get; set; }
        public byte ResistFear { get; set; }

        public ItemStatusEffect StatusMalus {get; set; }
        public int StatusMalusPercent { get; set; }

        public string ObjectType = "NONE"; //TODO
        public string EquipSlot2 = "WhoKnows"; //TODO
        //public string IconType = "eh"; //TODO
        public byte BagSize { get; set; }
        public bool IsUseableInTown { get; set; }
        public bool IsStorable { get; set; }
        public bool IsDiscardable { get; set; }
        public bool IsSellable { get; set; }
        public bool IsTradeable { get; set; }
        public bool IsTradableAfterUse { get; set; }
        public bool IsStealable { get; set; }
        public bool IsGoldBorder { get; set; }
        public string Lore = "";
        public int IconId { get; set; }

    }
}
