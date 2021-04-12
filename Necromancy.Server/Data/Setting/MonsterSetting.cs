namespace Necromancy.Server.Data.Setting
{
    public class MonsterSetting : ISettingRepositoryItem
    {
        public string name { get; set; }
        public string title { get; set; }
        public int catalogId { get; set; }
        public int? effectId { get; set; }
        public int? activeEffectId { get; set; }
        public int? inactiveEffectId { get; set; }
        public string namePlateType { get; set; }
        public int? modelSwitching { get; set; }
        public int attackSkillId { get; set; }
        public int level { get; set; }
        public bool combatMode { get; set; }
        public int textureType { get; set; }
        public int id { get; set; }
    }
}
