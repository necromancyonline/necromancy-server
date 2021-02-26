using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

/// <summary>
/// This receive loads your spirit on the map.  all things spirit related go here.  
/// 
/// for living stuff go to the CharaData recv.
/// </summary>
namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyCharaBodyData : PacketResponse
    {
        private readonly DeadBody _deadBody;
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(RecvDataNotifyCharaBodyData));
        private ItemInstance[] _equippedItems;


        public RecvDataNotifyCharaBodyData(DeadBody deadBody) 
            : base((ushort) AreaPacketId.recv_data_notify_charabody_data, ServerType.Area)
        {
            _deadBody = deadBody;
            foreach (ItemInstance itemInstance in _deadBody.EquippedItems.Values)
            {
                if (itemInstance.CurrentEquipSlot == ItemEquipSlots.Talkring) _deadBody.EquippedItems.Remove(itemInstance.CurrentEquipSlot); //Skip rendering talk rings.
            }
            _equippedItems = new ItemInstance[_deadBody.EquippedItems.Count];
            _deadBody.EquippedItems.Values.CopyTo(_equippedItems, 0);
        }

        protected override IBuffer ToBuffer()
        {
            int i = 0;
            int numEntries = _equippedItems.Length; //Max of 25 Equipment Slots for Character Player. must be 0x19 or less

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_deadBody.InstanceId); //Instance ID of dead body
            res.WriteUInt32(_deadBody.CharacterInstanceId); //Reference to actual player's instance ID
            res.WriteCString($"{_deadBody.SoulName}"); // Soul name 
            res.WriteCString($"{_deadBody.CharaName}"); // Character name
            res.WriteFloat(_deadBody.X); // X
            res.WriteFloat(_deadBody.Y); // Y
            res.WriteFloat(_deadBody.Z); // Z
            res.WriteByte(_deadBody.Heading); // Heading
            res.WriteInt32(_deadBody.Level);

            res.WriteInt16(100); //model scale.  set to 100.

            //This is actually rendering gear on the character model laying dead on the ground.
            //sub_483420 
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_483660 
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32((int)_equippedItems[i].Type);
            }

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_4948C0
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32(_equippedItems[i].BaseID); //Item Base Model ID
                res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                res.WriteByte(0); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                res.WriteByte(0); //??item version

                res.WriteInt32(_equippedItems[i].BaseID); //testing (Theory, texture file related)
                res.WriteByte(0); //hair
                res.WriteByte(1); //color
                res.WriteByte(0); //face

                res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte((byte)(_deadBody.FaceId * 10));  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
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
                res.WriteInt32((int)_equippedItems[i].CurrentEquipSlot); //bitmask per equipment slot
            }

            //Traits
            res.WriteUInt32(_deadBody.RaceId); //race
            res.WriteUInt32(_deadBody.SexId);
            
            res.WriteByte(_deadBody.HairId); //hair
            res.WriteByte(_deadBody.HairColorId); //color
            res.WriteByte(_deadBody.FaceId); //face
            res.WriteByte(_deadBody.FaceArrangeId); //face style
            res.WriteByte(_deadBody.VoiceId); //voice

            res.WriteInt32(_deadBody.ConnectionState); // 0 = bag, 1 for dead? (Can't enter soul form if this isn't 0 or 1 i think).
            res.WriteInt32(_deadBody.ModelType); //4 = ash pile, not sure what this is.
            res.WriteUInt32(0);// _deadBody.ClassId);  //StateFlag???
            res.WriteInt32(_deadBody.deathPose); //%DeadState :death pose 0 = faced down, 1 = head chopped off, 2 = no arm, 3 = faced down, 4 = chopped in half, 5 = faced down, 6 = faced down, 7 and up "T-pose" the body (ONLY SEND 1 IF YOU ARE CALLING THIS FOR THE FIRST TIME)
            res.WriteByte((byte)_deadBody.CriminalStatus); //crim status (changes icon on the end also), 0 = white, 1 = yellow, 2 = red, 3 = red with crim icon, 
            res.WriteByte((byte)_deadBody.BeginnerProtection); // (bool) Beginner protection
            res.WriteInt32(600); //%deadNextTime :Time until 'ash' . 600 = 00:10  //ToDo,  add a if(Character.State==CharacterState.Soulstate) task.Delay(600){UpdateSoulState(SoulState.Ash)}

            return res;
        }
    }
}
