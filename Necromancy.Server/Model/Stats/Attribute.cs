namespace Necromancy.Server.Model.Stats
{
    public class Attribute
    {
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

        public int hp { get; set; }
        public int mp { get; set; }
        public ushort str { get; set; }
        public ushort vit { get; set; }
        public ushort dex { get; set; }
        public ushort agi { get; set; }
        public ushort @int { get; set; }
        public ushort pie { get; set; }
        public ushort luck { get; set; }

        public Attribute DefaultClassAtributes(uint @class)
        {
            switch (@class)
            {
                case 0:
                    hp = 60;
                    mp = 20;
                    str = 8;
                    vit = 7;
                    dex = 7;
                    agi = 6;
                    @int = 8;
                    pie = 5;
                    luck = 8;
                    break; //human
                case 1:
                    hp = 50;
                    mp = 30;
                    str = 6;
                    vit = 5;
                    dex = 8;
                    agi = 8;
                    @int = 10;
                    pie = 0;
                    luck = 4;
                    break; //elf
                case 2:
                    hp = 80;
                    mp = 15;
                    str = 9;
                    vit = 8;
                    dex = 8;
                    agi = 4;
                    @int = 5;
                    pie = 9;
                    luck = 5;
                    break; //dwarf
                case 3:
                    hp = 50;
                    mp = 20;
                    str = 5;
                    vit = 6;
                    dex = 9;
                    agi = 12;
                    @int = 7;
                    pie = 7;
                    luck = 15;
                    break; //porkul
                case 4:
                    hp = 70;
                    mp = 25;
                    str = 7;
                    vit = 7;
                    dex = 4;
                    agi = 7;
                    @int = 6;
                    pie = 10;
                    luck = 6;
                    break; //gnome
            }

            return this;
        }
    }
}
