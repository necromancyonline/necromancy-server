namespace Necromancy.Server.Data.Setting
{
    public class NpcSetting : ISettingRepositoryItem
    {
        public int level { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int id { get; set; }
    }
}
