namespace Necromancy.Server.Data.Setting
{
    public class ItemInfoCsvReader : CsvReader<ItemInfoSetting>
    {
        protected override int numExpectedItems => 2;

        protected override ItemInfoSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) return null;

            return new ItemInfoSetting
            {
                id = id,
                name = properties[1]
            };
        }
    }
}
