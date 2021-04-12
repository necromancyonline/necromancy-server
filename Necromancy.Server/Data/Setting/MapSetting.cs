namespace Necromancy.Server.Data.Setting
{
    public class MapSetting : ISettingRepositoryItem
    {
        public string country { get; set; }
        public string area { get; set; }
        public string place { get; set; }
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int orientation { get; set; }
        public int id { get; set; }
    }
}
