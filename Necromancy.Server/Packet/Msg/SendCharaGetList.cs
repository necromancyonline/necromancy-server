using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Msg
{
    public class SendCharaGetList : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharaGetList));
        public SendCharaGetList(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_chara_get_list;


        public override void Handle(NecClient client, NecPacket packet)
        {
            List<Character> characters = database.SelectCharactersBySoulId(client.soul.id);
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(characters.Count); //expected character count per soul
            router.Send(client, (ushort)MsgPacketId.recv_chara_get_list_r, res, ServerType.Msg);

            SendNotifyData(client);
            SendNotifyDataComplete(client);
        }

        private void SendNotifyDataComplete(NecClient client)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteByte(0); //bool Result
            res2.WriteUInt32(0); //Wanted_dead_CharaId - for when a char got sent to prison.  Slot of character in prison
            res2.WriteInt64(0b1111111111111111); //Soul Premium Flags
            res2.WriteByte(client.soul.criminalLevel); //Crime Level.
            res2.WriteUInt32(0); //Soul State
            router.Send(client, (ushort)MsgPacketId.recv_chara_notify_data_complete, res2, ServerType.Msg);
        }

        private void SendNotifyData(NecClient client)
        {
            ///////
            //// this one is not like the others.  It actually requires 25 entries to fire.
            ///////
            List<Character> characters = database.SelectCharactersBySoulId(client.soul.id);

            foreach (Character character in characters)
            {
                ItemService itemService = new ItemService(character);
                itemService.LoadEquipmentModels();
                character.LoginCheckDead();

                IBuffer res = BufferProvider.Provide();

                res.WriteByte(character.slot); //character slot, 0 for left, 1 for middle, 2 for right
                res.WriteInt32(character.id); //  Character ID
                res.WriteFixedString(character.name, 91); // 0x5B | 91x 1 byte

                res.WriteInt32(character.deadType); // 0 = Alive | 1,2,3, = Dead 4 = ash, 5 = lost
                res.WriteInt32(character.level); //character level stat
                res.WriteInt32(0); //todo (unknown)
                res.WriteUInt32(character.classId); //class stat

                res.WriteUInt32(character.raceId); //race
                res.WriteUInt32(character.sexId);
                res.WriteByte(character.hairId); //hair
                res.WriteByte(character.hairColorId); //color
                res.WriteByte(character.faceId); //face
                res.WriteByte(character.faceArrangeId);//Voice?
                res.WriteByte(character.voiceId);//skinTone?


                // cb eax 19.  Has to be 25 values.
                int numEntries = 0x19;
                int i = 0;
                //sub_483660
                foreach (ItemInstance itemInstance in character.equippedItems.Values)
                {
                    res.WriteInt32((int)itemInstance.type);
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

                i = 0;
                //sub_4948C0
                foreach (ItemInstance itemInstance in character.equippedItems.Values)
                {
                    res.WriteInt32(itemInstance.baseId); //Item Base Model ID
                    res.WriteByte(00); //? TYPE data/chara/##/ 00 is character model, 01 is npc, 02 is monster
                    res.WriteByte((byte)(character.raceId * 10 + character.sexId)); //Race and gender tens place is race 1= human, 2= elf 3=dwarf 4=gnome 5=porkul, ones is gender 1 = male 2 = female
                    res.WriteByte(16); //??item version

                    res.WriteInt32(itemInstance.baseId); //testing (Theory, texture file related)
                    res.WriteByte(0); //hair
                    res.WriteByte(1); //color
                    res.WriteByte(0); //face

                    res.WriteByte(45); // Hair style from  chara\00\041\000\model  45 = this file C:\WO\Chara\chara\00\041\000\model\CM_00_041_11_045.nif
                    res.WriteByte((byte)(character.faceId*10));  //Face Style calls C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data\chara\00\041\000\model\CM_00_041_10_010.nif.  must be 00 10, 20, 30, or 40 to work.
                    res.WriteByte(00); // testing (Theory Torso Tex)
                    res.WriteByte(0); // testing (Theory Pants Tex)
                    res.WriteByte(0); // testing (Theory Hands Tex)
                    res.WriteByte(0); // testing (Theory Feet Tex)
                    res.WriteByte(1); //Alternate texture for item model  0 normal : 1 Pink

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

                i = 0;
                //sub_483420
                foreach (ItemInstance itemInstance in character.equippedItems.Values)
                {
                    res.WriteInt32((int)itemInstance.currentEquipSlot); //bitmask per equipment slot
                    i++;
                }
                while (i < numEntries)
                {
                    //sub_483420
                    res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                    i++;
                }

                i = 0;
                foreach (ItemInstance itemInstance in character.equippedItems.Values)
                {
                    res.WriteInt32(itemInstance.enhancementLevel); ///item quality(+#) or aura? 10 = +7, 19 = +6,(maybe just wep aura)
                    i++;
                }
                while (i < numEntries)
                {
                    //sub_483420
                    res.WriteInt32(0); //Must have 25 on recv_chara_notify_data
                    i++;
                }

                for (i = 0; i < 0x19; i++)
                    res.WriteByte(1); //display item in slot.  1 yes 0 no


                //res.WriteByte((byte)character.Inventory._equippedItems.Count);
                res.WriteByte((byte)character.equippedItems.Count); //count your equipment here

                res.WriteInt32(character.mapId); //Map your character is on
                res.WriteInt32(Util.GetRandomNumber(0,3));//??? probably map area related

                res.WriteByte(0);
                res.WriteByte(0); //Character Name change in Progress.  (0 no : 1 Yes ).  Red indicator on top right

                res.WriteFixedString(character.name, 91); // 0x5B | 91x 1 byte
                router.Send(client, (ushort)MsgPacketId.recv_chara_notify_data, res, ServerType.Msg);
            }
        }
    }
}
