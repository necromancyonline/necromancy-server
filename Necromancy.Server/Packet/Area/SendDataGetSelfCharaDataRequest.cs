using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendDataGetSelfCharaDataRequest : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendDataGetSelfCharaDataRequest));
        private ItemInstance[] _equippedItems;

        public SendDataGetSelfCharaDataRequest(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_data_get_self_chara_data_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemService itemService = new ItemService(client.character);
            itemService.LoadEquipmentModels();
            client.soul.SetSoulAlignment();
            client.character.LoginCheckDead();
            client.character.AddStateBit(CharacterState.InvulnerableForm);
            _equippedItems = new ItemInstance[client.character.equippedItems.Count];
            client.character.equippedItems.Values.CopyTo(_equippedItems, 0);

            Attribute attribute = new Attribute();
            attribute.DefaultClassAtributes(client.character.raceId);
            client.character.Hp.SetMax(attribute.hp * client.character.level); //make better after HP calc exists

            SendDataGetSelfCharaData(client);

            IBuffer res2 = BufferProvider.Provide();
            router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_request_r, res2, ServerType.Area);
        }


        private void SendDataGetSelfCharaData(NecClient client)
        {
            int numEntries = _equippedItems.Length; //Max of 25 Equipment Slots for Character Player. must be 0x19 or less
            int numStatusEffects = client.character.statusEffects.Length; /*_character.Statuses.Length*/ //0x80; //Statuses effects. Max 128
            int i = 0;
            if (client.character.state.HasFlag(CharacterState.SoulForm)) numEntries = 0; //Dead mean wear no gear

            IBuffer res = BufferProvider.Provide();
            //sub_4953B0 - characteristics
            //Consolidated Frequently Used Code
            //LoadEquip.BasicTraits(res, character);
            res.WriteUInt32(client.character.raceId); //race
            res.WriteUInt32(client.character.sexId);
            res.WriteByte(client.character.hairId); //hair
            res.WriteByte(client.character.hairColorId); //color
            res.WriteByte(client.character.faceId); //face
            res.WriteByte(client.character.faceArrangeId); //FaceArrange
            res.WriteByte(client.character.voiceId); //Voice
            for (int j = 0; j < 100; j++)
                res.WriteInt64(0);

            //sub_484720 - combat/leveling info
            _Logger.Debug($"Character ID Loading : {client.character.id}");
            res.WriteUInt32(client.character.instanceId); // InstanceId
            res.WriteInt32(client.character.activeModel); //Model
            res.WriteUInt32(client.character.classId); // class
            res.WriteInt16(client.character.level); // current level
            res.WriteUInt64(client.character.experienceCurrent); // current exp
            res.WriteUInt64(client.soul.experienceCurrent); // soul exp
            res.WriteInt64(100); // exp needed to level
            res.WriteInt64(200); // soul exp needed to level
            res.WriteInt32(client.character.Hp.current); // current hp
            res.WriteInt32(client.character.Mp.current); // current mp
            res.WriteInt32(client.character.Od.current); // current od
            res.WriteInt32(client.character.Hp.max); // max hp
            res.WriteInt32(client.character.Mp.max); // maxmp
            res.WriteInt32(client.character.Od.max); // max od
            res.WriteInt32(client.character.Gp.current); // current guard points
            res.WriteInt32(client.character.Gp.max); // max guard points
            res.WriteInt32(client.character.Weight.current); // value/100 = current weight
            res.WriteInt32(client.character.Weight.max); // value/100 = max weight
            res.WriteByte((byte)client.character.Condition.current); // condition

            // total stat level includes bonus'?
            res.WriteUInt16(client.character.strength); // str
            res.WriteUInt16(client.character.vitality); // vit
            res.WriteInt16((short)(client.character.dexterity + 3)); // dex
            res.WriteUInt16(client.character.agility); // agi
            res.WriteUInt16(client.character.intelligence); // int
            res.WriteUInt16(client.character.piety); // pie
            res.WriteInt16((short)(client.character.luck + 4)); // luk

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
            res.WriteUInt64(client.character.adventureBagGold); // gold
            res.WriteUInt32(client.soul.alignmentId); // AlignmentId
            res.WriteInt32(client.soul.pointsLawful); // lawful
            res.WriteInt32(client.soul.pointsNeutral); // neutral
            res.WriteInt32(client.soul.pointsChaos); // chaos
            res.WriteInt32(Util.GetRandomNumber(90400101, 90400130)); // title from honor.csv

            //sub_484980
            res.WriteInt32(10000); // SP Lawful accrual per tick?
            res.WriteInt32(20000); // SP Neutral accrual per tick?
            res.WriteInt32(30000); // SP Chaos accrual per tick?

            // characters stats
            res.WriteUInt16(client.character.strength); // str
            res.WriteUInt16(client.character.vitality); // vit
            res.WriteInt16((short)client.character.dexterity); // dex
            res.WriteUInt16(client.character.agility); // agi
            res.WriteUInt16(client.character.intelligence); // int
            res.WriteUInt16(client.character.piety); // pie
            res.WriteInt16((short)client.character.luck); // luk

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
            res.WriteInt32(client.character.mapId); //MapSerialID
            res.WriteInt32(client.character.mapId); //MapID ?floor
            res.WriteInt32(client.character.mapId); //MapID ?
            res.WriteByte(client.soul.criminalLevel); //new??
            res.WriteByte(1); //Beginner Protection (bool) ???
            res.WriteFixedString(settings.dataAreaIpAddress, 65); //IP
            res.WriteUInt16(settings.areaPort); //Port

            //sub_484420 // Map Spawn coord
            res.WriteFloat(client.character.x); //X Pos
            res.WriteFloat(client.character.y); //Y Pos
            res.WriteFloat(client.character.z); //Z Pos
            res.WriteByte(client.character.heading); //view offset

            //sub_read_int32 skill point
            res.WriteUInt32(client.character.skillPoints); // skill point

            res.WriteInt64((long)client.character.state); //Character State

            //sub_494AC0
            res.WriteByte(client.soul.level); // soul level
            res.WriteInt64(client.soul.pointsCurrent); // Current Soul Points
            res.WriteInt64(120); //new Max level?
            res.WriteInt64(120); // Max soul points
            res.WriteByte(client.soul.criminalLevel); // 0 is white,1 yellow 2 red 3+ skull
            res.WriteByte((byte)client.character.beginnerProtection); //Beginner protection (bool)
            res.WriteByte(255); // character Level cap?
            res.WriteByte(1);
            res.WriteByte(2);
            res.WriteByte(3);
            res.WriteByte(1); //new

            res.WriteInt32(1); //new
            res.WriteInt32(2); //new
            res.WriteInt32(3); //new
            res.WriteInt32(4); //new
            res.WriteInt32(5); //new

            res.WriteInt32(6); //new

            //sub_read_3-int16 unknown
            res.WriteInt16(client.character.hpRecoveryRate); // HP Recovery Rate for heals?
            res.WriteInt16(client.character.mpRecoveryRate); // MP Recovery Rate for heals?
            res.WriteInt16(client.character.odRecoveryRate); // OD Consumption Rate (if greater than currentOD, Can not sprint)

            //sub_4833D0
            res.WriteInt64(1234);

            //sub_4833D0
            res.WriteInt64(5678);

            //sub_4834A0
            res.WriteFixedString($"{client.soul.name} Shop", 97); //Shopname

            //sub_4834A0
            res.WriteFixedString($"{client.soul.name} Comment", 385); //Comment

            //sub_494890
            res.WriteByte(0); //Bool for showing/hiding character comment.

            //sub_4834A0
            res.WriteFixedString($"{client.soul.name} chatbox?", 385); //Chatbox?

            //sub_494890
            res.WriteByte(1); //Bool

            res.WriteInt32(0); //new
            res.WriteByte(0); //new


            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 1", 73); //new

            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 2", 73); //new

            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 3", 73); //new

            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 4", 73); //new

            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 5", 73); //new

            res.WriteInt64(5678); //new
            res.WriteInt32(1); //new
            res.WriteFixedString("unknown 6", 73); //new

            res.WriteInt32(0); //new //swirly effect?

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_483660
            for (i = 0; i < numEntries; i++) res.WriteInt32((int)_equippedItems[i].type);

            //sub_483420
            res.WriteInt32(numEntries); // Number of equipment Slots
            //sub_4948C0
            for (i = 0; i < numEntries; i++)
            {
                res.WriteInt32(_equippedItems[i].baseId); //Item Base Model ID
                res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                res.WriteByte(0); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                res.WriteByte(0); //??item version

                res.WriteInt32(_equippedItems[i].baseId); //testing (Theory, texture file related)
                res.WriteByte(0); //hair
                res.WriteByte(1); //color
                res.WriteByte(0); //face

                res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                res.WriteByte((byte)(client.character.faceId * 10)); //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
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
            for (i = 0; i < numEntries; i++) res.WriteInt32((int)_equippedItems[i].currentEquipSlot); //bitmask per equipment slot

            //sub_483420
            res.WriteInt32(numStatusEffects); //has to be less than 128
            //sub_485A70
            for (int k = 0; k < numStatusEffects; k++) //status buffs / debuffs
            {
                res.WriteInt32(i); //instanceID or unique ID
                res.WriteUInt32(client.character.statusEffects[k]); //Buff.SerialId from buff.csv
                res.WriteInt32(Util.GetRandomNumber(100, 6000)); //Time Remaining in seconds
                res.WriteInt32(1); //new
            }

            res.WriteByte(0); //new
            res.WriteByte(0); //new bool

            router.Send(client, (ushort)AreaPacketId.recv_data_get_self_chara_data_r, res, ServerType.Area);
        }
    }
}
