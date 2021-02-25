using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Quick mob test commands.
    /// </summary>
    public class MobCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(MobCommand));

        public MobCommand(NecServer server) : base(server)
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

            if (!int.TryParse(command[1], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value to test"));
                return;
            }

            IInstance instance = Server.Instances.GetInstance(client.Character.eventSelectReadyCode);
            MonsterSpawn monsterSpawn = null;

            if (instance is MonsterSpawn monsterSpawn2)
            {
                client.Map.MonsterSpawns.TryGetValue(monsterSpawn2.InstanceId, out monsterSpawn2);
                monsterSpawn = monsterSpawn2;
            }

            switch (command[0])
            {
                case "state":
                    Logger.Debug($"MonsterInstanceId: {monsterSpawn.InstanceId} state is being set to {x}");

                    IBuffer res = BufferProvider.Provide();

                    res.WriteUInt32(monsterSpawn.InstanceId);
                    //Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing). 
                    res.WriteInt32(x);
                    Router.Send(client, (ushort) AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);
                    break;

                case "dead":
                    //recv_battle_report_noact_notify_dead = 0xCDC9,
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteUInt32(monsterSpawn.InstanceId);
                    res2.WriteInt32(x);
                    res2.WriteInt32(x);
                    res2.WriteInt32(x);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_noact_notify_dead, res2,
                        ServerType.Area);
                    break;

                case "pose":
                    IBuffer res3 = BufferProvider.Provide();
                    //recv_battle_attack_pose_start_notify = 0x7CB2,
                    res3.WriteInt32(x);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res3,
                        ServerType.Area);
                    break;

                case "pose2":
                    IBuffer resT = BufferProvider.Provide();
                    resT.WriteUInt32(client.Character.InstanceId); //Character ID
                    resT.WriteInt32(x); //Character pose
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_pose_notify, resT, ServerType.Area,
                        client);

                    break;

                case "hate":
                    Logger.Debug(
                        $"Setting Monster Hate for Monster ID {monsterSpawn.InstanceId} to act on character ID {client.Character.InstanceId}");
                    IBuffer res4 = BufferProvider.Provide();
                    res4.WriteUInt32(monsterSpawn
                        .InstanceId); //Unique instance of this monsters "Hate" attribute. not to be confused with the Monsters InstanceID
                    res4.WriteInt32(x);
                    Router.Send(client, (ushort) AreaPacketId.recv_monster_hate_on, res4, ServerType.Area);
                    break;

                case "jump":
                    monsterSpawn.Z += x;
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteUInt32(monsterSpawn.InstanceId);
                    res5.WriteFloat(monsterSpawn.X);
                    res5.WriteFloat(monsterSpawn.Y);
                    res5.WriteFloat(monsterSpawn.Z);
                    res5.WriteByte(monsterSpawn.Heading);
                    res5.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res5, ServerType.Area);
                    break;

                case "emotion":
                    //recv_emotion_notify_type = 0xF95B,
                    IBuffer res6 = BufferProvider.Provide();
                    res6.WriteUInt32(monsterSpawn.InstanceId);
                    res6.WriteInt32(x);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_emotion_notify_type, res6, ServerType.Area);
                    break;

                case "deadstate":
                    //recv_charabody_notify_deadstate = 0xCC36, // Parent = 0xCB94 // Range ID = 03
                    IBuffer res7 = BufferProvider.Provide();
                    res7.WriteUInt32(monsterSpawn.InstanceId);
                    res7.WriteInt32(
                        x); //4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                    res7.WriteInt32(x);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_deadstate, res7,
                        ServerType.Area);
                    break;

                case "target":
                    //recv_object_sub_target_update_notify = 0x23E5, 
                    IBuffer resA = BufferProvider.Provide();
                    resA.WriteUInt32(monsterSpawn.InstanceId);
                    //resA.WriteInt64(x);
                    resA.WriteUInt32(client.Character.InstanceId);
                    resA.WriteUInt32(client.Character.InstanceId);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_sub_target_update_notify, resA,
                        ServerType.Area);
                    break;

                case "hp":
                    //recv_object_hp_per_update_notify = 0xFF00,
                    IBuffer resB = BufferProvider.Provide();
                    resB.WriteUInt32(monsterSpawn.InstanceId);
                    resB.WriteByte((byte) x);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_hp_per_update_notify, resB,
                        ServerType.Area);
                    break;

                case "cast":
                    IBuffer resC = BufferProvider.Provide();
                    //recv_battle_report_action_monster_skill_start_cast = 0x1959,
                    resC.WriteUInt32(client.Character.InstanceId);
                    resC.WriteInt32(13010101);
                    resC.WriteFloat(3);
                    Router.Send(client, (ushort) AreaPacketId.recv_battle_report_action_monster_skill_start_cast, resC,
                        ServerType.Area);
                    break;

                case "exec":
                    IBuffer resE = BufferProvider.Provide();
                    //recv_battle_report_action_monster_skill_exec = 0x2A82,
                    resE.WriteInt32(x);
                    Router.Send(client, (ushort) AreaPacketId.recv_battle_report_action_monster_skill_exec, resE,
                        ServerType.Area);
                    break;

                case "start":
                    IBuffer resO = BufferProvider.Provide();
                    resO.WriteUInt32(instance.InstanceId);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_start_notify, resO,
                        ServerType.Area);
                    break;

                case "end":
                    IBuffer resG = BufferProvider.Provide();
                    resG.WriteUInt32(monsterSpawn.InstanceId);
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_start_notify, resG, ServerType.Area);

                    //insert battle_report command to test here
                    IBuffer resF = BufferProvider.Provide();
                    //recv_battle_report_notify_hit_effect_name = 0xB037,
                    resF.WriteUInt32(monsterSpawn.InstanceId);
                    resF.WriteCString("ToBeFound");
                    resF.WriteCString("ToBeFound_2");
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect_name, resF, ServerType.Area);

                    IBuffer resK = BufferProvider.Provide();
                    //recv_battle_report_notify_hit_effect = 0x179D, 
                    resK.WriteInt32(x);
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_notify_hit_effect, resK, ServerType.Area);

                    IBuffer resL = BufferProvider.Provide();
                    //recv_battle_report_action_effect_onhit = 0x5899,
                    resL.WriteInt32(x);
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_action_effect_onhit, resL, ServerType.Area);

                    //recv_battle_report_noact_notify_buff_effect = 0xDB5E,
                    IBuffer resM = BufferProvider.Provide();
                    resM.WriteInt32(696969);
                    resM.WriteInt32(x);
                    //Router.Send(client.Map, (ushort)AreaPacketId.recv_battle_report_noact_notify_buff_effect, resM, ServerType.Area);

                    IBuffer resH = BufferProvider.Provide();
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_end_notify, resH, ServerType.Area);
                    break;

                case "gimmick":
                    //recv_data_notify_gimmick_data = 0xBFE9,
                    IBuffer resI = BufferProvider.Provide();
                    resI.WriteInt32(69696);
                    resI.WriteFloat(client.Character.X);
                    resI.WriteFloat(client.Character.Y);
                    resI.WriteFloat(client.Character.Z);
                    resI.WriteByte(client.Character.Heading);
                    resI.WriteInt32(x); //Gimmick number (from gimmick.csv)
                    resI.WriteInt32(0);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_gimmick_data, resI, ServerType.Area);
                    break;

                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "mob";

        public override string HelpText =>
            "usage: `/mob [argument] [number]` - Fires a recv to the game of argument type with [x] number as an argument, must have swung/cast at a mob beforehand.";

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
