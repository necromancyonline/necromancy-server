

namespace Necromancy.Server.Model.Stats
{
    public class BattleParam
    {
        public short plusPhysicalAttack { get; set; }
        public short plusPhysicalDefence { get; set; }
        public short plusMagicalAttack { get; set; }
        public short plusMagicalDefence { get; set; }
        public short plusRangedAttack { get; set; }
        public ushort plusStrength { get; set; }
        public ushort plusVitality { get; set; }
        public ushort plusDexterity { get; set; }
        public ushort plusAgility { get; set; }
        public ushort plusIntelligence { get; set; }
        public ushort plusPiety { get; set; }
        public ushort plusLuck { get; set; }

        public BattleParam()
        {
            plusPhysicalAttack = 0;
            plusPhysicalDefence = 0;
            plusMagicalAttack = 0;
            plusMagicalDefence = 0;
            plusRangedAttack = 0;
            plusStrength = 0;
            plusVitality = 1;
            plusDexterity = 2;
            plusAgility = 3;
            plusIntelligence = 4;
            plusPiety = 5;
            plusLuck = 6;
        }
    }
}
