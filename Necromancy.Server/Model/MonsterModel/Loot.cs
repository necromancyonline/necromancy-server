

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

        //public Loot DefaultClassAtributes(uint Class)
        //{
        //    switch (Class)
        //    {
        //        case 0: this.Hp = 60; this.Mp = 20; this.Str = 8; this.Vit = 7; this.Dex = 7; this.Agi = 6; this.Int = 8; this.Pie = 5; this.Luck = 8; break;//human
        //        case 1: this.Hp = 50; this.Mp = 30; this.Str = 6; this.Vit = 5; this.Dex = 8; this.Agi = 8; this.Int = 10; this.Pie = 0; this.Luck = 4; break;//elf
        //        case 2: this.Hp = 80; this.Mp = 15; this.Str = 9; this.Vit = 8; this.Dex = 8; this.Agi = 4; this.Int = 5; this.Pie = 9; this.Luck = 5; break;//dwarf
        //        case 3: this.Hp = 50; this.Mp = 20; this.Str = 5; this.Vit = 6; this.Dex = 9; this.Agi = 12; this.Int = 7; this.Pie = 7; this.Luck = 15; break;//porkul
        //        case 4: this.Hp = 70; this.Mp = 25; this.Str = 7; this.Vit = 7; this.Dex = 4; this.Agi = 7; this.Int = 6; this.Pie = 10; this.Luck = 6; break;//gnome
        //    }
        //    return this;
        //}

}
}
