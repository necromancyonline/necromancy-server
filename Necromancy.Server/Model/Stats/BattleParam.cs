

namespace Necromancy.Server.Model.Stats
{
    public class BattleParam
    {
        public short PlusPhysicalAttack { get; set; }
        public short PlusPhysicalDefence { get; set; }
        public short PlusMagicalAttack { get; set; }
        public short PlusMagicalDefence { get; set; }
        public short PlusRangedAttack { get; set; }

        public BattleParam()
        {
            PlusPhysicalAttack = 0;
            PlusPhysicalDefence = 0;
            PlusMagicalAttack = 0;
            PlusMagicalDefence = 0;
            PlusRangedAttack = 0;
        }
    }
}
