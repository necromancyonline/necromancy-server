using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyNpcData : PacketResponse
    {
        private readonly Character _character = new Character();
        private readonly NpcSpawn _npcSpawn;

        public RecvDataNotifyNpcData(NpcSpawn npcSpawn)
            : base((ushort)AreaPacketId.recv_data_notify_npc_data, ServerType.Area)
        {
            _npcSpawn = npcSpawn;
            _character.name = npcSpawn.title;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0; //max of 0x19;
            int numStatusEffects = 0; //max of 128;
            int i = 0;
            ItemInstance[] equippedItems = new ItemInstance[numEntries]; //ToDo Add NPC specific equipment here

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_npcSpawn.instanceId); // InstanceId
            res.WriteInt32(_npcSpawn.npcId); // NPC Serial ID from "npc.csv"
            res.WriteByte(0); // interaction type. 0:chat bubble. 1:none 2:press f
            res.WriteCString(_npcSpawn.name); //Name
            res.WriteCString(_npcSpawn.title); //Title
            res.WriteFloat(_npcSpawn.x); //X Pos
            res.WriteFloat(_npcSpawn.y); //Y Pos
            res.WriteFloat(_npcSpawn.z); //Z Pos
            res.WriteByte(_npcSpawn.heading); //view offset

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_483660
            for (i = 0; i < numEntries; i++) res.WriteInt32((int)equippedItems[i].type);

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
                res.WriteByte(1); //color
                res.WriteByte(0); //face

                res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte((byte)(_character.faceId * 10)); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
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
            for (i = 0; i < numEntries; i++) res.WriteInt32((int)equippedItems[i].currentEquipSlot); //bitmask per equipment slot

            res.WriteInt32(_npcSpawn.modelId); //NPC Model from file "model_common.csv"
            res.WriteInt16(100 /*_npcSpawn.Size*/); //NPC Model Size  //Hardcoded to 100
            res.WriteByte(4); //Hair ID for Character models
            res.WriteByte(5); //Hair Color ID for Character models
            res.WriteByte(3); //Face ID for Character models
            res.WriteInt32(0b10100110); //BITMASK for NPC State
            //0bxxxxxxx1 - 1 dead / 0 alive | for character models only
            //0bxxxxxx1x - 1 Soul form visible / 0 soul form invisible
            //0bxxxxx1xx -
            //0bxxxx1xxx - 1 Show Emoticon / 0 Hide Emoticon
            //0bxxx1xxxx -
            //0bxx1xxxxx -
            //0bx1xxxxxx - 1 blinking  / 0 solid
            //0b1xxxxxxx -
            res.WriteInt32(Util.GetRandomNumber(1, 9)); //npc Emoticon above head 1 for skull. 2-9 different hearts
            res.WriteInt32(_npcSpawn.status); //From  NPC.CSV column C  |   //horse: 4 TP machine:5 Ghost: 6 Illusion 7. Dungeun: 8 Stone 9. Ggate 1.  torch 13,14,15. power spot :22  event:23 ??:16,17,18
            res.WriteFloat(_npcSpawn.statusX); //x for particle effects from Int32 above From NPC.CSV column D
            res.WriteFloat(_npcSpawn.statusY); //y for particle effects from Int32 above From NPC.CSV column E
            res.WriteFloat(_npcSpawn.statusZ); //z for particle effects from Int32 above From NPC.CSV column F

            res.WriteInt32(numStatusEffects); //number of status effects. 128 Max.
            for (i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0); //new
            }

            return res;
        }
    }
}
