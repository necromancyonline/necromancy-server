/*
 * This file is part of Necromancy.Server
 *
 * Necromancy.Server is a server implementation for the game "Wizardry Online".
 * Copyright (C) 2019-2020 Necromancy Team
 *
 * Github: https://github.com/necromancyonline/necromancy-server
 *
 * Necromancy.Server is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Necromancy.Server is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Necromancy.Server. If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Arrowgene.Networking.Tcp.Server.AsyncEvent;
using Necromancy.Server.Chat;
using Necromancy.Server.Chat.Command.Commands;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Database;
using Necromancy.Server.Discord;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Area;
using Necromancy.Server.Packet.Area.SendChatPostMessage;
using Necromancy.Server.Packet.Area.SendCmdExec;
using Necromancy.Server.Packet.Auth;
using Necromancy.Server.Packet.Custom;
using Necromancy.Server.Packet.Msg;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecServer
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(NecServer));
        private readonly NecQueueConsumer _areaConsumer;
        private readonly AsyncEventServer _areaServer;

        private readonly NecQueueConsumer _authConsumer;
        private readonly AsyncEventServer _authServer;
        private readonly NecQueueConsumer _msgConsumer;
        private readonly AsyncEventServer _msgServer;
        private volatile bool _running;

        public NecServer(NecSetting setting)
        {
            _running = false;
            this.setting = new NecSetting(setting);

            necromancyBot = new NecromancyBot(this.setting);
            necromancyBot.AddSingleton(this);
            instances = new InstanceGenerator(this);
            clients = new ClientLookup();
            maps = new MapLookup();
            chat = new ChatManager(this);
            router = new PacketRouter();
            settingRepository = new SettingRepository(this.setting.repositoryFolder).Initialize();
            database = new NecDatabaseBuilder(this.setting, settingRepository).Build();
            _authConsumer = new NecQueueConsumer(ServerType.Auth, this.setting, this.setting.authSocketSettings);
            _authConsumer.clientDisconnected += AuthClientDisconnected;
            _msgConsumer = new NecQueueConsumer(ServerType.Msg, this.setting, this.setting.msgSocketSettings);
            _msgConsumer.clientDisconnected += MsgClientDisconnected;
            _areaConsumer = new NecQueueConsumer(ServerType.Area, this.setting, this.setting.areaSocketSettings);
            _areaConsumer.clientDisconnected += AreaClientDisconnected;

            _authServer = new AsyncEventServer(
                this.setting.listenIpAddress,
                this.setting.authPort,
                _authConsumer,
                this.setting.authSocketSettings
            );

            _msgServer = new AsyncEventServer(
                this.setting.listenIpAddress,
                this.setting.msgPort,
                _msgConsumer,
                this.setting.msgSocketSettings
            );

            _areaServer = new AsyncEventServer(
                this.setting.listenIpAddress,
                this.setting.areaPort,
                _areaConsumer,
                this.setting.areaSocketSettings
            );

            LoadChatCommands();
            LoadDatabaseObjects();
            LoadHandler();
        }

        public NecSetting setting { get; }
        public PacketRouter router { get; }
        public ClientLookup clients { get; }
        public MapLookup maps { get; }
        public IDatabase database { get; }
        public SettingRepository settingRepository { get; }
        public ChatManager chat { get; }
        public NecromancyBot necromancyBot { get; }
        public InstanceGenerator instances { get; }
        public bool running => _running;

        public void Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            _authServer.Start();
            _msgServer.Start();
            _areaServer.Start();
            _running = true;
            necromancyBot.Start();
            necromancyBot.EnqueueEvent_ServerStatus("Hello! I'm Online!");
        }

        public void Stop()
        {
            necromancyBot.Send_ServerStatus("Bye Byte, I'm Offline");
            _authServer.Stop();
            _msgServer.Stop();
            _areaServer.Stop();
            _running = false;
            necromancyBot.Stop();
            AppDomain.CurrentDomain.UnhandledException -= CurrentDomainOnUnhandledException;
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Stop();
        }

        private void AuthClientDisconnected(NecConnection client)
        {
        }

        private void MsgClientDisconnected(NecConnection client)
        {
        }

        private void AreaClientDisconnected(NecConnection connection)
        {
            NecClient client = connection.client;
            if (client == null) return;
            //Try to update the character stats.
            if (!database.UpdateCharacter(client.character)) _Logger.Error("Could not update the database with character details before disconnect");
            if (!database.UpdateSoul(client.soul)) _Logger.Error("Could not update the database with soul details before disconnect");
            clients.Remove(client);

            //I disconnected while my dead body was being carried around by another player
            if (client.character.hasDied)
            {
                DeadBody deadBody = instances.GetInstance(client.character.deadBodyInstanceId) as DeadBody;
                if (deadBody.salvagerId != 0)
                {
                    NecClient mySalvager = clients.GetByCharacterInstanceId(deadBody.salvagerId);
                    if (mySalvager != null)
                    {
                        deadBody.x = mySalvager.character.x;
                        deadBody.y = mySalvager.character.y;
                        deadBody.z = mySalvager.character.z;
                        deadBody.mapId = mySalvager.character.mapId;
                        deadBody.connectionState = 0;
                        mySalvager.bodyCollection.Remove(deadBody.instanceId);

                        mySalvager.map.deadBodies.Add(deadBody.instanceId, deadBody);
                        RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                        if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                            router.Send(mySalvager.map, cBodyData.ToPacket(), client);

                        //must occur after the charaBody notify.
                        RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(deadBody.instanceId, 5);
                        router.Send(mySalvager, recvCharaBodySalvageEnd.ToPacket());
                    }
                }
            }

            //while i was dead and being carried around, the player carrying me disconnected
            foreach (NecClient collectedBody in client.bodyCollection.Values)
            {
                DeadBody deadBody = instances.GetInstance(collectedBody.character.deadBodyInstanceId) as DeadBody;

                RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(3);
                router.Send(collectedBody, recvCharaBodySelfSalvageEnd.ToPacket());


                deadBody.x = client.character.x;
                deadBody.y = client.character.y;
                deadBody.z = client.character.z;
                collectedBody.character.x = client.character.x;
                collectedBody.character.y = client.character.y;
                collectedBody.character.z = client.character.z;
                //ToDo  add Town checking.  if map.ID.toString()[0]==1 skip deadbody rendering
                deadBody.mapId = client.character.mapId;

                client.map.deadBodies.Add(deadBody.instanceId, deadBody);
                RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                    router.Send(client.map, cBodyData.ToPacket());

                //send your soul to all the other souls runnin around
                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(collectedBody.character, collectedBody.soul.name);
                foreach (NecClient soulStateClient in client.map.clientLookup.GetAll())
                    if (soulStateClient.character.state == CharacterState.SoulForm)
                        router.Send(soulStateClient, cData.ToPacket());
            }

            Map map = client.map;

            //If i was dead, toggle my deadBody to a Rucksack
            if (map != null)
                if (map.deadBodies.ContainsKey(client.character.deadBodyInstanceId))
                {
                    map.deadBodies.TryGetValue(client.character.deadBodyInstanceId, out DeadBody deadBody);
                    deadBody.connectionState = 0;
                    RecvCharaBodyNotifySpirit recvCharaBodyNotifySpirit = new RecvCharaBodyNotifySpirit(client.character.deadBodyInstanceId, (byte)RecvCharaBodyNotifySpirit.ValidSpirit.DisconnectedClient);
                    router.Send(map, recvCharaBodyNotifySpirit.ToPacket());

                    Task.Delay(TimeSpan.FromSeconds(600)).ContinueWith
                    (t1 =>
                        {
                            if (map.deadBodies.ContainsKey(client.character.deadBodyInstanceId))
                            {
                                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.character.deadBodyInstanceId);
                                router.Send(client.map, recvObjectDisappearNotify.ToPacket(), client);
                                map.deadBodies.Remove(client.character.deadBodyInstanceId);
                            }
                        }
                    );
                }

            if (map != null) map.Leave(client);

            Union union = client.union;
            if (union != null) union.Leave(client);

            Character character = client.character;
            if (character != null)
            {
                instances.FreeInstance(character);
                character.characterActive = false;
            }
        }

        private void LoadChatCommands()
        {
            chat.commandHandler.AddCommand(new RTestCommand(this));
            chat.commandHandler.AddCommand(new BattleCommand(this));
            chat.commandHandler.AddCommand(new ArrangeCommand(this));
            chat.commandHandler.AddCommand(new ItemInstanceCommand(this));
            chat.commandHandler.AddCommand(new ItemGeneratorCommand(this));
            chat.commandHandler.AddCommand(new HonorCommand(this));
            chat.commandHandler.AddCommand(new SummonCommand(this));
            chat.commandHandler.AddCommand(new PlayersCommand(this));
            chat.commandHandler.AddCommand(new MapTranCommand(this));
            chat.commandHandler.AddCommand(new GGateCommand(this));
            chat.commandHandler.AddCommand(new GimmickCommand(this));
            chat.commandHandler.AddCommand(new ScriptCommand(this));
            chat.commandHandler.AddCommand(new UnionCommand(this));
            chat.commandHandler.AddCommand(new HelpCommand(this));
            chat.commandHandler.AddCommand(new StatusCommand(this));
            chat.commandHandler.AddCommand(new NpcCommand(this));
            chat.commandHandler.AddCommand(new MonsterCommand(this));
            chat.commandHandler.AddCommand(new ChangeFormMenu(this));
            chat.commandHandler.AddCommand(new Died(this));
            chat.commandHandler.AddCommand(new LogOut(this));
            chat.commandHandler.AddCommand(new OnHit(this));
            chat.commandHandler.AddCommand(new QuestStarted(this));
            chat.commandHandler.AddCommand(new Revive(this));
            chat.commandHandler.AddCommand(new SendAuctionNotifyOpen(this));
            chat.commandHandler.AddCommand(new SendCharacterSave(this));
            chat.commandHandler.AddCommand(new SendDataNotifyItemObjectData(this));
            chat.commandHandler.AddCommand(new SendEventEnd(this));
            chat.commandHandler.AddCommand(new SendEventTreasureboxBegin(this));
            chat.commandHandler.AddCommand(new SendMapChangeForce(this));
            chat.commandHandler.AddCommand(new SendRandomBoxNotifyOpen(this));
            chat.commandHandler.AddCommand(new SendSalvageNotifyBody(this));
            chat.commandHandler.AddCommand(new SendShopNotifyOpen(this));
            chat.commandHandler.AddCommand(new SendSoulStorageEvent(this));
            chat.commandHandler.AddCommand(new SendStallSellItem(this));
            chat.commandHandler.AddCommand(new SendStallUpdateFeatureItem(this));
            chat.commandHandler.AddCommand(new SendTestEvent(this));
            chat.commandHandler.AddCommand(new SendTrapEvent(this));
            chat.commandHandler.AddCommand(new SendWantedJailOpen(this));
            chat.commandHandler.AddCommand(new SendWantedListOpen(this));
            chat.commandHandler.AddCommand(new SoulShop(this));
            chat.commandHandler.AddCommand(new JumpCommand(this));
            chat.commandHandler.AddCommand(new NoStringTestCommand(this));
            chat.commandHandler.AddCommand(new Takeover(this));
            chat.commandHandler.AddCommand(new MobCommand(this));
            chat.commandHandler.AddCommand(new CharaCommand(this));
            chat.commandHandler.AddCommand(new ItemCommand(this));
            chat.commandHandler.AddCommand(new TeleportCommand(this));
            chat.commandHandler.AddCommand(new TeleportToCommand(this));
            chat.commandHandler.AddCommand(new GetCommand(this));
        }

        private void LoadDatabaseObjects()
        {
            List<MapData> maps = database.SelectMaps();
            foreach (MapData mapData in maps)
            {
                Map map = new Map(mapData, this);
                this.maps.Add(map);
            }

            //Logger.Info($"Maps: {maps.Count}");

            //List<Item> items = Database.SelectItems();
            //foreach (Item item in items)
            //{
            //    Items.Add(item.Id, item);
            //}

            //Logger.Info($"Items: {items.Count}");
        }

        private void LoadHandler()
        {
            // Authentication Handler
            _authConsumer.AddHandler(new SendHeartbeat(this));
            _authConsumer.AddHandler(new SendUnknown1(this));
            _authConsumer.AddHandler(new SendBaseCheckVersionAuth(this));
            _authConsumer.AddHandler(new SendBaseAuthenticate(this));
            _authConsumer.AddHandler(new SendBaseGetWorldlist(this));
            _authConsumer.AddHandler(new SendBaseSelectWorld(this));

            // Message Handler
            _msgConsumer.AddHandler(new SendDisconnect(this));
            _msgConsumer.AddHandler(new SendHeartbeat(this));
            _msgConsumer.AddHandler(new SendUnknown1(this));
            _msgConsumer.AddHandler(new SendBaseCheckVersionMsg(this));
            _msgConsumer.AddHandler(new SendBaseLogin(this));
            _msgConsumer.AddHandler(new SendCashBuyPremium(this));
            _msgConsumer.AddHandler(new SendCashGetUrlCommon(this));
            _msgConsumer.AddHandler(new SendCashGetUrlCommonSteam(this));
            _msgConsumer.AddHandler(new SendCashUpdate(this));
            _msgConsumer.AddHandler(new SendChannelSelect(this));
            _msgConsumer.AddHandler(new SendCharaCreate(this));
            _msgConsumer.AddHandler(new SendCharaDelete(this));
            _msgConsumer.AddHandler(new SendCharaDrawBonuspoint(this));
            _msgConsumer.AddHandler(new SendCharaGetCreateinfo(this));
            _msgConsumer.AddHandler(new SendCharaGetInheritinfo(this));
            _msgConsumer.AddHandler(new SendCharaGetList(this));
            _msgConsumer.AddHandler(new SendCharaSelect(this));
            _msgConsumer.AddHandler(new SendCharaSelectBack(this));
            _msgConsumer.AddHandler(new SendCharaSelectBackSoulSelect(this));
            _msgConsumer.AddHandler(new SendFriendReplyToLink2(this));
            _msgConsumer.AddHandler(new SendFriendRequestDeleteFriend(this));
            _msgConsumer.AddHandler(new SendFriendRequestLinkTarget(this));
            _msgConsumer.AddHandler(new SendFriendAcceptRequestLink(this));
            _msgConsumer.AddHandler(new SendFriendRequestLoadMsg(this));
            _msgConsumer.AddHandler(new SendSoulAuthenticatePasswd(this));
            _msgConsumer.AddHandler(new SendSoulCreate(this));
            _msgConsumer.AddHandler(new SendSoulDelete(this));
            _msgConsumer.AddHandler(new SendSoulRename(this));
            _msgConsumer.AddHandler(new SendSoulSelect(this));
            _msgConsumer.AddHandler(new SendSoulSelectC44F(this));
            _msgConsumer.AddHandler(new SendSoulSetPasswd(this));
            _msgConsumer.AddHandler(new SendSystemRegisterErrorReport(this));
            _msgConsumer.AddHandler(new SendSkillRequestInfo(this));
            _msgConsumer.AddHandler(new SendUnionReplyToInvite2(this));
            _msgConsumer.AddHandler(new SendUnionRequestChangeRole(this));
            _msgConsumer.AddHandler(new SendUnionRequestDisband(this));
            _msgConsumer.AddHandler(new SendUnionRequestExpelMember(this));
            _msgConsumer.AddHandler(new SendUnionRequestInviteTarget(this));
            _msgConsumer.AddHandler(new SendUnionRequestMemberPriv(this));
            _msgConsumer.AddHandler(new SendUnionRequestNews(this));
            _msgConsumer.AddHandler(new SendUnionRequestSecede(this));
            _msgConsumer.AddHandler(new SendUnionRequestSetInfo(this));
            _msgConsumer.AddHandler(new SendUnionRequestSetMantle(this));

            // Area Handler
            _areaConsumer.AddHandler(new SendDisconnect(this));
            _areaConsumer.AddHandler(new SendHeartbeat(this));
            _areaConsumer.AddHandler(new SendUnknown1(this));
            _areaConsumer.AddHandler(new SendAuctionBid(this));
            _areaConsumer.AddHandler(new SendAuctionCancelBid(this));
            _areaConsumer.AddHandler(new SendAuctionCancelExhibit(this));
            _areaConsumer.AddHandler(new SendAuctionClose(this));
            _areaConsumer.AddHandler(new SendAuctionExhibit(this));
            _areaConsumer.AddHandler(new SendAuctionSearch(this));
            _areaConsumer.AddHandler(new SendBaseCheckVersionArea(this));
            _areaConsumer.AddHandler(new SendBaseEnter(this));
            _areaConsumer.AddHandler(new SendBattleAttackExec(this));
            _areaConsumer.AddHandler(new SendBattleAttackExecDirect(this));
            _areaConsumer.AddHandler(new SendBattleAttackNext(this));
            _areaConsumer.AddHandler(new SendBattleAttackPose(this));
            _areaConsumer.AddHandler(new SendBattleAttackStart(this));
            _areaConsumer.AddHandler(new SendBattleGuardEnd(this));
            _areaConsumer.AddHandler(new SendBattleGuardStart(this));
            _areaConsumer.AddHandler(new SendBattleReleaseAttackPose(this));
            _areaConsumer.AddHandler(new SendBlacklistClear(this));
            _areaConsumer.AddHandler(new SendBlacklistClose(this));
            _areaConsumer.AddHandler(new SendBlacklistLock(this));
            _areaConsumer.AddHandler(new SendBlacklistOpen(this));
            _areaConsumer.AddHandler(new SendBlacklistUnlock(this));
            _areaConsumer.AddHandler(new SendCashShopOpenByMenu(this));
            _areaConsumer.AddHandler(new SendCharaPoseLadder(this));
            _areaConsumer.AddHandler(new SendCharaPose(this));
            _areaConsumer.AddHandler(new SendCharabodyAccessStart(this));
            _areaConsumer.AddHandler(new SendCharacterViewOffset(this));
            _areaConsumer.AddHandler(new SendChatPostMessageHandler(this));
            _areaConsumer.AddHandler(new SendCmdExecHandler(this));
            _areaConsumer.AddHandler(new SendCommentSet(this));
            _areaConsumer.AddHandler(new SendCommentSwitch(this));
            _areaConsumer.AddHandler(new SendCreatePackage(this));
            _areaConsumer.AddHandler(new SendDataGetSelfCharaDataRequest(this));
            _areaConsumer.AddHandler(new SendEmotionUpdateType(this));
            _areaConsumer.AddHandler(new SendEventAccessObject(this));
            _areaConsumer.AddHandler(new SendEventQuestOrderR(this));
            _areaConsumer.AddHandler(new SendEventRemovetrapEnd(this));
            _areaConsumer.AddHandler(new SendEventRemovetrapSelect(this));
            _areaConsumer.AddHandler(new SendEventRemovetrapSkill(this));
            _areaConsumer.AddHandler(new SendEventRequestIntR(this));
            _areaConsumer.AddHandler(new SendEventScriptPlayR(this));
            _areaConsumer.AddHandler(new SendEventSelectChannelR(this));
            _areaConsumer.AddHandler(new SendEventSelectExecR(this));
            _areaConsumer.AddHandler(new SendEventSoulRankupClose(this));
            _areaConsumer.AddHandler(new SendEventSoulStorageClose(this));
            _areaConsumer.AddHandler(new SendEventSyncR(this));
            _areaConsumer.AddHandler(new SendEventTresureboxEnd(this));
            _areaConsumer.AddHandler(new SendHelpNewRemove(this));
            _areaConsumer.AddHandler(new SendItemDrop(this));
            _areaConsumer.AddHandler(new SendItemEquip(this));
            _areaConsumer.AddHandler(new SendItemMove(this));
            _areaConsumer.AddHandler(new SendItemSort(this));
            _areaConsumer.AddHandler(new SendItemUnequip(this));
            _areaConsumer.AddHandler(new SendLogoutCancelRequest(this));
            _areaConsumer.AddHandler(new SendLogoutStartRequest(this));
            _areaConsumer.AddHandler(new SendLootAccessObject(this));
            _areaConsumer.AddHandler(new SendMapChangeForceR(this));
            _areaConsumer.AddHandler(new SendMapEnter(this));
            _areaConsumer.AddHandler(new SendMapEntry(this));
            _areaConsumer.AddHandler(new SendMapGetInfo(this));
            _areaConsumer.AddHandler(new SendMovementInfo(this));
            _areaConsumer.AddHandler(new SendOpenMailbox(this));
            _areaConsumer.AddHandler(new SendPackageAllDelete(this));
            _areaConsumer.AddHandler(new SendPartyAcceptToInvite(this));
            _areaConsumer.AddHandler(new SendPartyDeclineToInvite(this));
            _areaConsumer.AddHandler(new SendPartyEstablish(this));
            _areaConsumer.AddHandler(new SendPartyInvite(this));
            _areaConsumer.AddHandler(new SendPartyLeave(this));
            _areaConsumer.AddHandler(new SendPartyRegistMemberRecruit(this));
            _areaConsumer.AddHandler(new SendPartyRegistPartyRecruit(this));
            _areaConsumer.AddHandler(new SendPartySearchRecruitedMember(this));
            _areaConsumer.AddHandler(new SendPartySearchRecruitedParty(this));
            _areaConsumer.AddHandler(new SendQuestAbort(this));
            _areaConsumer.AddHandler(new SendQuestCheckTarget(this));
            _areaConsumer.AddHandler(new SendQuestGetMissionQuestHistory(this));
            _areaConsumer.AddHandler(new SendQuestGetSoulMissionQuestHistory(this));
            _areaConsumer.AddHandler(new SendQuestGetStoryQuestHistory(this));
            _areaConsumer.AddHandler(new SendRandomBoxClose(this));
            _areaConsumer.AddHandler(new SendRefusallistAddUser(this));
            _areaConsumer.AddHandler(new SendSelectPackageUpdate(this));
            _areaConsumer.AddHandler(new SendShopClose(this));
            _areaConsumer.AddHandler(new SendShopIdentify(this));
            _areaConsumer.AddHandler(new SendShopSellCheck(this));
            _areaConsumer.AddHandler(new SendShopSell(this));
            _areaConsumer.AddHandler(new SendShortcutRequestRegist(this));
            _areaConsumer.AddHandler(new SendSkillCastCancelRequest(this));
            _areaConsumer.AddHandler(new SendSkillExec(this));
            _areaConsumer.AddHandler(new SendSkillOnhit(this));
            _areaConsumer.AddHandler(new SendSkillRequestGain(this));
            _areaConsumer.AddHandler(new SendSkillStartCast(this));
            _areaConsumer.AddHandler(new SendSoulDispitemRequestData(this));
            _areaConsumer.AddHandler(new SendStallClose(this));
            _areaConsumer.AddHandler(new SendStallDeregistItem(this));
            _areaConsumer.AddHandler(new SendStallOpen(this));
            _areaConsumer.AddHandler(new SendStallRegistItem(this));
            _areaConsumer.AddHandler(new SendStallSetItemPrice(this));
            _areaConsumer.AddHandler(new SendStallSetName(this));
            _areaConsumer.AddHandler(new SendStallShoppingAbort(this));
            _areaConsumer.AddHandler(new SendStallShoppingStart(this));
            _areaConsumer.AddHandler(new SendStorageDepositItem(this));
            _areaConsumer.AddHandler(new SendStorageDepositMoney(this));
            _areaConsumer.AddHandler(new SendStorageDrawMoney(this));
            _areaConsumer.AddHandler(new SendSvConfOptionRequest(this));
            _areaConsumer.AddHandler(new SendTempleClose(this));
            _areaConsumer.AddHandler(new SendTempleCureCurse(this));
            _areaConsumer.AddHandler(new SendTradeAbort(this));
            _areaConsumer.AddHandler(new SendTradeAddItem(this));
            _areaConsumer.AddHandler(new SendTradeFix(this));
            _areaConsumer.AddHandler(new SendTradeInvite(this));
            _areaConsumer.AddHandler(new SendTradeOffer(this));
            _areaConsumer.AddHandler(new SendTradeRemoveItem(this));
            _areaConsumer.AddHandler(new SendTradeReply(this));
            _areaConsumer.AddHandler(new SendTradeRevert(this));
            _areaConsumer.AddHandler(new SendTradeSetMoney(this));
            _areaConsumer.AddHandler(new SendUnionCloseWindow(this));
            _areaConsumer.AddHandler(new SendUnionMantleClose(this));
            _areaConsumer.AddHandler(new SendUnionRequestEstablish(this));
            _areaConsumer.AddHandler(new SendWantedEntry(this));
            _areaConsumer.AddHandler(new SendWantedJailClose(this));
            _areaConsumer.AddHandler(new SendWantedJailDrawPoint(this));
            _areaConsumer.AddHandler(new SendWantedJailPayment(this));
            _areaConsumer.AddHandler(new SendWantedListClose(this));
            //_areaConsumer.AddHandler(new send_gem_close(this));
            _areaConsumer.AddHandler(new SendGetRefusallist(this));
            _areaConsumer.AddHandler(new SendPartyRequestDrawItemList(this));
            _areaConsumer.AddHandler(new SendQuestGetMissionQuestWorks(this));
            _areaConsumer.AddHandler(new SendQuestGetSoulMissionQuestWorks(this));
            _areaConsumer.AddHandler(new SendQuestGetStoryQuestWorks(this));
            _areaConsumer.AddHandler(new SendShortcutRequestData(this));
            _areaConsumer.AddHandler(new SendSkillRequestInfo(this));
            _areaConsumer.AddHandler(new SendSvConfOptionChange(this));
            _areaConsumer.AddHandler(new SendCharabodySelfSalvageNotifyR(this));
            _areaConsumer.AddHandler(new SendReturnHomeRequestExec(this));
            _areaConsumer.AddHandler(new SendEventSelectMapAndChannelR(this));
            _areaConsumer.AddHandler(new SendGimmickAccessObject(this));
            _areaConsumer.AddHandler(new SendDoorOpen(this));
            _areaConsumer.AddHandler(new SendDoorClose(this));
            _areaConsumer.AddHandler(new SendQuestCheckTimeLimit(this));
            _areaConsumer.AddHandler(new SendQuestDisplay(this));
            _areaConsumer.AddHandler(new SendCharabodyAccessAbort(this));
            _areaConsumer.AddHandler(new SendCharabodySalvageRequest(this));
            _areaConsumer.AddHandler(new SendCharabodySalvageRequestCancel(this));
            _areaConsumer.AddHandler(new SendCharabodySalvageAbort(this));
            _areaConsumer.AddHandler(new SendPartyDisband(this));
            _areaConsumer.AddHandler(new SendEventSystemMessageTimerR(this));
            _areaConsumer.AddHandler(new SendRaisescaleOpenCashShop(this));
            _areaConsumer.AddHandler(new SendRaisescaleMoveMoney(this));
            _areaConsumer.AddHandler(new SendRaisescaleViewCloseRequest(this));
            _areaConsumer.AddHandler(new SendRaisescaleAddItem(this));
            _areaConsumer.AddHandler(new SendRaisescaleRequestReviveEvent(this));
            _areaConsumer.AddHandler(new SendRaisescaleRequestRevive(this));
            _areaConsumer.AddHandler(new SendShopRepair(this));
            _areaConsumer.AddHandler(new SendStorageDrawItem(this));
            _areaConsumer.AddHandler(new SendUnionRequestDetail(this)); // ORIGINALLY A MSG SEND
            _areaConsumer.AddHandler(new SendFriendRequestLoadArea(this)); // ORIGINALLY A MSG SEND
            _areaConsumer.AddHandler(new SendPartyEntryDraw(this));
            _areaConsumer.AddHandler(new SendPartyPassDraw(this));
            _areaConsumer.AddHandler(new SendPartyChangeLeader(this));
            _areaConsumer.AddHandler(new SendPartyKick(this));
            _areaConsumer.AddHandler(new SendPartyChangeMode(this));
            _areaConsumer.AddHandler(new SendPartyCancelMemberRecruit(this));
            _areaConsumer.AddHandler(new SendPartyApply(this));
            _areaConsumer.AddHandler(new SendPartyAcceptToApply(this));
            _areaConsumer.AddHandler(new SendPartyDeclineToApply(this));
            _areaConsumer.AddHandler(new SendMessageBoardClose(this));
            _areaConsumer.AddHandler(new SendRefusallistRemoveUser(this));
            _areaConsumer.AddHandler(new SendUnionRequestRename(this));
            _areaConsumer.AddHandler(new SendEventQuestReportListEnd(this));
            _areaConsumer.AddHandler(new SendEventQuestReportSelect(this));
            _areaConsumer.AddHandler(new SendBuffShopBuy(this));
            _areaConsumer.AddHandler(new SendEquipHonor(this));
            _areaConsumer.AddHandler(new SendUpdateHonor(this));
            _areaConsumer.AddHandler(new SendForgeCheck(this));
            _areaConsumer.AddHandler(new SendGemClose(this));
            _areaConsumer.AddHandler(new SendShopBuy(this));
            _areaConsumer.AddHandler(new SendEventUnionStorageClose(this));
            _areaConsumer.AddHandler(new SendUnionStorageDepositMoney(this));
            _areaConsumer.AddHandler(new SendUnionStorageMoveItem(this));
            _areaConsumer.AddHandler(new SendUnionStorageDrawMoney(this));
            _areaConsumer.AddHandler(new SendCharaUpdateBattleTarget(this));
            _areaConsumer.AddHandler(new SendStorageOpenCashShop(this));
            _areaConsumer.AddHandler(new SendEventTreasureboxSelect(this));
            _areaConsumer.AddHandler(new SendJobChangeClose(this));
            _areaConsumer.AddHandler(new SendLoginNewsGetUrl(this));
            _areaConsumer.AddHandler(new SendEcho(this));
            _areaConsumer.AddHandler(new SendSoulPartnerStatusOpen(this));
            _areaConsumer.AddHandler(new SendPartyMentorCreate(this));
            _areaConsumer.AddHandler(new SendPartyMentorRemove(this));
            _areaConsumer.AddHandler(new SendForgeExecute(this));
            _areaConsumer.AddHandler(new SendCharabodyLootStart2(this));
            _areaConsumer.AddHandler(new SendCharabodyLootComplete2(this));
            _areaConsumer.AddHandler(new SendCharabodyLootStart3(this));
            _areaConsumer.AddHandler(new SendCharabodyLootComplete3(this));
            _areaConsumer.AddHandler(new SendCharabodyLootStart2Cancel(this));
            _areaConsumer.AddHandler(new SendCharabodySelfSalvageAbort(this));
            _areaConsumer.AddHandler(new SendAuctionRegistSearchEquipmentCond(this));
            _areaConsumer.AddHandler(new SendAuctionDeregistSearchEquipmentCond(this));
            _areaConsumer.AddHandler(new SendAuctionRegistSearchItemCond(this));
            _areaConsumer.AddHandler(new SendAuctionDeregistSearchItemCond(this));
        }
    }
}
