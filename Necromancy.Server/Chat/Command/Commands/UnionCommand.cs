using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Does Union stuff
    /// </summary>
    public class UnionCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(UnionCommand));

        public UnionCommand(NecServer server) : base(server)
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

            IBuffer res36 = BufferProvider.Provide();

            switch (command[0])
            {
                case "union":
                    res36.WriteUInt32(client.Character.InstanceId);
                    res36.WriteInt32(client.Character.unionId);
                    res36.WriteCString("Trade_Union");
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
                    break;
                case "mant":
                    res36.WriteInt32(0);
                    for (int i = 0; i < 0x10; i++)
                    {
                        res36.WriteByte(0);
                    }

                    Router.Send(client, (ushort)AreaPacketId.recv_union_mantle_open, res36, ServerType.Area);
                    break;
                case "xunion":
                    res36.WriteUInt32(client.Character.InstanceId);
                    res36.WriteInt32(0 /*client.Character.UnionId*/);
                    res36.WriteCString("");
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_union_data, res36, ServerType.Area);
                    break;

                case "disband":
                    Router.Send(client, (ushort) MsgPacketId.recv_union_notify_disband, res36, ServerType.Msg);

                    res36.WriteInt32(0); //error check
                    Router.Send(client, (ushort) AreaPacketId.recv_union_request_disband_result, res36,
                        ServerType.Area);

                    IBuffer res37 = BufferProvider.Provide();
                    res37.WriteInt32(client.Character.unionId);
                    Router.Send(client, (ushort) MsgPacketId.recv_union_request_disband_r, res37, ServerType.Msg);

                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteUInt32(client.Character.InstanceId);
                    res40.WriteInt32(0 /*client.Character.UnionId*/);
                    res40.WriteCString("");
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_chara_notify_union_data, res40, ServerType.Area);

                    client.Character.unionId = 0;

                    break;

                case "open":
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_union_open_window, res36, ServerType.Area);
                    break;

                case "storage":
                    //Start an event
                    IBuffer res35 = BufferProvider.Provide();
                    res35.WriteInt32(0);
                    res35.WriteByte(0);
                    Router.Send(client, (ushort)AreaPacketId.recv_event_start, res35, ServerType.Area);
                    //Open union storage
                    res36.WriteUInt64(client.Soul.WarehouseGold); //todo make union gold variable
                    res36.WriteInt64(500); //??
                    Router.Send(client, (ushort) AreaPacketId.recv_event_union_storage_open, res36,
                        ServerType.Area);
                    break;

                case "establish":
                    res36.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_union_request_establish_r, res36,
                        ServerType.Area);
                    break;

                case "rename":
                    res36.WriteCString("YouDidIt");
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_union_rename_open, res36, ServerType.Area);

                    IBuffer res38 = BufferProvider.Provide();
                    res38.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_union_request_rename_r, res38, ServerType.Area);
                    break;
                case "resetid":
                    client.Character.unionId = 0;
                    break;

                case "growth":
                    res36.WriteInt32(0); //error check
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_union_request_growth_result, res36,
                        ServerType.Area);
                    IBuffer res39 = BufferProvider.Provide();
                    res39.WriteInt16(3); //sets union current exp
                    Router.Send(client.Map, (ushort) MsgPacketId.recv_union_notify_growth, res39, ServerType.Msg);
                    break;

                case "display":
                    res36.WriteInt32(333); //error check
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_quest_display_r, res36, ServerType.Area);
                    break;

                case "order":
                    IBuffer res2 = BufferProvider.Provide();
                    res2.WriteInt32(0); // 0 = normal 1 = cinematic
                    res2.WriteByte(0);

                    Router.Send(client, (ushort) AreaPacketId.recv_event_start, res2, ServerType.Area);
                    IBuffer res = BufferProvider.Provide();
                    res.WriteInt32(1999);
                    res.WriteByte(1);
                    res.WriteFixedString("aaaaaaa", 0x61);
                    res.WriteInt32(1);
                    res.WriteInt32(1);
                    res.WriteFixedString("ffffffffff", 0x61);
                    res.WriteByte(1);
                    res.WriteByte(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);
                    res.WriteInt32(1);
                    int numEntries4 = 0xA;
                    res.WriteInt32(numEntries4);
                    for (int i = 0; i < numEntries4; i++)
                    {
                        res.WriteInt32(0x10); //size of string
                        res.WriteFixedString("asfsaf", 0x10);
                        res.WriteInt16(1);
                        res.WriteInt32(1);
                    }

                    res.WriteByte(1);
                    int numEntries5 = 0xC;
                    for (int k = 0; k < numEntries5; k++)
                    {
                        res.WriteInt32(0x10); //size of string
                        res.WriteFixedString("boooooo", 0x10);
                        res.WriteInt16(1);
                        res.WriteInt32(1);
                    }

                    res.WriteByte(1);
                    //??res.WriteByte(1);
                    res.WriteFixedString("Eat the monster", 0x181);
                    res.WriteFixedString("no wait,  that'd be a really weird quest", 0x181);
                    for (int m = 0; m < 0x5; m++)
                    {
                        res.WriteByte(1);
                        res.WriteInt32(10101);
                        res.WriteInt32(100101);
                        res.WriteInt32(0);
                        res.WriteInt32(0);
                    }

                    res.WriteByte(2);
                    //no_JP Router.Send(client, (ushort) AreaPacketId.recv_event_quest_order, res, ServerType.Area);
                    break;

                case "mission":
                    IBuffer res3 = BufferProvider.Provide();
                    res3.WriteInt32(0); // 0 = normal 1 = cinematic
                    res3.WriteByte(0);

                    // Router.Send(client, (ushort)AreaPacketId.recv_event_start, res3, ServerType.Area);

                    IBuffer res4 = BufferProvider.Provide();
                    res4.WriteInt32(1); // 0 = normal 1 = cinematic
                    res4.WriteInt32(1); // 0 = normal 1 = cinematic

                    Router.Send(client, (ushort) AreaPacketId.recv_quest_get_mission_quest_works_r, res4,
                        ServerType.Area);
                    break;

                case "begin":
                    IBuffer res5 = BufferProvider.Provide();
                    res5.WriteInt32(0); // 0 = normal 1 = cinematic
                    res5.WriteByte(0);

                    Router.Send(client, (ushort) AreaPacketId.recv_event_start, res5, ServerType.Area);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_quest_report_list_begin, res36,
                        ServerType.Area);
                    break;
                case "start":
                    uint.TryParse(command[1], out uint x);
                    IBuffer res21 = BufferProvider.Provide();
                    res21.WriteInt32(1); // 0 = normal 1 = cinematic
                    res21.WriteByte(255);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_start, res21, ServerType.Area);

                    IBuffer res22 = BufferProvider.Provide();

                    res22.WriteCString(
                        "tutorial/tutorial_soul"); //ReadScript query to ...\Data\script\event\. Will play if found
                    Router.Send(client, (ushort) AreaPacketId.recv_event_script_play, res22, ServerType.Area);


                    break;

                default:
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    Task.Delay(TimeSpan.FromMilliseconds((int) (10 * 1000))).ContinueWith
                    (t1 =>
                        {
                            IBuffer res = BufferProvider.Provide();
                            res.WriteByte(0);
                            Router.Send(client, (ushort) AreaPacketId.recv_event_end, res, ServerType.Area);
                        }
                    );
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "union";
        public override string HelpText => "usage: `/union [command]` - Does something Union related.";
    }
}
