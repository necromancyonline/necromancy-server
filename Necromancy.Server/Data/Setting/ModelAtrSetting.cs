namespace Necromancy.Server.Data.Setting
{
    public class ModelAtrSetting : ISettingRepositoryItem
    {
        public float normalMagnification { get; set; }
        public float crouchingMagnification { get; set; }
        public float sittingMagnification { get; set; }
        public float rollingMagnification { get; set; }
        public float deathMagnification { get; set; }
        public float motionMagnification { get; set; }
        public int id { get; set; }
    }
}
