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
    ///     Anything related commands.  This is a sandbox
    /// </summary>
    public class RTestCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(RTestCommand));

        public RTestCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "rtest";

        public override string helpText =>
            "usage: `/rtest [argument] [number] [parameter]` - this is free-form for testing new recvs";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }

            int x = 1;
            if (!int.TryParse(command[1], out x))
                try
                {
                    string binaryString = command[1];
                    binaryString = binaryString.Replace("0b", "");
                    _Logger.Debug(binaryString);
                    x = Convert.ToInt32(binaryString, 2);
                }
                catch
                {
                    responses.Add(ChatResponse.CommandError(client, "no value specified. setting x to 1"));
                    //return;
                }

            if (!int.TryParse(command[2], out int y)) responses.Add(ChatResponse.CommandError(client, "Good Job!"));
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(2);
            router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = normal 1 = cinematic
            res.WriteByte(0);
            //Router.Send(client, (ushort)AreaPacketId.recv_event_start, res, ServerType.Area);

            switch (command[0])
            {
                case "itemobject":
                    RecvDataNotifyLocalItemobjectData itemObject = new RecvDataNotifyLocalItemobjectData(client.character, x);
                    router.Send(client.map, itemObject);
                    break;

                case "itemcharapost":
                    RecvItemCharaPostNotify itemCharaPost = new RecvItemCharaPostNotify(x, (byte)y);
                    router.Send(client.map, itemCharaPost);
                    break;

                case "itemuse":
                    RecvItemUseNotify itemUseNotify = new RecvItemUseNotify(x, y);
                    router.Send(client.map, itemUseNotify);
                    RecvItemUse itemUse = new RecvItemUse(0 /*success*/, y);
                    router.Send(client.map, itemUse);
                    break;

                case "dispitem":
                    RecvSoulDispItemNotifyData dispItem = new RecvSoulDispItemNotifyData(x);
                    router.Send(client.map, dispItem);
                    break;

                case "templeopen":
                    RecvTempleNotifyOpen templeOpen = new RecvTempleNotifyOpen((byte)x);
                    router.Send(client.map, templeOpen);
                    break;

                case "wantedstate":
                    RecvWantedUpdateState wantedState = new RecvWantedUpdateState(x);
                    router.Send(client.map, wantedState);
                    RecvWantedUpdateStateNotify wantedStateNotify = new RecvWantedUpdateStateNotify((int)client.character.instanceId, x);
                    router.Send(client.map, wantedStateNotify);
                    break;

                case "jailmoney":
                    RecvWantedJailUpdateMoney jailMoney = new RecvWantedJailUpdateMoney();
                    router.Send(client.map, jailMoney);
                    break;

                case "lootaccess":
                    RecvLootAccessObject lootAcess = new RecvLootAccessObject();
                    router.Send(client.map, lootAcess);
                    break;

                case "partygetitem":
                    //arearecv
                    RecvPartyNotifyGetItem recvPartyNotifyGetItem = new RecvPartyNotifyGetItem(client.character.instanceId);
                    router.Send(recvPartyNotifyGetItem, client);
                    //message recv
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.character.instanceId);
                    res.WriteCString(" a Dagger or any long string named object ");
                    res.WriteByte(20);
                    router.Send(client.map, (ushort)MsgPacketId.recv_party_notify_get_item, res, ServerType.Msg);
                    break;

                case "partygetmoney":
                    RecvPartyNotifyGetMoney recvPartyNotifyGetMoney = new RecvPartyNotifyGetMoney(client.character.instanceId);
                    router.Send(client.map, recvPartyNotifyGetMoney);
                    break;

                case "partybuff":
                    RecvPartyNotifyAttachBuff recvPartyNotifyAttachBuff = new RecvPartyNotifyAttachBuff();
                    router.Send(client.map, recvPartyNotifyAttachBuff);
                    break;

                case "partydragon":
                    RecvPartyNotifyUpdateDragon recvPartyNotifyUpdateDragon = new RecvPartyNotifyUpdateDragon(client.character.instanceId);
                    router.Send(client.map, recvPartyNotifyUpdateDragon);
                    break;

                case "partymap":
                    RecvPartyNotifyUpdateMap recvPartyNotifyUpdateMap = new RecvPartyNotifyUpdateMap(client);
                    router.Send(client.map, recvPartyNotifyUpdateMap);
                    break;

                case "partysync":
                    RecvPartyNotifyUpdateSyncLevel recvPartyNotifyUpdateSyncLevel = new RecvPartyNotifyUpdateSyncLevel(client);
                    router.Send(client.map, recvPartyNotifyUpdateSyncLevel);
                    break;

                case "ap":
                    RecvCharaUpdateAp recvCharaUpdateAp = new RecvCharaUpdateAp(Util.GetRandomNumber(0, 200));
                    router.Send(client.map, recvCharaUpdateAp);
                    client.character.Gp.SetCurrent(25);
                    break;

                case "ac":
                    RecvCharaUpdateAc recvCharaUpdateAc = new RecvCharaUpdateAc(Util.GetRandomNumber(0, 200));
                    router.Send(client.map, recvCharaUpdateAc);
                    break;

                case "iobject":
                    //SendDataNotifyItemObjectData
                    // objectid : %d, stateflag : %d, type : %d\n
                    res = BufferProvider.Provide();

                    res.WriteInt32(12345); //object instance ID
                    res.WriteFloat(client.character.x); //Initial X
                    res.WriteFloat(client.character.y); //Initial Y
                    res.WriteFloat(client.character.z); //Initial Z

                    res.WriteFloat(client.character.x); //Final X
                    res.WriteFloat(client.character.y); //Final Y
                    res.WriteFloat(client.character.z); //Final Z
                    res.WriteByte(client.character.heading); //View offset

                    res.WriteInt32(0); // ?
                    res.WriteUInt32(client.character.instanceId); // ?
                    res.WriteUInt32(0); // ?

                    res.WriteInt32(0b1); //object state. 1 = lootable .
                    res.WriteInt32(2); //type

                    router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_itemobject_data, res, ServerType.Area);
                    break;

                case "o2716":
                    res = BufferProvider.Provide();

                    res.WriteUInt32(0); //errhceck

                    router.Send(client, 0x2716, res, ServerType.Area);
                    break;

                case "o4abb":
                    res = BufferProvider.Provide();

                    res.WriteUInt32(0);
                    res.WriteUInt16(0);

                    router.Send(client, 0x4abb, res, ServerType.Area);
                    break;

                case "ob684":
                    Recv0Xb684 recv0Xb684 = new Recv0Xb684();
                    router.Send(client.map, recv0Xb684);
                    break;

                case "ob4978":
                    Recv0X4978 recv0X4978 = new Recv0X4978();
                    router.Send(client.map, recv0X4978);
                    break;

                case "ob5418":
                    Recv0X5418 recv0X5418 = new Recv0X5418();
                    router.Send(client.map, recv0X5418);
                    break;

                case "ob10da":
                    Recv0X10Da recv0X10Da = new Recv0X10Da();
                    router.Send(client.map, recv0X10Da);
                    break;

                case "ob8d62":
                    Recv0X8D62 recv0X8D62 = new Recv0X8D62();
                    router.Send(client.map, recv0X8D62);
                    break;

                case "ob9201":
                    Recv0X9201 recv0X9201 = new Recv0X9201();
                    router.Send(client.map, recv0X9201);
                    break;

                case "ob9ca1":
                    Recv0X9Ca1 recv0X9Ca1 = new Recv0X9Ca1();
                    router.Send(client.map, recv0X9Ca1);
                    break;

                case "obba61":
                    Recv0XBa61 recv0XBa61 = new Recv0XBa61();
                    router.Send(client.map, recv0XBa61);
                    break;

                case "obd1f6":
                    Recv0Xd1F6 recv0Xd1F6 = new Recv0Xd1F6();
                    router.Send(client.map, recv0Xd1F6);
                    break;

                case "obe8b9":
                    Recv0Xe8B9 recv0Xe8B9 = new Recv0Xe8B9();
                    router.Send(client.map, recv0Xe8B9);
                    break;


                case "partnersummon":
                    RecvSoulPartnerSummonStartNotify recvSoulPartnerSummonStartNotify = new RecvSoulPartnerSummonStartNotify();
                    router.Send(client.map, recvSoulPartnerSummonStartNotify);
                    RecvSoulPartnerSummonExecR recvSoulPartnerSummonExecR = new RecvSoulPartnerSummonExecR();
                    router.Send(client.map, recvSoulPartnerSummonExecR);
                    RecvSoulPartnerUnlockAvatar recvSoulPartnerUnlockAvatar = new RecvSoulPartnerUnlockAvatar();
                    router.Send(client.map, recvSoulPartnerUnlockAvatar);
                    RecvSoulPartnerNotifyAvatar recvSoulPartnerNotifyAvatar = new RecvSoulPartnerNotifyAvatar();
                    router.Send(client.map, recvSoulPartnerNotifyAvatar);

                    break;

                case "selectraise":
                    RecvDbgSelectRaise recvDbgSelectRaise = new RecvDbgSelectRaise();
                    router.Send(client.map, recvDbgSelectRaise);
                    break;

                case "dragonpos":
                    RecvSelfDragonPosNotify recvSelfDragonPosNotify = new RecvSelfDragonPosNotify(client, (byte)x);
                    router.Send(client.map, recvSelfDragonPosNotify);
                    break;

                case "dragonwarp":
                    RecvSelfDragonWarpNotify recvSelfDragonWarpNotify = new RecvSelfDragonWarpNotify((int)client.character.instanceId + 100);
                    router.Send(client.map, recvSelfDragonWarpNotify);
                    break;

                case "exdragon":
                    RecvDataNotifyNpcExDragon recvDataNotifyNpcExDragon = new RecvDataNotifyNpcExDragon((uint)x);
                    router.Send(client.map, recvDataNotifyNpcExDragon);
                    break;

                case "level":
                    RecvEventStart recvEventStart = new RecvEventStart(0, 0);
                    router.Send(recvEventStart, client);
                    Experience experience = new Experience();
                    client.character.level++;
                    client.character.Hp.SetMax(client.character.Hp.max + 10);
                    client.character.Mp.SetMax(client.character.Mp.max + 10);
                    client.character.strength += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.vitality += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.dexterity += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.agility += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.intelligence += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.piety += (ushort)Util.GetRandomNumber(0, 2);
                    client.character.luck += (ushort)Util.GetRandomNumber(0, 2);
                    int luckyShot = Util.GetRandomNumber(0, client.character.luck);
                    if (luckyShot > client.character.luck * .8)
                    {
                        client.character.Hp.SetMax(client.character.Hp.max + 10);
                        client.character.Mp.SetMax(client.character.Mp.max + 10);
                        client.character.strength = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.strength);
                        client.character.vitality = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.vitality);
                        client.character.dexterity = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.dexterity);
                        client.character.agility = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.agility);
                        client.character.intelligence = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.intelligence);
                        client.character.piety = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.piety);
                        client.character.luck = (ushort)(Util.GetRandomNumber(-2, 2) + client.character.luck);
                    }

                    RecvCharaUpdateLvDetailStart recvCharaUpdateLvDetailStart = new RecvCharaUpdateLvDetailStart();
                    RecvCharaUpdateLv recvCharaUpdateLv = new RecvCharaUpdateLv(client.character);
                    RecvCharaUpdateLvDetail recvCharaUpdateLvDetail = new RecvCharaUpdateLvDetail(client.character, experience);
                    RecvCharaUpdateLvDetail2 recvCharaUpdateLvDetail2 = new RecvCharaUpdateLvDetail2(client.character, experience);
                    RecvCharaUpdateLvDetailEnd recvCharaUpdateLvDetailEnd = new RecvCharaUpdateLvDetailEnd();

                    RecvCharaUpdateMaxHp recvCharaUpdateMaxHp = new RecvCharaUpdateMaxHp(client.character.Hp.max);
                    RecvCharaUpdateMaxMp recvCharaUpdateMaxMp = new RecvCharaUpdateMaxMp(client.character.Mp.max);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityStr = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Str, client.character.strength, client.character.battleParam.plusStrength);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityVit = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Vit, client.character.vitality, client.character.battleParam.plusVitality);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityDex = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Dex, client.character.dexterity, client.character.battleParam.plusDexterity);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityAgi = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Agi, client.character.agility, client.character.battleParam.plusAgility);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityInt = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Int, client.character.intelligence, client.character.battleParam.plusIntelligence);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityPie = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Pie, client.character.piety, client.character.battleParam.plusPiety);
                    RecvCharaUpdateAbility recvCharaUpdateAbilityLuk = new RecvCharaUpdateAbility((int)RecvCharaUpdateAbility.Ability.Luk, client.character.luck, client.character.battleParam.plusLuck);

                    router.Send(recvCharaUpdateLvDetailStart, client);


                    router.Send(recvCharaUpdateMaxHp, client);
                    router.Send(recvCharaUpdateMaxMp, client);
                    router.Send(recvCharaUpdateAbilityStr, client);
                    router.Send(recvCharaUpdateAbilityVit, client);
                    router.Send(recvCharaUpdateAbilityDex, client);
                    router.Send(recvCharaUpdateAbilityAgi, client);
                    router.Send(recvCharaUpdateAbilityInt, client);
                    router.Send(recvCharaUpdateAbilityPie, client);
                    router.Send(recvCharaUpdateAbilityLuk, client);

                    router.Send(recvCharaUpdateLv, client);
                    router.Send(recvCharaUpdateLvDetail, client);
                    router.Send(recvCharaUpdateLvDetail2, client);
                    router.Send(recvCharaUpdateLvDetailEnd, client);

                    break;

                case "questworks":
                    RecvQuestGetMissionQuestWorks recvQuestGetMissionQuestWorks = new RecvQuestGetMissionQuestWorks();
                    router.Send(client.map, recvQuestGetMissionQuestWorks);
                    break;

                case "roguehistory":
                    RecvQuestGetRogueMissionQuestHistoryR recvQuestGetRogueMissionQuestHistoryR = new RecvQuestGetRogueMissionQuestHistoryR();
                    router.Send(client.map, recvQuestGetRogueMissionQuestHistoryR);
                    break;

                case "debug":
                    //recv_data_notify_debug_object_data = 0x6510,
                    IBuffer resJ = BufferProvider.Provide();
                    resJ.WriteUInt32(400000221);
                    int numEntries = 0x20;
                    resJ.WriteInt32(numEntries); //less than or equal to 0x20
                    for (int i = 0; i < numEntries; i++)
                    {
                        resJ.WriteFloat(client.character.x + Util.GetRandomNumber(10, 50));
                        resJ.WriteFloat(client.character.y + Util.GetRandomNumber(10, 50));
                        resJ.WriteFloat(client.character.z + Util.GetRandomNumber(10, 50));
                    }

                    resJ.WriteByte(0);
                    resJ.WriteByte(0);
                    resJ.WriteByte(0);
                    router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_debug_object_data, resJ,
                        ServerType.Area);
                    break;

                case "interface":
                    RecvTradeNotifyInterfaceStatus recvTradeNotifyInterfaceStatus = new RecvTradeNotifyInterfaceStatus(x);
                    router.Send(client.map, recvTradeNotifyInterfaceStatus);
                    break;


                default: //you don't know what you're doing do you?
                    _Logger.Error($"There is no recv of type : {command[0]} ");
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
    }
}
