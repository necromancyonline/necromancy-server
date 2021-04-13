namespace Necromancy.Server.Data.Setting
{
    public class MapSymbolSetting : ISettingRepositoryItem
    {
        public int map { get; set; }
        public int displayConditionflag { get; set; }
        public int splitMapNumber { get; set; }
        public int settingTypeFlag { get; set; }
        public int settingIdOrText { get; set; }
        public int iconType { get; set; }
        public int displayPositionX { get; set; }
        public int displayPositionY { get; set; }
        public int displayPositionZ { get; set; }
        public int id { get; set; }
    }
}
