

using Necromancy.Server.Common;

namespace Necromancy.Server.Model
{
    public class Loot
    {
        public ulong Gold { get; set; }
        public ulong Experience { get; set; }
        public int[] DropTableItemSerialIds { get; set; }
        public byte ItemCountRNG { get; set; }

        public Loot(int level, int monsterSerialId)
        {
            Gold = (ulong)(level * Util.GetRandomNumber(1,5));
            Experience = (ulong)(level * level * level + level); //yea, make a better Exp calc, or find an exp table.
            ItemCountRNG = (byte) Util.GetRandomNumber(0,5); //number of items from loot table you get to loot
            DropTableItemSerialIds = new int[] { 50100302, 51000101, 61100017, 100111, 200102 , 210311 }; //Temporary.  reference an actual loot table by serial ID later.

        }


    }
}
