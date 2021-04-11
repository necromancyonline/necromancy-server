namespace Necromancy.Server.Data.Setting
{
    /// <summary>
    /// Additional Item information, that was not provided or could not be extracted from the client.
    /// </summary>
    public class ItemLibrarySetting : ISettingRepositoryItem
    {
        public int id { get; set; }
        public string name { get; set; }
        //Equipment Slot Settings
        public string itemType { get; set; }
        public string equipmentType { get; set; }

        //Core Attributes
        public string rarity { get; set; }
        public int physicalAttack { get; set; }
        public int magicalAttack { get; set; }
        public int rangeDistance { get; set; }
        public int specialPerformance { get; set; }
        public int durability { get; set; }
        public int hardness { get; set; }
        public float weight { get; set; }

        //Attack type
        public int slash { get; set; }
        public int strike { get; set; }
        public int pierce { get; set; }

        //UnKnown
        public int bonusTolerance { get; set; }

        //Equip Restrictions
        public int fig { get; set; }
        public int thi { get; set; }
        public int mag { get; set; }
        public int pri { get; set; }
        public int sam { get; set; }
        public int nin { get; set; }
        public int bis { get; set; }
        public int lor { get; set; }
        public int clo { get; set; }
        public int alc { get; set; }

        //Bitmask of occupation restrictions
        public int occupation { get; set; }

        //Bonus Stat gains
        public int hp { get; set; }
        public int mp { get; set; }
        public int str { get; set; }
        public int vit { get; set; }
        public int dex { get; set; }
        public int agi { get; set; }
        public int @int { get; set; }
        public int pie { get; set; }
        public int luk { get; set; }

        //Bonus Skills on Attack
        public int poison { get; set; }
        public int paralysis { get; set; }
        public int stone { get; set; }
        public int faint { get; set; }
        public int blind { get; set; }
        public int sleep { get; set; }
        public int charm { get; set; }
        public int confusion { get; set; }
        public int fear { get; set; }

        //Bonus Elemental Defence
        public int fireDef { get; set; }
        public int waterDef { get; set; }
        public int windDef { get; set; }
        public int earthDef { get; set; }
        public int lightDef { get; set; }
        public int darkDef { get; set; }

        //Bonus Elemental Attack
        public int fireAtk { get; set; }
        public int waterAtk { get; set; }
        public int windAtk { get; set; }
        public int earthAtk { get; set; }
        public int lightAtk { get; set; }
        public int darkAtk { get; set; }

        //Transfer Restrictions
        public bool sellable { get; set; }
        public bool tradeable { get; set; }
        public bool newItem { get; set; }
        public bool lootable { get; set; }
        public bool blessable { get; set; }
        public bool curseable { get; set; }

        //Character Level Restrictions
        public int lowerLimit { get; set; }
        public int upperLimit { get; set; }

        //Minimum Stat Requirements
        public int requiredStr { get; set; }
        public int requiredVit { get; set; }
        public int requiredDex { get; set; }
        public int requiredAgi { get; set; }
        public int requiredInt { get; set; }
        public int requiredPie { get; set; }
        public int requiredLuk { get; set; }

        //Soul Level Requirement
        public int requiredSoulLevel { get; set; }

        //Allignment Requirement
        public string requiredAlignment { get; set; }

        //Race Requirement
        public int requiredHuman { get; set; }
        public int requiredElf { get; set; }
        public int requiredDwarf { get; set; }
        public int requiredGnome { get; set; }
        public int requiredPorkul { get; set; }

        //Gender Requirement
        public string requiredGender { get; set; }

        //Special Description Text
        public string whenEquippedText { get; set; }


















    }
}
