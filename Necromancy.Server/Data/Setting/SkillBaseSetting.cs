namespace Necromancy.Server.Data.Setting
{
    public class SkillBaseSetting : ISettingRepositoryItem
    {
        public string name { get; set; }

        public int logId { get; set; }
        public bool logBlockEnemy { get; set; }
        public int castLogId { get; set; }
        public int hitLogId { get; set; }
        public string effectType { get; set; }
        public int occupationEffectType { get; set; }
        public float castingTime { get; set; }
        public float castingCooldown { get; set; }
        public int changeByMapId { get; set; }
        public int rigidityTime { get; set; }
        public int noSword { get; set; }
        public int necessaryLevel { get; set; }
        public int hpUsed { get; set; }
        public int mpUsed { get; set; }
        public int apUsed { get; set; }
        public int acUsed { get; set; }
        public int durabilityUsed { get; set; }
        public int item1Id { get; set; }
        public int item1Count { get; set; }
        public int item2Id { get; set; }
        public int item2Count { get; set; }
        public int item3Id { get; set; }
        public int item3Count { get; set; }
        public int item4Id { get; set; }
        public int item4Count { get; set; }
        public int castScriptId { get; set; }
        public int activatedScriptId { get; set; }
        public int activatedEffect1Id { get; set; }
        public int activatedEffect2Id { get; set; }
        public int equipmentScriptChange { get; set; }
        public bool effectOnSelf { get; set; }
        public string objectFaction { get; set; }
        public int automaticCombo { get; set; }
        public int hitEffect2 { get; set; }
        public int scriptParameter1 { get; set; }
        public int scriptParameter2 { get; set; }
        public string scanType { get; set; }
        public int unknown1 { get; set; }
        public int unknown2 { get; set; }
        public int unknown3 { get; set; }
        public int unknown4 { get; set; }
        public string displayName { get; set; }
        public int effectTime { get; set; }
        public int id { get; set; }
        public string characteristicEffectType { get; set; }

    }
}
