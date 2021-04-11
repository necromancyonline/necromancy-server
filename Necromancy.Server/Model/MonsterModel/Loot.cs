

using Necromancy.Server.Common;

namespace Necromancy.Server.Model
{
    public class Loot
    {
        public ulong gold { get; set; }
        public ulong experience { get; set; }
        public int[] dropTableItemSerialIds { get; set; }
        public byte itemCountRng { get; set; }

        public Loot(int level, int monsterSerialId)
        {
            gold = (ulong)(level * Util.GetRandomNumber(1,5));
            experience = (ulong)(level * level * level + level); //yea, make a better Exp calc, or find an exp table.
            itemCountRng = (byte) Util.GetRandomNumber(0,5); //number of items from loot table you get to loot
            dropTableItemSerialIds = new int[] { 50100302, 51000401, 90039902, 100111, 50100504, 210311 }; //Temporary.  reference an actual loot table by serial ID later.

        }


    }
}
