using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System;

/// <summary>
/// This receive loads your Character on the map.  dead or alive.
/// 
/// for spirit stuff go to the CharaBodyData recv.
/// </summary>
namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDataNotifyCharaData : PacketResponse
    {
        private readonly Character _character;
        private readonly string _soulName;

        public RecvDataNotifyCharaData(Character character, string soulName)
            : base((ushort) AreaPacketId.recv_data_notify_chara_data, ServerType.Area)
        {
            _character = character;
            _soulName = soulName;
        }

        protected override IBuffer ToBuffer()
        {
            TimeSpan differenceJoined = DateTime.Today.ToUniversalTime() - DateTime.UnixEpoch;
            int DateAttackedCalculation = (int)Math.Floor(differenceJoined.TotalSeconds);
            int numEntries = 0x19; //Max of 25 Equipment Slots for Character Player
            int numStatusEffects = 0x80; //Statuses effects. Max 128
            int i = 0;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_character.InstanceId);
            res.WriteCString(_soulName);
            res.WriteCString(_character.Name);
            res.WriteFloat(_character.X);
            res.WriteFloat(_character.Y);
            res.WriteFloat(_character.Z);
            res.WriteByte(_character.Heading);
            res.WriteInt32(_character.activeModel);//_character.Level); Character.ActiveModel  0 = default
            res.WriteInt16(_character.modelScale); //Character.Scale   100 = normal size.
            res.WriteInt64((uint)_character.State); //find out where soul state is getting set.. and un-set it appropriately
            res.WriteInt16(0/*_character.deadType*/); //??  Soul State?  Soul Form? turns you soul form if above 0   : ToDo. key to death and revival

            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
            //sub_483660 
            foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
            {
                res.WriteInt32((int)itemInstance.Type);
                //Logger.Debug($"Loading {i}:{itemInstance.Type} | {itemInstance.UnidentifiedName}");
                i++;
            }
            while (i < numEntries)
            {
                //sub_483660   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                //Logger.Debug($"Loading {i}: blank");
                i++;
            }
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
            foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
            {
                res.WriteInt32(itemInstance.BaseID); //Item Base Model ID
                res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                res.WriteByte(0); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                res.WriteByte(0); //??item version

                res.WriteInt32(itemInstance.BaseID); //testing (Theory, texture file related)
                res.WriteByte(0); //hair
                res.WriteByte(1); //color
                res.WriteByte(0); //face

                res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte((byte)(_character.FaceId * 10));  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                res.WriteByte(00); // testing (Theory Torso Tex)
                res.WriteByte(0); // testing (Theory Pants Tex)
                res.WriteByte(0); // testing (Theory Hands Tex)
                res.WriteByte(0); // testing (Theory Feet Tex)
                res.WriteByte(0); //Alternate texture for item model  0 normal : 1 Pink 

                res.WriteByte(0); // separate in assembly
                res.WriteByte(0); // separate in assembly
                i++;
            }
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
            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            i = 0;
            //Messy SoulState Equipment removal  hasdied check
            if (_character.HasDied == false)
            {
                foreach (ItemInstance itemInstance in _character.EquippedItems.Values)
                {
                    res.WriteInt32((int)itemInstance.CurrentEquipSlot); //bitmask per equipment slot
                    i++;
                }
            }
            while (i < numEntries)
            {
                //sub_483420   
                res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                i++;
            }
            



            //sub_4835C0
            res.WriteInt32(_character.charaPose); //1 here means crouching?
            //sub_484660
            res.WriteUInt32(_character.RaceId); //race
            res.WriteUInt32(_character.SexId);
            res.WriteByte(_character.HairId); //hair
            res.WriteByte(_character.HairColorId); //color
            res.WriteByte(_character.FaceId); //face
            res.WriteByte(0);//voice?
            res.WriteByte(0);//skinTone?
            //weird 64 loop
            for (i = 0; i < 100; i++)
            { res.WriteInt64(0); }

            //sub_483420
            res.WriteUInt32(_character.partyId); // party id?
            //sub_4837C0
            res.WriteUInt32(0); // party id? // i don't think sooo'
            //sub_read_byte
            res.WriteByte(_character.criminalState); //Criminal name icon
            //sub_494890
            res.WriteByte((byte) _character.beginnerProtection); //Bool Beginner Protection
            //sub_4835E0
            res.WriteInt32(_character.movementPose); //pose, 1 = sitting, 0 = standing
            //sub_483920
            res.WriteInt32(0); //???
            //sub_491A00
            res.WriteByte(0); //newjp
            //sub_483440
            res.WriteInt16(_character.Level); //Player level (stat gui)
            //sub_read_byte
            res.WriteByte(0); //no change?   MemberShip Status?
            //sub_read_byte
            res.WriteByte(0); //no change?
            //sub_read_int_32
            res.WriteInt32(90400101); //Title from Honor.csv   _character.Title
            //sub_483580
            res.WriteUInt32(_character.ClassId); //Signifies character class
            //sub_483420
            res.WriteInt32(numStatusEffects); //Number of Status Effects to display 128 Max
            //sub_485A70
            for (i = 0; i < numStatusEffects; i++)
            {
                res.WriteInt32(0); //instanceID or unique ID
                res.WriteInt32(0); //Buff.SerialId
                res.WriteInt32(0); //Buff.EffectId
                res.WriteInt32(9999999); //new
            }

            //sub_481AA0
            res.WriteCString("" /*_character.comment*/); //Comment string
            res.WriteInt32(0);
            return res;
        }
    }
}
