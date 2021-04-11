

namespace Necromancy.Server.Model.Stats
{
    public class Attribute
    {
        public int hp { get; set; }
        public int mp { get; set; }
        public ushort str { get; set; }
        public ushort vit { get; set; }
        public ushort dex { get; set; }
        public ushort agi { get; set; }
        public ushort @int { get; set; }
        public ushort pie { get; set; }
        public ushort luck { get; set; }

        public Attribute()
        {
            hp = 0;
            mp = 0;
            str = 0;
            vit = 0;
            dex = 0;
            agi = 0;
            @int = 0;
            pie = 0;
            luck = 0;
        }

        public Attribute DefaultClassAtributes(uint @class)
        {
            switch (@class)
            {
                case 0: this.hp = 60; this.mp = 20; this.str = 8; this.vit = 7; this.dex = 7; this.agi = 6; this.@int = 8; this.pie = 5; this.luck = 8; break;//human
                case 1: this.hp = 50; this.mp = 30; this.str = 6; this.vit = 5; this.dex = 8; this.agi = 8; this.@int = 10; this.pie = 0; this.luck = 4; break;//elf
                case 2: this.hp = 80; this.mp = 15; this.str = 9; this.vit = 8; this.dex = 8; this.agi = 4; this.@int = 5; this.pie = 9; this.luck = 5; break;//dwarf
                case 3: this.hp = 50; this.mp = 20; this.str = 5; this.vit = 6; this.dex = 9; this.agi = 12; this.@int = 7; this.pie = 7; this.luck = 15; break;//porkul
                case 4: this.hp = 70; this.mp = 25; this.str = 7; this.vit = 7; this.dex = 4; this.agi = 7; this.@int = 6; this.pie = 10; this.luck = 6; break;//gnome
            }
            return this;
        }

}
}
