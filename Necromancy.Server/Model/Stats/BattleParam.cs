

namespace Necromancy.Server.Model.Stats
{
    public class BattleParam
    {
        public short PlusPhysicalAttack { get; set; }
        public short PlusPhysicalDefence { get; set; }
        public short PlusMagicalAttack { get; set; }
        public short PlusMagicalDefence { get; set; }
        public short PlusRangedAttack { get; set; }
        public ushort PlusStrength { get; set; }
        public ushort PlusVitality { get; set; }
        public ushort PlusDexterity { get; set; }
        public ushort PlusAgility { get; set; }
        public ushort PlusIntelligence { get; set; }
        public ushort PlusPiety { get; set; }
        public ushort PlusLuck { get; set; }

        public BattleParam()
        {
            PlusPhysicalAttack = 0;
            PlusPhysicalDefence = 0;
            PlusMagicalAttack = 0;
            PlusMagicalDefence = 0;
            PlusRangedAttack = 0;
            PlusStrength = 0;
            PlusVitality = 1;
            PlusDexterity = 2;
            PlusAgility = 3;
            PlusIntelligence = 4;
            PlusPiety = 5;
            PlusLuck = 6;
        }
    }
}
