using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Msg
{
    public class send_chara_get_createinfo : ClientHandler
    {
        public send_chara_get_createinfo(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_chara_get_createinfo;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(0x6);//Hair type; Higher than 6 will cause a crash
            for (int i = 0; i < 0x6; i++)
            {
                res.WriteByte((byte)i);
            }

            res.WriteUInt32(0x6);//Hair color; Higher than 6 will cause a crash
            for (int i = 0; i < 0x6; i++)
            {
                res.WriteByte((byte)i);
            }

            res.WriteUInt32(0x5);//Face type; Higher than 5 will break the faces when going to the last one, and higher than 6 will cause a crash
            for (int i = 0; i < 0x5; i++)
            {
                res.WriteByte((byte)i);
            }

            res.WriteUInt32(5);
            wo_4E08E0_Human_Stats(res);//holds correct data for starting stats for Humans
            wo_4E08E0_Elf_Stats(res);//holds correct data for starting stats for Elfs
            wo_4E08E0_Dwarf_Stats(res);//holds correct data for starting stats for Dwarf
            wo_4E08E0_Porkul_Stats(res);//holds correct data for starting stats for Porkul
            wo_4E08E0_Gnome_Stats(res);//holds correct data for starting stats for Gnome

            res.WriteUInt32(0x8);//cmp to 0xA
            wo_4E3700_Human_Male(res);//Holds Male Human model details
            wo_4E3700_Human_Female(res);//Holds Female Human model details
            wo_4E3700_Elf_Male(res);//Holds Male Elf model details
            wo_4E3700_Elf_Female(res);//Holds Female Elf model details
            wo_4E3700_Dwarf_Male(res);//Holds Male Dwarf model details
            wo_4E3700_Gnome_Female(res);//Holds Female Gnome model details
            wo_4E3700_Prokul_Male(res);//Holds Male Porkul model details
            wo_4E3700_Porkul_Female(res);//Holds Female Porkul model details

            res.WriteUInt32(0x20);//cmp to 0x96
            wo_4E37F0_HumanMaleClassGear(res);
            wo_4E37F0_HumanFemaleClassGear(res);
            wo_4E37F0_ElfMaleClassGear(res);
            wo_4E37F0_ElfFemaleClassGear(res);
            wo_4E37F0_DwarfMaleClassGear(res);
            wo_4E37F0_GnomeFemaleClassGear(res);
            wo_4E37F0_PorkulMaleClassGear(res);
            wo_4E37F0_PorkulFemaleClassGear(res);

            res.WriteUInt32(4);//cmp to 0xF
            wo_4E0970_Fighter(res);//Holds Stats and info for Fighter Class(need to fix bonus HP+MP+Stats)
            wo_4E0970_Priest(res);//Holds Stats and info for Priest Class(need to fix bonus HP+MP+Stats)
            wo_4E0970_Thief(res);//Holds Stats and info for Thief Class(need to fix bonus HP+MP+Stats)
            wo_4E0970_Mage(res);//Holds Stats and info for Mage Class(need to fix bonus HP+MP+Stats)

            Router.Send(client, (ushort) MsgPacketId.recv_chara_get_createinfo_r, res, ServerType.Msg);
        }

        private void wo_4E08E0_Human_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(60);//HP
            res.WriteInt32(20);//MP
            res.WriteInt32(0);

            res.WriteInt16(8);//STR
            res.WriteInt16(7);//VIT
            res.WriteInt16(7);//DEX
            res.WriteInt16(6);//AGI
            res.WriteInt16(8);//INT
            res.WriteInt16(5);//PIE
            res.WriteInt16(8);//LUK
        }

        private void wo_4E08E0_Elf_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(50);//HP
            res.WriteInt32(30);//MP
            res.WriteInt32(0);

            res.WriteInt16(6);//STR
            res.WriteInt16(5);//VIT
            res.WriteInt16(8);//DEX
            res.WriteInt16(8);//AGI
            res.WriteInt16(10);//INT
            res.WriteInt16(8);//PIE
            res.WriteInt16(4);//LUK
        }

        private void wo_4E08E0_Dwarf_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(80);//HP
            res.WriteInt32(15);//MP
            res.WriteInt32(0);

            res.WriteInt16(9);//STR
            res.WriteInt16(8);//VIT
            res.WriteInt16(8);//DEX
            res.WriteInt16(4);//AGI
            res.WriteInt16(5);//INT
            res.WriteInt16(9);//PIE
            res.WriteInt16(5);//LUK
        }

        private void wo_4E08E0_Porkul_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(50);//HP
            res.WriteInt32(20);//MP
            res.WriteInt32(0);

            res.WriteInt16(5);//STR
            res.WriteInt16(6);//VIT
            res.WriteInt16(9);//DEX
            res.WriteInt16(12);//AGI
            res.WriteInt16(7);//INT
            res.WriteInt16(7);//PIE
            res.WriteInt16(15);//LUK
        }

        private void wo_4E08E0_Gnome_Stats(IBuffer res)
        {
            res.WriteInt32(1);
            res.WriteInt32(70);//HP
            res.WriteInt32(25);//MP
            res.WriteInt32(0);

            res.WriteInt16(7);//STR
            res.WriteInt16(7);//VIT
            res.WriteInt16(4);//DEX
            res.WriteInt16(7);//AGI
            res.WriteInt16(6);//INT
            res.WriteInt16(10);//PIE
            res.WriteInt16(6);//LUK
        }

        private void wo_4E3700_Elf_Female(IBuffer res) //characater creation area?
        {
            res.WriteInt32(1);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool


            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752401, 752401, 752401, 752401, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E3700_Human_Male(IBuffer res) //characater creation area?
        {

            res.WriteInt32(0);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752101, 752101, 752101, 752101, 0 , 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SlotContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SlotContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E3700_Human_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(0);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool

            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752201, 752201, 752201, 752201, 0 , 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E3700_Elf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(1);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752301, 752301, 752301, 752301, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display

        }

        private void wo_4E3700_Dwarf_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(2);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool

            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752501, 752501, 752501, 752501, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }


        private void wo_4E3700_Gnome_Female(IBuffer res) //characater creation area?

        {
            // 4 byte
            res.WriteInt32(4);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool

            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 753001, 753001, 753001, 753001, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E3700_Prokul_Male(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(3);//race ID
            res.WriteInt32(0);//gender flag

            res.WriteByte(1);//bool


            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752701, 752701, 752701, 752701, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E3700_Porkul_Female(IBuffer res) //characater creation area?
        {
            // 4 byte
            res.WriteInt32(3);//race ID
            res.WriteInt32(1);//gender flag

            res.WriteByte(1);//bool


            int[] WeaponContainer = new int[] { 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, (int)ItemType.SHOES,
                                                0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.HELMET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.GLOVES, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int x = 0;
            int xx = 0;
            int xxx = 0;
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(WeaponContainer[x]);

                x++;
            }

            int[] EquipmentContainer = new int[] { 0, 0, 260103, 110504, 360103, 460103, 560103, 0, 0, 0, 0, 0, 752801, 752801, 752801, 752801, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(EquipmentContainer[xx]);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);//bool
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);

                xx++;
            }

            int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

            for (int i = 0; i < 0x19; i++)
            {
                res.WriteInt32(SoltContainer[xxx]);

                xxx++;
            }

            res.WriteByte(0x19); // how many items to display
        }

        private void wo_4E37F0_HumanMaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {
                
                res.WriteInt32(0);//race
                res.WriteInt32(0);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250101, 550101, 450101, 350101, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250101, 550101, 450101, 350101, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250101, 550101, 450101, 350101, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250101, 550101, 450101, 350101, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }

        private void wo_4E37F0_HumanFemaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(0);//race
                res.WriteInt32(1);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250201, 550201, 450201, 350201, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250201, 550201, 450201, 350201, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250201, 550201, 450201, 350201, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250201, 550201, 450201, 350201, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_ElfMaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(1);//race
                res.WriteInt32(0);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250301, 550301, 450301, 350301, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250301, 550301, 450301, 350301, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250301, 550301, 450301, 350301, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250301, 550301, 450301, 350301, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_ElfFemaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(1);//race
                res.WriteInt32(1);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250401, 550401, 450401, 350401, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250401, 550401, 450401, 350401, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250401, 550401, 450401, 350401, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250401, 550401, 450401, 350401, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_DwarfMaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(2);//race
                res.WriteInt32(0);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250501, 550501, 450501, 350501, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250501, 550501, 450501, 350501, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250501, 550501, 450501, 350501, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250501, 550501, 450501, 350501, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_GnomeFemaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(4);//race
                res.WriteInt32(1);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 251001, 551001, 451001, 351001, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 251001, 551001, 451001, 351001, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 251001, 551001, 451001, 351001, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 251001, 551001, 451001, 351001, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_PorkulMaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(3);//race
                res.WriteInt32(0);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250701, 550701, 450701, 350701, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250701, 550701, 450701, 350701, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250701, 550701, 450701, 350701, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250701, 550701, 450701, 350701, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }


        private void wo_4E37F0_PorkulFemaleClassGear(IBuffer res)
        {
            int classSlot = 0;
            for (int load = 0; load < 4; load++)
            {

                res.WriteInt32(3);//race
                res.WriteInt32(1);//gender
                res.WriteInt32(classSlot);//class

                int[] WeaponContainer0 = new int[] { (int)ItemType.SWORD_1H, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer1 = new int[] { (int)ItemType.DAGGER, (int)ItemType.SHIELD_SMALL, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS,
                                                     (int)ItemType.SHOES, (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer2 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] WeaponContainer3 = new int[] { (int)ItemType.WAND_2H, 0, 0, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0, (int)ItemType.ARMOR_TOPS, (int)ItemType.SHOES,
                                                     (int)ItemType.BRACLET, (int)ItemType.ARMOR_BOTTOMS, (int)ItemType.HELMET, 0, 0, 0, 0, 0, 0, 0, 0 };

                int x = 0;
                int xx = 0;
                int xxx = 0;
                if (classSlot == 0)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer0[x]);

                        x++;
                    }
                }
                if (classSlot == 1)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer1[x]);

                        x++;
                    }
                }
                if (classSlot == 2)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer2[x]);

                        x++;
                    }
                }
                if (classSlot == 3)
                {
                    for (int i = 0; i < 0x19; i++)
                    {
                        res.WriteInt32(WeaponContainer3[x]);

                        x++;
                    }
                }
                int[] EQC0 = new int[] { 10300101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250801, 550801, 450801, 350801, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC1 = new int[] { 10200101, 15000101, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250801, 550801, 450801, 350801, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC2 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250801, 550801, 450801, 350801, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };
                int[] EQC3 = new int[] { 11300101, 0, 0, 120101, 0, 0, 0, 0, 0, 0, 0, 0, 250801, 550801, 450801, 350801, 120101, 0, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    if (classSlot == 0)
                    {
                        res.WriteInt32(EQC0[xx]);
                    }
                    if (classSlot == 1)
                    {
                        res.WriteInt32(EQC1[xx]);
                    }
                    if (classSlot == 2)
                    {
                        res.WriteInt32(EQC2[xx]);
                    }
                    if (classSlot == 3)
                    {
                        res.WriteInt32(EQC3[xx]);
                    }
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteInt32(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);//bool
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);
                    res.WriteByte(0);

                    xx++;
                }

                int[] SoltContainer = new int[] { 1, 2, 16, 8, 32, 64, 128, 4, 0, 0, 0, 0, 16, 128, 64, 32, 8, 4, 0, 0, 0, 0, 0, 0, 0 };

                for (int i = 0; i < 0x19; i++)
                {
                    res.WriteInt32(SoltContainer[xxx]);

                    xxx++;
                }

                res.WriteByte(0x19); // how many items to display

                classSlot++;
            }
        }

        private void wo_4E0970_Fighter(IBuffer res)
        {
            
            res.WriteInt32(0);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                                //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(8);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(30);//Class Bonus HP
            res.WriteInt32(0);//Class Bonus MP

            res.WriteInt16(2);//Class Bonus STR
            res.WriteInt16(2);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(0);//Class Bonux AGI
            res.WriteInt16(-1);;//Class Bonus INT
            res.WriteInt16(0);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(11101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(11201);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?

        }

        private void wo_4E0970_Thief(IBuffer res)
        {
            res.WriteInt32(1);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(12);//Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                              //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(8);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(10);//Class Bonus HP
            res.WriteInt32(0);//Class Bonus MP

            res.WriteInt16(1);//Class Bonus STR
            res.WriteInt16(0);//Class Bonus VIT
            res.WriteInt16(1);//Class Bonus DEX
            res.WriteInt16(1);//Class Bonux AGI
            res.WriteInt16(-1); ;//Class Bonus INT
            res.WriteInt16(-1);//Class Bonus PIE
            res.WriteInt16(1);//Class Bonus LUK

            res.WriteInt32(14101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(14302);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(14803);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }

        private void wo_4E0970_Mage(IBuffer res)
        {
            res.WriteInt32(2);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(14); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(8);//INT Requirement
            res.WriteInt16(0);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(0);//Class Bonus HP
            res.WriteInt32(50);//Class Bonus MP

            res.WriteInt16(-1);//Class Bonus STR
            res.WriteInt16(0);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(0);//Class Bonux AGI
            res.WriteInt16(+2); ;//Class Bonus INT
            res.WriteInt16(+1);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(13101);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(13404);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }

        private void wo_4E0970_Priest(IBuffer res)
        {
            
            res.WriteInt32(3);//Class ID Fighter = 0, Thief = 1, Mage = 2, Priest - 3
            res.WriteByte(10); //Alignment Requirements 0/1 = Fully Locked, 2/3 = Lawful, 4/5 = Neutral. 6/7 = Lawful/Neutral,
                               //8/9 = Chaotic, 10/11 = Lawful/Chaotic, 12/13 = Neutral Chaotic, 14/15 = Fully unlocked, 16+ = fully locked

            res.WriteInt16(0);//STR Requirement
            res.WriteInt16(0);//VIT Requirement
            res.WriteInt16(0);//DEX Requirement
            res.WriteInt16(0);//AGI Requirement
            res.WriteInt16(0);//INT Requirement
            res.WriteInt16(8);//PIE Requirement
            res.WriteInt16(0);//LUK Requirement

            res.WriteInt32(20);//Class Bonus HP
            res.WriteInt32(40);//Class Bonus MP

            res.WriteInt16(0);//Class Bonus STR
            res.WriteInt16(1);//Class Bonus VIT
            res.WriteInt16(0);//Class Bonus DEX
            res.WriteInt16(-1);//Class Bonux AGI
            res.WriteInt16(0); ;//Class Bonus INT
            res.WriteInt16(2);//Class Bonus PIE
            res.WriteInt16(0);//Class Bonus LUK

            res.WriteInt32(12501);// states what is in skill slot 0 (left most), 0 = nothing
            res.WriteInt32(12601);// states what is in skill slot 1 (middle), 0 = nothing              
            res.WriteInt32(0);// states what is in skill slot 2 (right most), 0 = nothing              

            res.WriteByte(0);//bonus roll number store?
        }


       
        
        
    }
}
