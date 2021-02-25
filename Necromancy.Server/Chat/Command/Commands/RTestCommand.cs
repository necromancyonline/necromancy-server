using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Anything related commands.  This is a sandbox
    /// </summary>
    public class RTestCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(RTestCommand));

        public RTestCommand(NecServer server) : base(server)
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
            int x = 1;
            if (!int.TryParse(command[1], out  x))
            {
                try
                {
                    string binaryString = command[1];
                    binaryString = binaryString.Replace("0b", "");
                    Logger.Debug(binaryString);
                    x = Convert.ToInt32(binaryString, 2);
                }
                catch
                {
                    responses.Add(ChatResponse.CommandError(client, $"no value specified. setting x to 1"));
                    //return;
                }

            }

            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Good Job!"));
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = normal 1 = cinematic
            res.WriteByte(0);
           //Router.Send(client, (ushort)AreaPacketId.recv_event_start, res, ServerType.Area);

            switch (command[0])
            {

                case "itemobject":
                    recv_data_notify_local_itemobject_data itemObject = new recv_data_notify_local_itemobject_data(client.Character, x);
                    Router.Send(client.Map, itemObject);
                    break;

                case "itemcharapost":
                    recv_item_chara_post_notify itemCharaPost = new recv_item_chara_post_notify(x, (byte)y);
                    Router.Send(client.Map, itemCharaPost);
                    break;

                case "itemuse":
                    RecvItemUseNotify itemUseNotify = new RecvItemUseNotify((long)x, (float)y);
                    Router.Send(client.Map, itemUseNotify);
                    RecvItemUse itemUse = new RecvItemUse(0 /*success*/, (float)y);
                    Router.Send(client.Map, itemUse);
                    break;

                case "dispitem":
                    RecvSoulDispItemNotifyData dispItem = new RecvSoulDispItemNotifyData(x);
                    Router.Send(client.Map, dispItem);
                    break;

                case "templeopen":
                    RecvTempleNotifyOpen templeOpen = new RecvTempleNotifyOpen((byte)x);
                    Router.Send(client.Map, templeOpen);
                    break;

                case "wantedstate":
                    RecvWantedUpdateState wantedState = new RecvWantedUpdateState(x);
                    Router.Send(client.Map, wantedState);
                    RecvWantedUpdateStateNotify wantedStateNotify = new RecvWantedUpdateStateNotify((int)client.Character.InstanceId, x);
                    Router.Send(client.Map, wantedStateNotify);
                    break;

                case "jailmoney":
                    RecvWantedJailUpdateMoney jailMoney = new RecvWantedJailUpdateMoney();
                    Router.Send(client.Map, jailMoney);
                    break;

                case "lootaccess":
                    RecvLootAccessObject lootAcess = new RecvLootAccessObject();
                    Router.Send(client.Map, lootAcess);
                    break;

                case "partygetitem":
                    //arearecv
                    RecvPartyNotifyGetItem recvPartyNotifyGetItem = new RecvPartyNotifyGetItem(client.Character.InstanceId);
                    Router.Send(recvPartyNotifyGetItem, client);
                    //message recv
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteCString(" a Dagger or any long string named object ");
                    res.WriteByte(20);
                    Router.Send(client.Map, (ushort)MsgPacketId.recv_party_notify_get_item, res, ServerType.Msg);
                    break;

                case "partygetmoney":
                    RecvPartyNotifyGetMoney recvPartyNotifyGetMoney = new RecvPartyNotifyGetMoney(client.Character.InstanceId);
                    Router.Send(client.Map, recvPartyNotifyGetMoney);
                    break;

                case "partybuff":
                    RecvPartyNotifyAttachBuff recvPartyNotifyAttachBuff = new RecvPartyNotifyAttachBuff();
                    Router.Send(client.Map, recvPartyNotifyAttachBuff);
                    break;

                case "partydragon":
                    RecvPartyNotifyUpdateDragon recvPartyNotifyUpdateDragon = new RecvPartyNotifyUpdateDragon(client.Character.InstanceId);
                    Router.Send(client.Map, recvPartyNotifyUpdateDragon);
                    break;

                case "partymap":
                    RecvPartyNotifyUpdateMap recvPartyNotifyUpdateMap = new RecvPartyNotifyUpdateMap(client);
                    Router.Send(client.Map, recvPartyNotifyUpdateMap);
                    break;

                case "partysync":
                    RecvPartyNotifyUpdateSyncLevel recvPartyNotifyUpdateSyncLevel = new RecvPartyNotifyUpdateSyncLevel(client);
                    Router.Send(client.Map, recvPartyNotifyUpdateSyncLevel);
                    break;

                case "ap":
                    RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(Util.GetRandomNumber(0,200));
                    Router.Send(client.Map, recvCharaUpdateAp);
                    client.Character.Gp.setCurrent(25);
                    break;

                case "ac":
                    RecvCharaUpdateAc recvCharaUpdateAc = new RecvCharaUpdateAc(Util.GetRandomNumber(0, 200));
                    Router.Send(client.Map, recvCharaUpdateAc);
                    break;

                case "iobject":
                    //SendDataNotifyItemObjectData
                    // objectid : %d, stateflag : %d, type : %d\n
                    res = BufferProvider.Provide();

                    res.WriteInt32(12345); //object instance ID
                    res.WriteFloat(client.Character.X); //Initial X
                    res.WriteFloat(client.Character.Y); //Initial Y
                    res.WriteFloat(client.Character.Z); //Initial Z

                    res.WriteFloat(client.Character.X); //Final X
                    res.WriteFloat(client.Character.Y); //Final Y
                    res.WriteFloat(client.Character.Z); //Final Z
                    res.WriteByte(client.Character.Heading); //View offset

                    res.WriteInt32(0); // ?
                    res.WriteUInt32(client.Character.InstanceId); // ?
                    res.WriteUInt32(0); // ?

                    res.WriteInt32(0b1); //object state. 1 = lootable .
                    res.WriteInt32(2); //type

                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);
                    break;

                case "o2716":
                    res = BufferProvider.Provide();

                    res.WriteUInt32(0); //errhceck

                    Router.Send(client, (ushort)0x2716, res, ServerType.Area);
                    break;

                    
                case "partnersummon":
                    recv_soul_partner_summon_start_notify recv_soul_partner_summon_start_notify = new recv_soul_partner_summon_start_notify();
                    Router.Send(client.Map, recv_soul_partner_summon_start_notify);
                    recv_soul_partner_summon_exec_r recv_soul_partner_summon_exec_r = new recv_soul_partner_summon_exec_r();
                    Router.Send(client.Map, recv_soul_partner_summon_exec_r);
                    recv_soul_partner_unlock_avatar recv_soul_partner_unlock_avatar = new recv_soul_partner_unlock_avatar();
                    Router.Send(client.Map, recv_soul_partner_unlock_avatar);
                    recv_soul_partner_notify_avatar recv_soul_partner_notify_avatar = new recv_soul_partner_notify_avatar();
                    Router.Send(client.Map, recv_soul_partner_notify_avatar);
                    
                    break;

                case "selectraise":
                    RecvDbgSelectRaise recvDbgSelectRaise = new RecvDbgSelectRaise();
                    Router.Send(client.Map, recvDbgSelectRaise);
                    break;

                case "dragonpos":
                    RecvSelfDragonPosNotify recvSelfDragonPosNotify = new RecvSelfDragonPosNotify(client, (byte)x);
                    Router.Send(client.Map, recvSelfDragonPosNotify);
                    break;

                case "dragonwarp":
                    RecvSelfDragonWarpNotify recvSelfDragonWarpNotify = new RecvSelfDragonWarpNotify((int)client.Character.InstanceId+100);
                    Router.Send(client.Map, recvSelfDragonWarpNotify);
                    break;
                    
                case "exdragon":
                    RecvDataNotifyNpcExDragon recvDataNotifyNpcExDragon = new RecvDataNotifyNpcExDragon((uint)x);
                    Router.Send(client.Map, recvDataNotifyNpcExDragon);
                    break;

                case "debug":
                    //recv_data_notify_debug_object_data = 0x6510,
                    IBuffer resJ = BufferProvider.Provide();
                    resJ.WriteUInt32(400000221);
                    int numEntries = 0x20;
                    resJ.WriteInt32(numEntries); //less than or equal to 0x20
                    for (int i = 0; i < numEntries; i++)
                    {
                        resJ.WriteFloat(client.Character.X+Util.GetRandomNumber(10,50));
                        resJ.WriteFloat(client.Character.Y + Util.GetRandomNumber(10, 50));
                        resJ.WriteFloat(client.Character.Z + Util.GetRandomNumber(10, 50));
                    }

                    resJ.WriteByte(0);
                    resJ.WriteByte(0);
                    resJ.WriteByte(0);
                    Router.Send(client.Map, (ushort)AreaPacketId.recv_data_notify_debug_object_data, resJ,
                        ServerType.Area);
                    break;

                default: //you don't know what you're doing do you?
                    Logger.Error($"There is no recv of type : {command[0]} ");
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"{command[0]} is not a valid command."));
                    }
                    break;


            }

            res = BufferProvider.Provide();
            //Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteByte(0);
            //Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
            //Router.Send(client, (ushort)AreaPacketId.recv_event_end, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "rtest";

        public override string HelpText =>
            "usage: `/rtest [argument] [number] [parameter]` - this is free-form for testing new recvs";
    }
}
