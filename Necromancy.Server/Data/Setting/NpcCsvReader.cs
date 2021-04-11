namespace Necromancy.Server.Data.Setting
{
    public class NpcCsvReader : CsvReader<NpcSetting>
    {
        protected override int numExpectedItems => 34;

        protected override NpcSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[20], out int level))
            {
                return null;
            }

            return new NpcSetting
            {
                id = id,
                level = level,
                name = properties[1],
                title = properties[20]
            };
        }
    }
}
