namespace Necromancy.Server.Data.Setting
{
    public class ModelCommonSetting : ISettingRepositoryItem
    {
        public int radius { get; set; }
        public int height { get; set; }
        public int crouchHeight { get; set; }
        public int nameHeight { get; set; }
        public ModelAtrSetting atr { get; set; }
        public int zRadiusOffset { get; set; }
        public int effect { get; set; }
        public int active { get; set; }

        /// <summary>
        ///     Developer Comment
        /// </summary>
        public string remarks { get; set; }

        public MonsterSetting monster { get; set; }
        public int id { get; set; }
    }
}
