using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyMonsterData : PacketResponse
    {
        private MonsterSpawn _monsterSpawn;

        public RecvDataNotifyMonsterData(MonsterSpawn monsterSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_monster_data, ServerType.Area)
        {
            _monsterSpawn = monsterSpawn;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0;// 16; //Max of 16 Equipment Slots for Monster.  cmp to 0x10
            int numStatusEffects = 0;// 0x80; //Statuses effects. Max 128
            int i = 0;
            ItemInstance[] equippedItems = new ItemInstance[numEntries]; //ToDo Add NPC specific equipment here


            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_monsterSpawn.instanceId);
            res.WriteCString(_monsterSpawn.name);
            res.WriteCString(_monsterSpawn.title);
            res.WriteFloat(_monsterSpawn.x);
            res.WriteFloat(_monsterSpawn.y);
            res.WriteFloat(_monsterSpawn.z);
            res.WriteByte(_monsterSpawn.heading);
            res.WriteInt32(_monsterSpawn.monsterId); //Monster Serial ID
            res.WriteInt32(_monsterSpawn.modelId); //Monster Model ID
            res.WriteInt16(_monsterSpawn.size);

            res.WriteByte(0);//new
            res.WriteByte(0);//new
            res.WriteByte(0);//new

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_483660
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32((int)equippedItems[i].type);
            }

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_4948C0
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32(equippedItems[i].baseId); //Item Base Model ID
                res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                res.WriteByte(0); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                res.WriteByte(0); //??item version

                res.WriteInt32(equippedItems[i].baseId); //testing (Theory, texture file related)
                res.WriteByte(0); //hair
                res.WriteByte(0); //color
                res.WriteByte(0); //face

                res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00);  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(00); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink

                res.WriteByte(0); // separate in assembly
                res.WriteByte(0); // separate in assembly
            }
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots to display
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32((int)equippedItems[i].currentEquipSlot); //bitmask per equipment slot
            }

            res.WriteInt32(0b00000000); //BITMASK for Monster State
            //0bxxxxxxx1 - 1 Dead / 0 Alive  |
            //0bxxxxxx1x - 1 crouching / 0 standing
            //0bxxxxx1xx -
            //0bxxxx1xxx - 1 crouching / 0 standing
            //0bxxx1xxxx -
            //0bxx1xxxxx -
            //0bx1xxxxxx - 1 Aggro Battle  / 0 Normal    | (for when you join a map and the monster is in battle)
            //0b1xxxxxxx -
            res.WriteInt64(0); //
            res.WriteInt64(0); //
            res.WriteInt64(0); //
            res.WriteByte(231);
            res.WriteByte(232);
            res.WriteByte(0);//new
            res.WriteInt32(_monsterSpawn.hp.current); //Current HP
            res.WriteInt32(_monsterSpawn.hp.max); //Max HP
            res.WriteInt32(numStatusEffects); // cmp to 0x80 = 128
            for (i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0); // status effect ID. set to i
                res.WriteInt32(0); //1 on 0 off
                res.WriteInt32(0);
                res.WriteInt32(0);//new
            }

            return res;
        }
    }
}
