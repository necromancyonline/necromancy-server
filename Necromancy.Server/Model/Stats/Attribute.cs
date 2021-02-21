

namespace Necromancy.Server.Model.Stats
{
    public class Attribute
    {
        public int Hp { get; set; }
        public int Mp { get; set; }
        public ushort Str { get; set; }
        public ushort Vit { get; set; }
        public ushort Dex { get; set; }
        public ushort Agi { get; set; }
        public ushort Int { get; set; }
        public ushort Pie { get; set; }
        public ushort Luck { get; set; }

        public Attribute()
        {
            Hp = 0;
            Mp = 0;
            Str = 0;
            Vit = 0;
            Dex = 0;
            Agi = 0;
            Int = 0;
            Pie = 0;
            Luck = 0;
        }

        public Attribute DefaultClassAtributes(uint Class)
        {
            switch (Class)
            {
                case 0: this.Hp = 60; this.Mp = 20; this.Str = 8; this.Vit = 7; this.Dex = 7; this.Agi = 6; this.Int = 8; this.Pie = 5; this.Luck = 8; break;//human
                case 1: this.Hp = 50; this.Mp = 30; this.Str = 6; this.Vit = 5; this.Dex = 8; this.Agi = 8; this.Int = 10; this.Pie = 0; this.Luck = 4; break;//elf
                case 2: this.Hp = 80; this.Mp = 15; this.Str = 9; this.Vit = 8; this.Dex = 8; this.Agi = 4; this.Int = 5; this.Pie = 9; this.Luck = 5; break;//dwarf
                case 3: this.Hp = 50; this.Mp = 20; this.Str = 5; this.Vit = 6; this.Dex = 9; this.Agi = 12; this.Int = 7; this.Pie = 7; this.Luck = 15; break;//porkul
                case 4: this.Hp = 70; this.Mp = 25; this.Str = 7; this.Vit = 7; this.Dex = 4; this.Agi = 7; this.Int = 6; this.Pie = 10; this.Luck = 6; break;//gnome
            }
            return this;
        }

}
}
