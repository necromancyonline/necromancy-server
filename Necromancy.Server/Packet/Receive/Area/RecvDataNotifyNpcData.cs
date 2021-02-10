using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyNpcData : PacketResponse
    {
        private NpcSpawn _npcSpawn;
        private Character _character = new Character();

        public RecvDataNotifyNpcData(NpcSpawn npcSpawn)
            : base((ushort) AreaPacketId.recv_data_notify_npc_data, ServerType.Area)
        {
            _npcSpawn = npcSpawn;
            _character.Name = npcSpawn.Title;
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x19; // 1 to 19 equipment.  
            int numStatusEffects = 128;
            int i = 0;

            if (_npcSpawn.ModelId > 52000 /*CharacterModelUpperLimit*/)
            {
                numEntries = 0;
            } //ToDo find NPCs using chara models and build a table containing their equipment IDs

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_npcSpawn.InstanceId); // InstanceId 
            res.WriteInt32(_npcSpawn.NpcId); // NPC Serial ID from "npc.csv"
            res.WriteByte(0); // interaction type. 0:chat bubble. 1:none 2:press f
            res.WriteCString(_npcSpawn.Name); //Name 
            res.WriteCString(_npcSpawn.Title); //Title
            res.WriteFloat(_npcSpawn.X); //X Pos
            res.WriteFloat(_npcSpawn.Y); //Y Pos
            res.WriteFloat(_npcSpawn.Z); //Z Pos
            res.WriteByte(_npcSpawn.Heading); //view offset

            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
            while (i < numEntries)
            {
                //sub_483660   
                res.WriteInt32(0); //Must have 25 on recv_data_notify_npc_data
                i++;
            }
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
            while (i < numEntries)//Must have 25 on recv_chara_notify_data
            {
                res.WriteInt32(0); //Sets your Item ID per Iteration
                res.WriteByte(0); // 
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteInt32(0); //testing (Theory, Icon related)
                res.WriteByte(0); //
                res.WriteByte(0); // (theory bag)
                res.WriteByte(0); // (theory Slot)

                res.WriteByte(0); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte(00); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(0); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model 

                res.WriteByte(0); // separate in assembly
                res.WriteByte(0); // separate in assembly
                i++;
            }
            res.WriteInt32(numEntries); // Number of equipment Slots
            //Consolidated Frequently Used Code
            i = 0;
            while (i < numEntries)
            {
                //sub_483420   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                i++;
            }

            res.WriteInt32(_npcSpawn.ModelId); //NPC Model from file "model_common.csv"
            res.WriteInt16(100/*_npcSpawn.Size*/); //NPC Model Size  //Hardcoded to 100
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
            res.WriteInt32(_npcSpawn
                .Status); //From  NPC.CSV column C  |   //horse: 4 TP machine:5 Ghost: 6 Illusion 7. Dungeun: 8 Stone 9. Ggate 1.  torch 13,14,15. power spot :22  event:23 ??:16,17,18
            res.WriteFloat(_npcSpawn.Status_X); //x for particle effects from Int32 above From NPC.CSV column D
            res.WriteFloat(_npcSpawn.Status_Y); //y for particle effects from Int32 above From NPC.CSV column E
            res.WriteFloat(_npcSpawn.Status_Z); //z for particle effects from Int32 above From NPC.CSV column F
            res.WriteInt32(numStatusEffects); //number of status effects. 128 Max.
            for (i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt32(0);//new
            }

            return res;
        }
    }
}
