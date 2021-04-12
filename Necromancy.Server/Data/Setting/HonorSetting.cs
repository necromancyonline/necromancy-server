namespace Necromancy.Server.Data.Setting
{
    public class HonorSetting : ISettingRepositoryItem
    {
        public string name { get; set; }

        public string condition { get; set; }

//        public int EffectId { get; set; }
        public int hiddenTitle { get; set; }
        public int alwaysDisplayTitle { get; set; }
        public int prerequesit { get; set; }
        public int id { get; set; }
    }
}
