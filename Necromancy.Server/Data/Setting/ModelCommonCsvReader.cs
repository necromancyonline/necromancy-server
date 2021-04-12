using System.Collections.Generic;

namespace Necromancy.Server.Data.Setting
{
    public class ModelCommonCsvReader : CsvReader<ModelCommonSetting>
    {
        private readonly Dictionary<int, ModelAtrSetting> _modelAtrSettings;
        private readonly Dictionary<int, MonsterSetting> _monsterSetting;

        public ModelCommonCsvReader(Dictionary<int, MonsterSetting> monsterSetting,
            Dictionary<int, ModelAtrSetting> modelAtrSettings)
        {
            _monsterSetting = monsterSetting;
            _modelAtrSettings = modelAtrSettings;
        }

        protected override int numExpectedItems => 14;

        protected override ModelCommonSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id)) return null;

            if (!int.TryParse(properties[1], out int radius)) return null;

            if (!int.TryParse(properties[2], out int height)) return null;

            if (!int.TryParse(properties[3], out int crouchHeight)) return null;

            if (!int.TryParse(properties[4], out int nameHeight)) return null;

            if (!int.TryParse(properties[5], out int modelAtrId)) return null;

            if (!int.TryParse(properties[6], out int zRadiusOffset)) return null;

            if (!int.TryParse(properties[7], out int effectScaling))
                //return null;
                effectScaling = 0;

            if (!int.TryParse(properties[8], out int active))
                //return null;
                active = 0;

            //10 = damage sprite height
            //11 = bow firing position z offset
            //12 = Gun firing position Z offset
            //13 = hide shadows
            //14 = model name

            //commenting out.  jp no longer contained a setting identifier in model_common like US
            MonsterSetting monster = null;
            //if (properties.Length > 11 && int.TryParse(properties[11], out int monsterId))
            //{
            //    if (_monsterSetting.ContainsKey(monsterId))
            //    {
            //        monster = _monsterSetting[monsterId];
            //    }
            //}

            ModelAtrSetting atr = null;
            if (_modelAtrSettings.ContainsKey(modelAtrId)) atr = _modelAtrSettings[modelAtrId];

            return new ModelCommonSetting
            {
                id = id,
                radius = radius,
                height = height,
                crouchHeight = crouchHeight,
                nameHeight = nameHeight,
                atr = atr,
                zRadiusOffset = zRadiusOffset,
                effect = effectScaling,
                active = active,
                remarks = properties[14], //this is the name of the model
                monster = monster
            };
        }
    }
}
