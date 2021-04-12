using Arrowgene.Logging;

namespace Necromancy.Server.Data.Setting
{
    public class CharacterAttackCsvReader : CsvReader<CharacterAttackSetting>
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(CharacterAttackCsvReader));

        protected override int numExpectedItems => 40;

        protected override CharacterAttackSetting CreateInstance(string[] properties)
        {
            if (!int.TryParse(properties[0], out int id))
            {
                _Logger.Debug("First entry empty!!");
                return null;
            }

            int.TryParse(properties[1], out int motionId);
            bool.TryParse(properties[3], out bool firstShot);
            int.TryParse(properties[4], out int nextAttackId);
            int.TryParse(properties[5], out int channel);
            int.TryParse(properties[6], out int moveStart);
            int.TryParse(properties[7], out int moveEnd);
            int.TryParse(properties[8], out int moveAmount);
            int.TryParse(properties[9], out int swordShadowStart);
            int.TryParse(properties[10], out int swordShadowEnd);
            int.TryParse(properties[11], out int socket1Type);
            int.TryParse(properties[12], out int fx1Id);
            int.TryParse(properties[13], out int socket2Type);
            int.TryParse(properties[14], out int fx2Id);
            int.TryParse(properties[15], out int interruptStart);
            int.TryParse(properties[16], out int interruptEnd);
            int.TryParse(properties[17], out int rigidTime);
            int.TryParse(properties[18], out int inputReception);
            int.TryParse(properties[19], out int hit);
            int.TryParse(properties[20], out int guardCanel);
            int.TryParse(properties[21], out int attackAtariStart);
            int.TryParse(properties[22], out int attackAtariEnd);
            float.TryParse(properties[23], out float consecutiveAttackStart);
            float.TryParse(properties[24], out float continuousAttackEnd);
            float.TryParse(properties[25], out float delay);
            float.TryParse(properties[26], out float rigidity);
            bool.TryParse(properties[27], out bool reuse);

            return new CharacterAttackSetting
            {
                id = id,
                motionId = motionId,
                weapon = properties[2],
                firstShot = firstShot,
                nextAttackId = nextAttackId,
                channel = channel,
                moveStart = moveStart,
                moveEnd = moveEnd,
                moveAmount = moveAmount,
                swordShadowStart = swordShadowStart,
                swordShadowEnd = swordShadowEnd,
                socket1Type = socket1Type,
                fx1Id = fx1Id,
                socket2Type = socket2Type,
                fx2Id = fx2Id,
                interruptStart = interruptStart,
                interruptEnd = interruptEnd,
                rigidTime = rigidTime,
                inputReception = inputReception,
                hit = hit,
                guardCanel = guardCanel,
                attackAtariStart = attackAtariStart,
                attackAtariEnd = attackAtariEnd,
                consecutiveAttackStart = consecutiveAttackStart,
                continuousAttackEnd = continuousAttackEnd,
                delay = delay,
                rigidity = rigidity,
                reuse = reuse
            };
        }
    }
}
