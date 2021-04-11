namespace Necromancy.Server.Data.Setting
{
    public class MonsterCsvReader : CsvReader<MonsterSetting>
    {
        protected override int numExpectedItems => 30;

        protected override MonsterSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                return null;
            }

            if (!int.TryParse(properties[3], out int catalogId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[4], out int? effectId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[5], out int? activeEffectId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[6], out int? inactiveEffectId))
            {
                return null;
            }

            if (!TryParseNullableInt(properties[9], out int? modelSwitching))
            {
                return null;
            }

            if (!int.TryParse(properties[10], out int attackSkillId))
            {
                return null;
            }


            if (!int.TryParse(properties[11], out int level))
            {
                return null;
            }

            if (!int.TryParse(properties[12], out int combatMode))
            {
                return null;
            }


            if (!int.TryParse(properties[13], out int textureType))
            {
                return null;
            }

            return new MonsterSetting
            {
                id = id,
                name = properties[1],
                title = properties[2],
                catalogId = catalogId,
                effectId = effectId,
                activeEffectId = activeEffectId,
                inactiveEffectId = inactiveEffectId,
                namePlateType = properties[7],
                modelSwitching = modelSwitching,
                attackSkillId = attackSkillId,
                level = level,
                combatMode = combatMode == 1,
                textureType = textureType
            };
        }
    }
}
