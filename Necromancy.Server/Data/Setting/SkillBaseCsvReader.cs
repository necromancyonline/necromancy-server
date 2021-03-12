using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class SkillBaseCsvReader : CsvReader<SkillBaseSetting>
    {
        private static readonly ILogger Logger = LogProvider.Logger(typeof(SkillBaseCsvReader));

        protected override int NumExpectedItems => 40;

        protected override SkillBaseSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                //Logger.Debug("First entry empty!!");
                return null;
            }

            int.TryParse(properties[2], out int logId);
            bool logBlockEnemy = false;
            bool.TryParse(properties[3], out logBlockEnemy);

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
                Id = id,
                Name = properties[1],
                LogId = logId,
                LogBlockEnemy = logBlockEnemy,
                CastLogId = castLogId,
                HitLogId = hitLogId,
                EffectType = properties[6],
                OccupationEffectType = occupationEffectType,
                CastingTime = castingTime,
                CastingCooldown = castingCooldown,
                ChangeByMapId = changeByMapId,
                RigidityTime = rigidityTime,
                NoSword = noSword,
                NecessaryLevel = necessaryLevel,
                HpUsed = hpUsed,
                MpUsed = mpUsed,
                ApUsed = apUsed,
                AcUsed = acUsed,
                DurabilityUsed = durabilityUsed,
                Item1Id = item1Id,
                Item1Count = item1Count,
                Item2Id = item2Id,
                Item2Count = item2Count,
                Item3Id = item3Id,
                Item3Count = item3Count,
                Item4Id = item4Id,
                Item4Count = item4Count,
                CastScriptId = castScriptId,
                ActivatedScriptId = activatedScriptId,
                ActivatedEffect1Id = activatedEffect1Id,
                ActivatedEffect2Id = activatedEffect2Id,
                EquipmentScriptChange = equipmentScriptChange,
                EffectOnSelf = effectOnSelf,
                ObjectFaction = properties[33],
                AutomaticCombo = automaticCombo,
                HitEffect2 = hitEffect2,
                ScriptParameter1 = scriptParameter1,
                ScriptParameter2 = scriptParameter2,
                ScanType = properties[38],
                Unknown2 = unknown2,
                Unknown3 = unknown3,
                EffectTime = effectTime
            };
        }
    }
}
