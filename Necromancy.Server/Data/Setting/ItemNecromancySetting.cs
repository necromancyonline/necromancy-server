namespace Necromancy.Server.Data.Setting
{
    /// <summary>
    ///     Additional Item information, that was not provided or could not be extracted from the client.
    /// </summary>
    public class ItemNecromancySetting : ISettingRepositoryItem
    {
        public string name { get; set; }

        public int physical { get; set; }
        public int magical { get; set; }
        public int durability { get; set; }
        public int hardness { get; set; }
        public float weight { get; set; }
        public int id { get; set; }
    }
}
