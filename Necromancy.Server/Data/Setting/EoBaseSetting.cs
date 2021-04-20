namespace Necromancy.Server.Data.Setting
{
    public class EoBaseSetting : ISettingRepositoryItem
    {
        public string name { get; set; }
        public int logId { get; set; }
        public string faction { get; set; }
        public bool onlyOwner { get; set; }
        public bool showActivationTime { get; set; }
        public bool showName { get; set; }
        public string damageShape { get; set; }
        public int effectRadius { get; set; }
        public int id { get; set; }
    }
}
