using System.Collections.Generic;
using Necromancy.Server.Model;
using Necromancy.Server.Common;
using Necromancy.Server.Packet.Id;
using System.Threading;
using System;
using Necromancy.Server.Common.Instance;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick mob test commands.
    /// </summary>
    public class CharaCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(CharaCommand));

        public CharaCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }

            Character character2 = null;
            if (uint.TryParse(command[1], out uint x))
            {
                IInstance instance = Server.Instances.GetInstance(x);
                if (instance is Character character)
                {
                    character2 = character;
                }
                else if (instance is DeadBody deadbody)
                {
                    responses.Add(ChatResponse.CommandError(client, $"That's a dead body man. have some respect!"));
                    character2 = new Character();
                    character2.InstanceId = deadbody.InstanceId;
                }
                else
                {
                    responses.Add(ChatResponse.CommandError(client, $"Please provide a character instance id"));
                    return;
                }
            }

            if (!int.TryParse(command[2], out int y))
            {
                try
                {
                    string binaryString = command[2];
                    binaryString = binaryString.Replace("0b", "");
                    Logger.Debug(binaryString);
                    y = Convert.ToInt32(binaryString, 2);
                }
                catch
                {
                    responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
                    return;
                }
            }


            switch (command[0])
            {
                case "hp":
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(y);
                    character2.Hp.setCurrent(y);
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res, ServerType.Area);
                    break;

                case "dead":
                    SendBattleReportStartNotify(client, character2);
                    //recv_battle_report_noact_notify_dead = 0xCDC9,
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteUInt32(character2.InstanceId);
                    res2.WriteInt32(2); // death type? 1 = death, 2 = death and message, 3 = unconscious, beyond that = nothing
                    res2.WriteInt32(2);
                    res2.WriteInt32(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_noact_notify_dead, res2,
                        ServerType.Area);
                    SendBattleReportEndNotify(client, character2);
                    break;


                case "pose2":
                    IBuffer res4 = BufferProvider.Provide();
                    res4.WriteUInt32(character2.InstanceId); //Character ID
                    res4.WriteInt32(y); //Character pose
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_pose_notify, res4, ServerType.Area);
                    break;

                case "emotion":
                    //recv_emotion_notify_type = 0xF95B,
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteUInt32(character2.InstanceId);
                    res5.WriteInt32(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_emotion_notify_type, res5, ServerType.Area);
                    break;

                case "deadstate":
                    //recv_charabody_notify_deadstate = 0xCC36, // Parent = 0xCB94 // Range ID = 03
                    IBuffer res6 = BufferProvider.Provide();
                    res6.WriteUInt32(character2.DeadBodyInstanceId);
                    res6.WriteInt32(5); //4 changes body to ash pile, 5 causes a mist to happen and disappear
                    res6.WriteInt32(y); // change type. unknown impact.
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_deadstate, res6,
                        ServerType.Area);
                    break;

                case "start":
                    SendBattleReportStartNotify(client, character2);
                    IBuffer res7 = BufferProvider.Provide();
                    //recv_battle_report_action_item_enchant = 0x6BDC,
                    res7.WriteInt32(517);
                    res7.WriteInt32(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_action_item_enchant, res7,
                        ServerType.Area);
                    SendBattleReportEndNotify(client, character2);
                    break;

                case "end":
                    IBuffer res8 = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_end_notify, res8, ServerType.Area);
                    break;

                case "bodystate": ///State of your dead body
                    //recv_charabody_state_update_notify = 0x1A0F, 
                    IBuffer res10 = BufferProvider.Provide();
                    res10.WriteUInt32(character2.DeadBodyInstanceId); 
                    res10.WriteInt32(y); //0b0 = disconnected backpack, 0b1 = normal, 0b100 = title:On 0b10000=invisible. nothing above ob1 << 5
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_state_update_notify, res10, ServerType.Area);
                    responses.Add(ChatResponse.CommandError(client, $"setting bodyState to {y} for character {character2.Name}"));
                    break;

                case "charastate": //state of your regular body
                    //recv_chara_notify_stateflag = 0x23D3, 
                    IBuffer res11 = BufferProvider.Provide();
                    res11.WriteUInt32(character2.InstanceId);
                    res11.WriteInt64(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_stateflag, res11, ServerType.Area);
                    responses.Add(ChatResponse.CommandError(client,
                        $"setting charaState to {y} for character {character2.Name}"));
                    break;

                case "spirit":
                    //recv_charabody_notify_spirit = 0x36A6, //causes charater model to dissappear, but not title. for body collection??
                    IBuffer res12 = BufferProvider.Provide();
                    res12.WriteUInt32(character2.InstanceId);
                    res12.WriteByte((byte) y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_spirit, res12, ServerType.Area);
                    break;

                case "abyss": //lil marker in soul form of where you died if you jump off the map
                    //recv_charabody_self_notify_abyss_stead_pos = 0x679B,
                    IBuffer res13 = BufferProvider.Provide();
                    res13.WriteFloat(character2.X);
                    res13.WriteFloat(character2.Y);
                    res13.WriteFloat(character2.Z);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_self_notify_abyss_stead_pos, res13,
                        ServerType.Area);
                    break;

                case "charadata":
                    RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(character2,
                        client.Soul.Name /*Should be client of supplied instanceID. this is a band-aid*/);
                    Router.Send(Server.Clients.GetAll(), cData.ToPacket());
                    break;

                case "charabodydata":
                    //recv_data_notify_charabody_data = 0x906A,
                    DeadBody _deadBody = Server.Instances.GetInstance(character2.DeadBodyInstanceId) as DeadBody;
                    IBuffer res14 = BufferProvider.Provide();
                    res14.WriteUInt32(character2.DeadBodyInstanceId); //Instance ID of dead body
                    res14.WriteUInt32(character2.InstanceId); //Reference to actual player's instance ID
                    res14.WriteCString("soulname"); // Soul name 
                    res14.WriteCString($"{character2.Name}"); // Character name
                    res14.WriteFloat(character2.X + 200); // X
                    res14.WriteFloat(character2.Y); // Y
                    res14.WriteFloat(character2.Z); // Z
                    res14.WriteByte(character2.Heading); // Heading
                    res14.WriteInt32(character2.Level); //??level?


                    int numEntries = 19;
                    res14.WriteInt32(numEntries); //less than or equal to 19
                    //Consolidated Frequently Used Code
                    //LoadEquip.SlotSetup(res14, character2, numEntries);

                    res14.WriteInt32(numEntries);
                    //Consolidated Frequently Used Code
                    //LoadEquip.EquipItems(res14, character2, numEntries);

                    res14.WriteInt32(numEntries);
                    //Consolidated Frequently Used Code
                    //LoadEquip.EquipSlotBitMask(res14, character2, numEntries);

                    //Traits
                    res14.WriteUInt32(character2.RaceId); //race
                    res14.WriteUInt32(character2.SexId);
                    res14.WriteByte(character2.HairId); //hair
                    res14.WriteByte(character2.HairColorId); //color
                    res14.WriteByte(character2.FaceId); //face

                    res14.WriteInt32(_deadBody
                        .ConnectionState); // 0 = bag, 1 for dead? (Can't enter soul form if this isn't 0 or 1 i think).
                    res14.WriteInt32(_deadBody.ModelType); //4 = ash pile, not sure what this is.
                    res14.WriteInt32(y);
                    res14.WriteInt32(_deadBody
                        .deathPose); //death pose 0 = faced down, 1 = head chopped off, 2 = no arm, 3 = faced down, 4 = chopped in half, 5 = faced down, 6 = faced down, 7 and up "T-pose" the body (ONLY SEND 1 IF YOU ARE CALLING THIS FOR THE FIRST TIME)
                    res14.WriteByte(_deadBody
                        .CriminalStatus); //crim status (changes icon on the end also), 0 = white, 1 = yellow, 2 = red, 3 = red with crim icon, 
                    res14.WriteByte(_deadBody.BeginnerProtection); // (bool) Beginner protection
                    res14.WriteInt32(1);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_charabody_data, res14,
                        ServerType.Area);
                    break;

                case "event":
                    IBuffer res16 = BufferProvider.Provide();
                    //recv_event_start = 0x1B5C, 
                    res16.WriteInt32(0);
                    res16.WriteByte(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_start, res16, ServerType.Area);

                    IBuffer res17 = BufferProvider.Provide();
                    //recv_event_quest_report = 0xE07E,
                    res17.WriteInt32(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_quest_report, res17, ServerType.Area);

                    IBuffer res18 = BufferProvider.Provide();
                    //recv_event_block_message_end_no_object = 0x1AB,
                    //Router.Send(client, (ushort)AreaPacketId.recv_event_block_message_end_no_object, res18, ServerType.Area);

                    IBuffer res19 = BufferProvider.Provide();
                    Router.Send(client, (ushort) AreaPacketId.recv_event_sync, res19, ServerType.Area);
                    break;

                case "popup":
                    IBuffer res22 = BufferProvider.Provide();
                    //recv_event_start = 0x1B5C, 
                    res22.WriteInt32(0);
                    res22.WriteByte((byte) y);
                    //Router.Send(client, (ushort)AreaPacketId.recv_event_start, res22, ServerType.Area);

                    IBuffer res21 = BufferProvider.Provide();
                    //recv_normal_system_message = 0xAE2B,
                    res21.WriteCString("ToBeFound");
                    Router.Send(client, (ushort) AreaPacketId.recv_normal_system_message, res21, ServerType.Area);

                    IBuffer res23 = BufferProvider.Provide();
                    //recv_event_end = 0x99D, 
                    res23.WriteByte((byte) y);
                    //Router.Send(client, (ushort)AreaPacketId.recv_event_end, res23, ServerType.Area);
                    break;

                case "recv":
                    IBuffer res24 = BufferProvider.Provide();
                    //recv_auction_receive_item_r = 0xB1CA,
                    res24.WriteInt32(y);
                    Router.Send(client, (ushort) AreaPacketId.recv_auction_receive_gold_r, res24, ServerType.Area);
                    break;

                case "atk":  //udates settings 1-22 on the attribute tab
                    res = BufferProvider.Provide();
                    res.WriteInt32(y);
                    res.WriteInt16(888);
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_atk_magic_attr, res, ServerType.Area);
                    break;

                case "ability":  //udates settings 1-7 on the ability tab
                    res = BufferProvider.Provide();
                    res.WriteInt32(y);
                    res.WriteInt16(10);
                    res.WriteInt16(25);
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_ability, res, ServerType.Area);
                    break;

                case "battle":  //udates settings 1-7 on the ability tab
                    res = BufferProvider.Provide();
                    res.WriteInt16(10);
                    res.WriteInt16(10);
                    res.WriteInt16(10);
                    res.WriteInt16(10);

                    res.WriteInt16(10);
                    res.WriteInt16(10);
                    res.WriteInt16(10);
                    res.WriteInt16(10);
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_battle_base_param, res, ServerType.Area);
                    break;

                case "alignment":
                    IBuffer res28 = BufferProvider.Provide();
                    //recv_chara_update_alignment_param = 0xB435,
                    res28.WriteInt32(1);
                    res28.WriteInt32(2);
                    res28.WriteInt32(3);
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_alignment_param, res28, ServerType.Area);
                    res28 = BufferProvider.Provide();
                    res28.WriteInt32(y); //Alignment ID
                    Router.Send(client, (ushort)AreaPacketId.recv_chara_update_alignment, res28, ServerType.Area);
                    break;

                case "shop":
                    IBuffer res29 = BufferProvider.Provide();
                    //recv_shop_notify_open = 0x52FD, // Parent = 0x5243 // Range ID = 02
                    res29.WriteInt16((short) y); //Shop type
                    res29.WriteInt32(0);
                    res29.WriteInt32(0);
                    res29.WriteByte(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_shop_notify_open, res29, ServerType.Area);
                    break;

                case "sc":
                    IBuffer res32 = BufferProvider.Provide();
                    //recv_shop_sell_check_r = 0x4E8D,
                    res32.WriteInt32(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_shop_sell_check_r, res32, ServerType.Area);
                    break;

                case "view":
                    IBuffer res33 = BufferProvider.Provide();
                    //recv_chara_view_landing_notify = 0x14DA, 
                    res33.WriteInt32(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_view_landing_notify, res33,
                        ServerType.Area);
                    break;

                case "damage":
                    int hp = character2.Hp.current;
                    client.Character.Hp.Modify(-hp, character2.InstanceId);
                    IBuffer res35 = BufferProvider.Provide();
                    res35.WriteInt32(hp - hp);
                    Router.Send(client, (ushort) AreaPacketId.recv_chara_update_hp, res35, ServerType.Area);
                    break;

                case "itemforth":
                    IBuffer res38 = BufferProvider.Provide();
                    res38.WriteUInt32(200000002); //item ID?
                    res38.WriteInt32(200101); //Owner going 'forth'  id?
                    res38.WriteUInt32(200201); //item state setting?
                    res38.WriteByte(1);//newJp
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_update_notify_item_forth, res38,
                        ServerType.Area);
                    break;

                case "disconnect":
                    NecClient DeadManClient = Server.Clients.GetByCharacterInstanceId(x);
                    IBuffer res39 = BufferProvider.Provide();
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    res39.WriteUInt32(client.Character.InstanceId);
                    Router.Send(DeadManClient, (ushort) AreaPacketId.recv_chara_update_notify_item_forth, res39,
                        ServerType.Area);
                    break;

                case "crime":
                    //for (byte i = 0; i < y; i++)
                {
                    NecClient crimeClient = Server.Clients.GetByCharacterInstanceId(x);
                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteUInt32(crimeClient.Character.InstanceId);
                    res40.WriteByte((byte) y);
                    client.Character.criminalState = (byte) y;
                    Logger.Debug($"Setting crime level for Character {crimeClient.Character.Name} to {y}");
                    Router.Send(crimeClient, (ushort) AreaPacketId.recv_chara_update_notify_crime_lv, res40, ServerType.Area);
                    Router.Send(crimeClient.Map, (ushort) AreaPacketId.recv_charabody_notify_crime_lv, res40, ServerType.Area, crimeClient); 
                    Thread.Sleep(400);
                }
                    break;

                case "inherit":
                    //for (byte i = 0; i < y; i++)
                {
                    NecClient inheritClient = Server.Clients.GetByCharacterInstanceId(x);
                    IBuffer res41 = BufferProvider.Provide();
                    res41.WriteInt32(y);
                    res41.WriteInt32(0x64); //less than or equal to 0x64
                    for (int i = 0; i < 0x64; i++) //limit is the int32 above
                    {
                        res41.WriteInt32(i);
                        // TODO what ip is this? Area Msg ?? use -> Server.Setting.DataAreaIpAddress or Server.Setting.DataMsgIpAddress
                        res41.WriteFixedString("127.0.0.1", 0x10); //size is 0x10
                    }

                    res41.WriteUInt32(client.Character.InstanceId);
                    // TODO what ip is this? Area Msg ?? use -> Server.Setting.DataAreaIpAddress or Server.Setting.DataMsgIpAddress
                    res41.WriteFixedString("127.0.0.1", 0x10); //size is 0x10
                    res41.WriteByte((byte) y);
                    Router.Send(inheritClient, (ushort) MsgPacketId.recv_chara_get_inheritinfo_r, res41,
                        ServerType.Msg);

                    Thread.Sleep(400);
                }
                    break;

                case "seqmask":
                    IBuffer res43 = BufferProvider.Provide();
                    //95 torso ?
                    //55 full armor too ?
                    //93 full armor ?
                    // 27 full armor ?
                    //11 under ?
                    // 38 = boots and cape
                    //byte y = unchecked((byte)110111);
                    //byte y = unchecked ((byte)Util.GetRandomNumber(0, 100)); // for the moment i only get the armor on this way :/


                    /*int[] EquipBitMask = new int[]
                    {
                        1, 2, 4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192, 16384, 32768, 65536, 131072, 262144, 524288,
                        1048576, 2097152
                    };*/
                    res43.WriteInt64(180201);
                    res43.WriteInt32(y);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_item_update_spirit_eqmask, res43, ServerType.Area);
                    break;

                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "chara";

        public override string HelpText =>
            "usage: `/chara [command] [instance id] [y]` - Fires a recv to the game of command type with [instance id] as target and [y] number as an argument.";

        private void SendBattleReportStartNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(instance.InstanceId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }

        private void SendBattleReportEndNotify(NecClient client, IInstance instance)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }
    }
}
