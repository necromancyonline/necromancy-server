namespace Necromancy.Server.Data.Setting
{
    public class MapSetting : ISettingRepositoryItem
    {
        public int id { get; set; }
        public string country { get; set; }
        public string area { get; set; }
        public string place { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int orientation { get; set; }
    }
}
