using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class send_data_get_self_chara_data_request : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_data_get_self_chara_data_request));
        private ItemInstance[] _equippedItems;

        public send_data_get_self_chara_data_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemService itemService = new ItemService(client.Character);
            itemService.LoadEquipmentModels();
            client.Character.AddStateBit(Model.CharacterModel.CharacterState.InvulnerableForm);
            client.Soul.SetSoulAlignment();
            _equippedItems = new ItemInstance[client.Character.EquippedItems.Count];
            client.Character.EquippedItems.Values.CopyTo(_equippedItems, 0);

            SendDataGetSelfCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2, ServerType.Area);
        }


        private void SendDataGetSelfCharaData(NecClient client)
        {
            int numEntries = _equippedItems.Length; //Max of 25 Equipment Slots for Character Player. must be 0x19 or less
            int numStatusEffects = 0; /*_character.Statuses.Length*/ //0x80; //Statuses effects. Max 128
            int i = 0;
            if (client.Character.State == Model.CharacterModel.CharacterState.SoulForm) numEntries = 0; //Dead mean wear no gear

            IBuffer res = BufferProvider.Provide();
            //sub_4953B0 - characteristics
            //Consolidated Frequently Used Code
            //LoadEquip.BasicTraits(res, character);
            res.WriteUInt32(client.Character.RaceId); //race
            res.WriteUInt32(client.Character.SexId);
            res.WriteByte(client.Character.HairId); //hair
            res.WriteByte(client.Character.HairColorId); //color
            res.WriteByte(client.Character.FaceId); //face
            res.WriteByte(client.Character.FaceArrangeId);//FaceArrange
            res.WriteByte(client.Character.VoiceId);//Voice
            for (int j = 0; j < 100; j++)
                res.WriteInt64(0);

            //sub_484720 - combat/leveling info
            Logger.Debug($"Character ID Loading : {client.Character.Id}");
            res.WriteUInt32(client.Character.InstanceId); // InstanceId
            res.WriteInt32(client.Character.activeModel);//Model
            res.WriteUInt32(client.Character.ClassId); // class
            res.WriteInt16(client.Character.Level); // current level 
            res.WriteUInt64(client.Character.ExperienceCurrent); // current exp
            res.WriteUInt64(client.Soul.ExperienceCurrent); // soul exp
            res.WriteInt64(100); // exp needed to level
            res.WriteInt64(200); // soul exp needed to level
            res.WriteInt32(client.Character.Hp.current); // current hp
            res.WriteInt32(client.Character.Mp.current); // current mp
            res.WriteInt32(client.Character.Od.current); // current od
            res.WriteInt32(client.Character.Hp.max); // max hp
            res.WriteInt32(client.Character.Mp.max); // maxmp
            res.WriteInt32(client.Character.Od.max); // max od
            res.WriteInt32(client.Character.Gp.current); // current guard points
            res.WriteInt32(client.Character.Gp.max); // max guard points
            res.WriteInt32(client.Character.Weight.current); // value/100 = current weight
            res.WriteInt32(client.Character.Weight.max); // value/100 = max weight
            res.WriteByte((byte)client.Character.Condition.current); // condition

            // total stat level includes bonus'?
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.Vitality); // vit
            res.WriteInt16((short)(client.Character.Dexterity + 3)); // dex
            res.WriteUInt16(client.Character.Agility); // agi
            res.WriteUInt16(client.Character.Intelligence); // int
            res.WriteUInt16(client.Character.Piety); // pie
            res.WriteInt16((short)(client.Character.Luck + 4)); // luk

            // mag atk atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            // mag def atrb
            res.WriteInt16(5); // fire
            res.WriteInt16(52); // water
            res.WriteInt16(58); // wind
            res.WriteInt16(45); // earth
            res.WriteInt16(33); // light
            res.WriteInt16(12); // dark
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);

            //status change resistance
            res.WriteInt16(11); // Poison
            res.WriteInt16(12); // Paralyze
            res.WriteInt16(13); // Stone
            res.WriteInt16(14); // Faint
            res.WriteInt16(15); // Blind
            res.WriteInt16(16); // Sleep
            res.WriteInt16(17); // Silent
            res.WriteInt16(18); // Charm
            res.WriteInt16(19); // confus
            res.WriteInt16(20); // fear
            res.WriteInt16(21); //possibly EXP Boost Gauge. trying to find it

            // gold and alignment?
            res.WriteUInt64(client.Character.AdventureBagGold); // gold
            res.WriteUInt32(client.Soul.AlignmentId); // AlignmentId
            res.WriteInt32(client.Soul.PointsLawful); // lawful
            res.WriteInt32(client.Soul.PointsNeutral); // neutral
            res.WriteInt32(client.Soul.PointsChaos); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(10000); // SP Lawful accrual per tick?
            res.WriteInt32(20000); // SP Neutral accrual per tick?
            res.WriteInt32(30000); // SP Chaos accrual per tick?

            // characters stats
            res.WriteUInt16(client.Character.Strength); // str
            res.WriteUInt16(client.Character.Vitality); // vit
            res.WriteInt16((short)(client.Character.Dexterity)); // dex
            res.WriteUInt16(client.Character.Agility); // agi
            res.WriteUInt16(client.Character.Intelligence); // int
            res.WriteUInt16(client.Character.Piety); // pie
            res.WriteInt16((short)(client.Character.Luck)); // luk

            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);


            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);

            // nothing
            res.WriteInt16(1);
            res.WriteInt16(2);
            res.WriteInt16(3);
            res.WriteInt16(4);
            res.WriteInt16(5);
            res.WriteInt16(6);
            res.WriteInt16(7);
            res.WriteInt16(8);
            res.WriteInt16(9);
            res.WriteInt16(10);
            res.WriteInt16(11);


            //sub_484B00 map ip and connection
            res.WriteInt32(client.Character.MapId); //MapSerialID
            res.WriteInt32(client.Character.MapId); //MapID ?floor
            res.WriteInt32(client.Character.MapId); //MapID ?
            res.WriteByte((byte)(client.Character.criminalState + 5));//new??
            res.WriteByte(1); //Beginner Protection (bool) ???
            res.WriteFixedString(Settings.DataAreaIpAddress, 65); //IP
            res.WriteUInt16(Settings.AreaPort); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.Character.X); //X Pos
            res.WriteFloat(client.Character.Y); //Y Pos
            res.WriteFloat(client.Character.Z); //Z Pos
            res.WriteByte(client.Character.Heading); //view offset

            //sub_read_int32 skill point
            res.WriteUInt32(client.Character.SkillPoints); // skill point

            res.WriteInt64((long)client.Character.State);//Character State

            //sub_494AC0
            res.WriteByte(client.Soul.Level); // soul level
            res.WriteInt64(client.Soul.PointsCurrent);// Current Soul Points
            res.WriteInt64(120);//new Max level?
            res.WriteInt64(120);// Max soul points
            res.WriteByte(client.Soul.CriminalLevel); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte((byte)client.Character.beginnerProtection); //Beginner protection (bool)
            res.WriteByte(255); // character Level cap?
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(3);
            res.WriteByte(1);//new

            res.WriteInt32(1);//new
            res.WriteInt32(2);//new
            res.WriteInt32(3);//new
            res.WriteInt32(4);//new
            res.WriteInt32(5);//new

            res.WriteInt32(6);//new

            //sub_read_3-int16 unknown
            res.WriteInt16(client.Character.HpRecoveryRate); // HP Recovery Rate for heals?
            res.WriteInt16(client.Character.MpRecoveryRate); // MP Recovery Rate for heals?
            res.WriteInt16(client.Character.OdRecoveryRate); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(1234);

            //sub_4833D0
            res.WriteInt64(5678);

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(0); //Bool for showing/hiding character comment.

            //sub_4834A0
            res.WriteFixedString($"{client.Soul.Name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            res.WriteInt32(0);//new
            res.WriteByte(0);//new


            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 1", 73); //new

            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 2", 73); //new

            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 3", 73); //new

            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 4", 73); //new

            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 5", 73); //new

            res.WriteInt64(5678);//new
            res.WriteInt32(1);//new
            res.WriteFixedString($"unknown 6", 73); //new

            res.WriteInt32(0);//new //swirly effect?

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
                res.WriteByte((byte)(client.Character.FaceId * 10));  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
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

            //sub_483420
            res.WriteInt32(numStatusEffects); //has to be less than 128
            //sub_485A70
            for (int k = 0; k < numStatusEffects; k++) //status buffs / debuffs
            {
                res.WriteInt32(0); //instanceID or unique ID
                res.WriteInt32(0); //Buff.SerialId
                res.WriteInt32(0); //Buff.EffectId
                res.WriteInt32(9999999); //new
            }

            res.WriteByte(0);//new
            res.WriteByte(0);//new bool

            Router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }

    }
}
