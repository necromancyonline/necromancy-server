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

                case "o4abb":
                    res = BufferProvider.Provide();

                    res.WriteUInt32(0); 
                    res.WriteUInt16(0); 

                    Router.Send(client, (ushort)0x4abb, res, ServerType.Area);
                    break;

                case "ob684":
                    Recv0xB684 Recv0xB684 = new Recv0xB684();
                    Router.Send(client.Map, Recv0xB684);
                    break;

                case "ob4978":
                    Recv0x4978 Recv0x4978 = new Recv0x4978();
                    Router.Send(client.Map, Recv0x4978);
                    break;

                case "ob5418":
                    recv_0x5418 recv_0x5418 = new recv_0x5418();
                    Router.Send(client.Map, recv_0x5418);
                    break;

                case "ob10da":
                    Recv0x10DA Recv0x10DA = new Recv0x10DA();
                    Router.Send(client.Map, Recv0x10DA);
                    break;

                case "ob8d62":
                    Recv0x8D62 Recv0x8D62 = new Recv0x8D62();
                    Router.Send(client.Map, Recv0x8D62);
                    break;

                case "ob9201":
                    Recv0x9201 Recv0x9201 = new Recv0x9201();
                    Router.Send(client.Map, Recv0x9201);
                    break;

                case "ob9ca1":
                    Recv0x9CA1 Recv0x9CA1 = new Recv0x9CA1();
                    Router.Send(client.Map, Recv0x9CA1);
                    break;

                case "obba61":
                    Recv0xBA61 Recv0xBA61 = new Recv0xBA61();
                    Router.Send(client.Map, Recv0xBA61);
                    break;

                case "obd1f6":
                    Recv0xD1F6 Recv0xD1F6 = new Recv0xD1F6();
                    Router.Send(client.Map, Recv0xD1F6);
                    break;

                case "obe8b9":
                    Recv0xE8B9 Recv0xE8B9 = new Recv0xE8B9();
                    Router.Send(client.Map, Recv0xE8B9);
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

                case "level":
                    RecvEventStart recvEventStart = new RecvEventStart(0, 0);
                    Router.Send(recvEventStart, client);
                    Experience experience = new Experience();
                    client.Character.Level++;
                    client.Character.Hp.setMax(client.Character.Hp.max + 10);
                    client.Character.Mp.setMax(client.Character.Mp.max + 10);
                    client.Character.Strength += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Vitality += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Dexterity += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Agility += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Intelligence += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Piety += (ushort)Util.GetRandomNumber(0, 2);
                    client.Character.Luck += (ushort)Util.GetRandomNumber(0, 2);
                    int luckyShot = Util.GetRandomNumber(0, client.Character.Luck);
                    if (luckyShot > (client.Character.Luck * .8))
                    {
                        client.Character.Hp.setMax(client.Character.Hp.max + 10);
                        client.Character.Mp.setMax(client.Character.Mp.max + 10);
                        client.Character.Strength       = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Strength );
                        client.Character.Vitality       = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Vitality);
                        client.Character.Dexterity      = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Dexterity );
                        client.Character.Agility        = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Agility );
                        client.Character.Intelligence   = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Intelligence );
                        client.Character.Piety          = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Piety );
                        client.Character.Luck           = (ushort)(Util.GetRandomNumber(-2, 2) + client.Character.Luck );
                    }

                    RecvCharaUpdateLvDetailStart recvCharaUpdateLvDetailStart = new RecvCharaUpdateLvDetailStart();
                    RecvCharaUpdateLv recvCharaUpdateLv = new RecvCharaUpdateLv(client.Character);
                    RecvCharaUpdateLvDetail recvCharaUpdateLvDetail = new RecvCharaUpdateLvDetail(client.Character, experience);
                    RecvCharaUpdateLvDetail2 recvCharaUpdateLvDetail2 = new RecvCharaUpdateLvDetail2(client.Character, experience);
                    RecvCharaUpdateLvDetailEnd recvCharaUpdateLvDetailEnd = new RecvCharaUpdateLvDetailEnd();

                    RecvCharaUpdateMaxHp recvCharaUpdateMaxHp = new RecvCharaUpdateMaxHp(client.Character.Hp.max);
                    RecvCharaUpdateMaxMp recvCharaUpdateMaxMp = new RecvCharaUpdateMaxMp(client.Character.Mp.max);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityStr = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._str, client.Character.Strength, client.Character.battleParam.PlusStrength);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityVit = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._vit, client.Character.Vitality, client.Character.battleParam.PlusVitality);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityDex = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._dex, client.Character.Dexterity, client.Character.battleParam.PlusDexterity);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityAgi = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._agi, client.Character.Agility, client.Character.battleParam.PlusAgility);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityInt = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._int, client.Character.Intelligence, client.Character.battleParam.PlusIntelligence);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityPie = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._pie, client.Character.Piety, client.Character.battleParam.PlusPiety);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityLuk = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.ability._luk, client.Character.Luck, client.Character.battleParam.PlusLuck);

                    Router.Send(recvCharaUpdateLvDetailStart, client);


                    Router.Send(recvCharaUpdateMaxHp, client);
                    Router.Send(recvCharaUpdateMaxMp, client);
                    Router.Send(recvCharaUpdateAbilityStr, client);
                    Router.Send(recvCharaUpdateAbilityVit, client);
                    Router.Send(recvCharaUpdateAbilityDex, client);
                    Router.Send(recvCharaUpdateAbilityAgi, client);
                    Router.Send(recvCharaUpdateAbilityInt, client);
                    Router.Send(recvCharaUpdateAbilityPie, client);
                    Router.Send(recvCharaUpdateAbilityLuk, client);

                    Router.Send(recvCharaUpdateLv, client);
                    Router.Send(recvCharaUpdateLvDetail, client);
                    Router.Send(recvCharaUpdateLvDetail2, client);
                    Router.Send(recvCharaUpdateLvDetailEnd, client);

                    break;

                case "questworks":
                    RecvQuestGetMissionQuestWorks recvQuestGetMissionQuestWorks = new RecvQuestGetMissionQuestWorks();
                    Router.Send(client.Map, recvQuestGetMissionQuestWorks);
                    break;

                case "roguehistory":
                    recv_quest_get_rogue_mission_quest_history_r recv_quest_get_rogue_mission_quest_history_r = new recv_quest_get_rogue_mission_quest_history_r();
                    Router.Send(client.Map, recv_quest_get_rogue_mission_quest_history_r);
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

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "rtest";

        public override string HelpText =>
            "usage: `/rtest [argument] [number] [parameter]` - this is free-form for testing new recvs";
    }
}
