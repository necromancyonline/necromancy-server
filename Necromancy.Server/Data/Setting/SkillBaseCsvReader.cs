using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SkillBaseCsvReader : CsvReader<SkillBaseSetting>
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(SkillBaseCsvReader));

        protected override int numExpectedItems => 40;

        protected override SkillBaseSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
                //Logger.Debug("First entry empty!!");
                return null;

            int.TryParse(properties[2], out int logId);
            bool.TryParse(properties[3], out bool logBlockEnemy);

            int.TryParse(properties[4], out int castLogId);

            int.TryParse(properties[5], out int hitLogId);

            int.TryParse(properties[7], out int occupationEffectType);
            float.TryParse(properties[8], out float castingTime);
            float.TryParse(properties[9], out float castingCooldown);
            int.TryParse(properties[10], out int changeByMapId);
            int.TryParse(properties[11], out int rigidityTime);
            int.TryParse(properties[12], out int noSword);
            int.TryParse(properties[13], out int necessaryLevel);
            int.TryParse(properties[14], out int hpUsed);
            int.TryParse(properties[15], out int mpUsed);
            int.TryParse(properties[16], out int apUsed);
            int.TryParse(properties[17], out int acUsed);
            int.TryParse(properties[18], out int durabilityUsed);
            int.TryParse(properties[19], out int item1Id);
            int.TryParse(properties[20], out int item1Count);
            int.TryParse(properties[21], out int item2Id);
            int.TryParse(properties[22], out int item2Count);
            int.TryParse(properties[23], out int item3Id);
            int.TryParse(properties[24], out int item3Count);
            int.TryParse(properties[25], out int item4Id);
            int.TryParse(properties[26], out int item4Count);
            int.TryParse(properties[27], out int castScriptId);
            int.TryParse(properties[28], out int activatedScriptId);
            int.TryParse(properties[29], out int activatedEffect1Id);
            int.TryParse(properties[30], out int activatedEffect2Id);
            int.TryParse(properties[31], out int equipmentScriptChange);
            bool.TryParse(properties[32], out bool effectOnSelf);


            int.TryParse(properties[34], out int automaticCombo);
            int.TryParse(properties[35], out int hitEffect2);
            int.TryParse(properties[36], out int scriptParameter1);
            int.TryParse(properties[37], out int scriptParameter2);
            string displayName = "";
            int effectTime = 0;
            int unknown2 = 0;
            int unknown3 = 0;
            int unknown4 = 0;
            int unknown5 = 0;
            if (properties.Length >= 46)
            {
                int.TryParse(properties[39], out unknown2);
                int.TryParse(properties[40], out unknown3);
                int.TryParse(properties[41], out unknown4);
                int.TryParse(properties[42], out unknown5);
                displayName = properties[43];
                int.TryParse(properties[44], out effectTime);
            }

            return new SkillBaseSetting
            {
                id = id,
                name = properties[1],
                logId = logId,
                logBlockEnemy = logBlockEnemy,
                castLogId = castLogId,
                hitLogId = hitLogId,
                effectType = properties[6],
                occupationEffectType = occupationEffectType,
                castingTime = castingTime,
                castingCooldown = castingCooldown,
                changeByMapId = changeByMapId,
                rigidityTime = rigidityTime,
                noSword = noSword,
                necessaryLevel = necessaryLevel,
                hpUsed = hpUsed,
                mpUsed = mpUsed,
                apUsed = apUsed,
                acUsed = acUsed,
                durabilityUsed = durabilityUsed,
                item1Id = item1Id,
                item1Count = item1Count,
                item2Id = item2Id,
                item2Count = item2Count,
                item3Id = item3Id,
                item3Count = item3Count,
                item4Id = item4Id,
                item4Count = item4Count,
                castScriptId = castScriptId,
                activatedScriptId = activatedScriptId,
                activatedEffect1Id = activatedEffect1Id,
                activatedEffect2Id = activatedEffect2Id,
                equipmentScriptChange = equipmentScriptChange,
                effectOnSelf = effectOnSelf,
                objectFaction = properties[33],
                automaticCombo = automaticCombo,
                hitEffect2 = hitEffect2,
                scriptParameter1 = scriptParameter1,
                scriptParameter2 = scriptParameter2,
                scanType = properties[38],
                unknown2 = unknown2,
                unknown3 = unknown3,
                effectTime = effectTime
            };
        }
    }
}
