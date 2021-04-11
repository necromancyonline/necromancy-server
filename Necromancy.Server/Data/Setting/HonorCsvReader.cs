namespace Necromancy.Server.Data.Setting
{
    public class HonorCsvReader : CsvReader<HonorSetting>
    {
        protected override int numExpectedItems => 3;

        protected override HonorSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            string name = properties[1];
            string condition = properties[2];

            //if (!int.TryParse(properties[3], out int effectId))
            //{
            //    return null;
            //}

            //if (!int.TryParse(properties[4], out int hiddenTitle))
            //{
            //    hiddenTitle = 0;
            //}

            //if (!int.TryParse(properties[5], out int alwaysDisplayTitle))
            //{
            //    alwaysDisplayTitle = 0;
            //}

            //if (!int.TryParse(properties[6], out int prerequesit))
            //{
            //    prerequesit = 0;
            //}

            return new HonorSetting
            {
                id = id,
                name = name,
                condition = condition,
                //EffectId = effectId,
                //HiddenTitle = hiddenTitle,
                //AlwaysDisplayTitle = alwaysDisplayTitle,
                //Prerequesit = prerequesit
            };
        }
    }
}
