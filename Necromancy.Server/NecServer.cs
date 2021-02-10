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
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Model.Union;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Area;
using Necromancy.Server.Packet.Area.SendChatPostMessage;
using Necromancy.Server.Packet.Area.SendCmdExec;
using Necromancy.Server.Packet.Auth;
using Necromancy.Server.Packet.Custom;
using Necromancy.Server.Packet.Msg;
using Necromancy.Server.Setting;

namespace Necromancy.Server
{
    public class NecServer
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(NecServer));

        public NecSetting Setting { get; }
        public PacketRouter Router { get; }
        public ClientLookup Clients { get; }
        public MapLookup Maps { get; }
        public IDatabase Database { get; }
        public SettingRepository SettingRepository { get; }
        public ChatManager Chat { get; }
        public NecromancyBot NecromancyBot { get; }
        public InstanceGenerator Instances { get; }
        public bool Running => _running;

        private readonly NecQueueConsumer _authConsumer;
        private readonly NecQueueConsumer _msgConsumer;
        private readonly NecQueueConsumer _areaConsumer;
        private readonly AsyncEventServer _authServer;
        private readonly AsyncEventServer _msgServer;
        private readonly AsyncEventServer _areaServer;
        private volatile bool _running;

        public NecServer(NecSetting setting)
        {
            _running = false;
            Setting = new NecSetting(setting);

            NecromancyBot = new NecromancyBot(Setting);
            NecromancyBot.AddSingleton(this);
            Instances = new InstanceGenerator(this);
            Clients = new ClientLookup();
            Maps = new MapLookup();
            Chat = new ChatManager(this);
            Router = new PacketRouter();
            SettingRepository = new SettingRepository(Setting.RepositoryFolder).Initialize();
            Database = new NecDatabaseBuilder(Setting, SettingRepository).Build();
            _authConsumer = new NecQueueConsumer(ServerType.Auth, Setting, Setting.AuthSocketSettings);
            _authConsumer.ClientDisconnected += AuthClientDisconnected;
            _msgConsumer = new NecQueueConsumer(ServerType.Msg, Setting, Setting.MsgSocketSettings);
            _msgConsumer.ClientDisconnected += MsgClientDisconnected;
            _areaConsumer = new NecQueueConsumer(ServerType.Area, Setting, Setting.AreaSocketSettings);
            _areaConsumer.ClientDisconnected += AreaClientDisconnected;

            _authServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AuthPort,
                _authConsumer,
                Setting.AuthSocketSettings
            );

            _msgServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.MsgPort,
                _msgConsumer,
                Setting.MsgSocketSettings
            );

            _areaServer = new AsyncEventServer(
                Setting.ListenIpAddress,
                Setting.AreaPort,
                _areaConsumer,
                Setting.AreaSocketSettings
            );

            LoadChatCommands();
            LoadDatabaseObjects();
            LoadHandler();
        }

        public void Start()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            _authServer.Start();
            _msgServer.Start();
            _areaServer.Start();
            _running = true;
            NecromancyBot.Start();
            NecromancyBot.EnqueueEvent_ServerStatus("Hello! I'm Online!");
        }

        public void Stop()
        {
            NecromancyBot.Send_ServerStatus("Bye Byte, I'm Offline");
            _authServer.Stop();
            _msgServer.Stop();
            _areaServer.Stop();
            _running = false;
            NecromancyBot.Stop();
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
            NecClient client = connection.Client;
            if (client == null)
            {
                return;
            }

            Clients.Remove(client);

            Map map = client.Map;
            if (map != null)
            {
                map.Leave(client);
            }

            Union union = client.Union;
            if (union != null)
            {
                union.Leave(client);
            }

            Character character = client.Character;
            if (character != null)
            {
                Instances.FreeInstance(character);
                character.characterActive = false;
            }
        }

        private void LoadChatCommands()
        {
            Chat.CommandHandler.AddCommand(new RTestCommand(this));
            Chat.CommandHandler.AddCommand(new BattleCommand(this));
            Chat.CommandHandler.AddCommand(new ArrangeCommand(this));
            Chat.CommandHandler.AddCommand(new ItemInstanceCommand(this));
            Chat.CommandHandler.AddCommand(new ItemGeneratorCommand(this));
            Chat.CommandHandler.AddCommand(new HonorCommand(this));
            Chat.CommandHandler.AddCommand(new SummonCommand(this));
            Chat.CommandHandler.AddCommand(new PlayersCommand(this));
            Chat.CommandHandler.AddCommand(new MapTranCommand(this));
            Chat.CommandHandler.AddCommand(new GGateCommand(this));
            Chat.CommandHandler.AddCommand(new GimmickCommand(this));
            Chat.CommandHandler.AddCommand(new ScriptCommand(this));
            Chat.CommandHandler.AddCommand(new UnionCommand(this));
            Chat.CommandHandler.AddCommand(new HelpCommand(this));
            Chat.CommandHandler.AddCommand(new StatusCommand(this));
            Chat.CommandHandler.AddCommand(new NpcCommand(this));
            Chat.CommandHandler.AddCommand(new MonsterCommand(this));
            Chat.CommandHandler.AddCommand(new AdminConsoleRecvItemInstance(this));
            Chat.CommandHandler.AddCommand(new AdminConsoleRecvItemInstanceUnidentified(this));
            Chat.CommandHandler.AddCommand(new ChangeFormMenu(this));
            Chat.CommandHandler.AddCommand(new Died(this));
            Chat.CommandHandler.AddCommand(new LogOut(this));
            Chat.CommandHandler.AddCommand(new OnHit(this));
            Chat.CommandHandler.AddCommand(new QuestStarted(this));
            Chat.CommandHandler.AddCommand(new Revive(this));
            Chat.CommandHandler.AddCommand(new SendAuctionNotifyOpen(this));
            Chat.CommandHandler.AddCommand(new SendCharacterSave(this));
            Chat.CommandHandler.AddCommand(new SendDataNotifyItemObjectData(this));
            Chat.CommandHandler.AddCommand(new SendEventEnd(this));
            Chat.CommandHandler.AddCommand(new SendEventTreasureboxBegin(this));
            Chat.CommandHandler.AddCommand(new SendMapChangeForce(this));
            Chat.CommandHandler.AddCommand(new SendRandomBoxNotifyOpen(this));
            Chat.CommandHandler.AddCommand(new SendSalvageNotifyBody(this));
            Chat.CommandHandler.AddCommand(new SendShopNotifyOpen(this));
            Chat.CommandHandler.AddCommand(new SendSoulStorageEvent(this));
            Chat.CommandHandler.AddCommand(new SendStallSellItem(this));
            Chat.CommandHandler.AddCommand(new SendStallUpdateFeatureItem(this));
            Chat.CommandHandler.AddCommand(new SendTestEvent(this));
            Chat.CommandHandler.AddCommand(new SendTrapEvent(this));
            Chat.CommandHandler.AddCommand(new SendWantedJailOpen(this));
            Chat.CommandHandler.AddCommand(new SendWantedListOpen(this));
            Chat.CommandHandler.AddCommand(new SoulShop(this));
            Chat.CommandHandler.AddCommand(new JumpCommand(this));
            Chat.CommandHandler.AddCommand(new NoStringTestCommand(this));
            Chat.CommandHandler.AddCommand(new Takeover(this));
            Chat.CommandHandler.AddCommand(new MobCommand(this));
            Chat.CommandHandler.AddCommand(new CharaCommand(this));
            Chat.CommandHandler.AddCommand(new ItemCommand(this));
            // Chat.CommandHandler.AddCommand(new BagCommand(this));
            Chat.CommandHandler.AddCommand(new TeleportCommand(this));
            Chat.CommandHandler.AddCommand(new TeleportToCommand(this));
            Chat.CommandHandler.AddCommand(new GetCommand(this));
        }

        private void LoadDatabaseObjects()
        {
            List<MapData> maps = Database.SelectMaps();
            foreach (MapData mapData in maps)
            {
                Map map = new Map(mapData, this);
                Maps.Add(map);
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
            _authConsumer.AddHandler(new send_base_check_version_auth(this));
            _authConsumer.AddHandler(new send_base_authenticate(this));
            _authConsumer.AddHandler(new send_base_get_worldlist(this));
            _authConsumer.AddHandler(new send_base_select_world(this));

            // Message Handler
            _msgConsumer.AddHandler(new SendDisconnect(this));
            _msgConsumer.AddHandler(new SendHeartbeat(this));
            _msgConsumer.AddHandler(new SendUnknown1(this));
            _msgConsumer.AddHandler(new send_base_check_version_msg(this));
            _msgConsumer.AddHandler(new send_base_login(this));
            _msgConsumer.AddHandler(new send_cash_buy_premium(this));
            _msgConsumer.AddHandler(new send_cash_get_url_common(this));
            _msgConsumer.AddHandler(new send_cash_get_url_common_steam(this));
            _msgConsumer.AddHandler(new send_cash_update(this));
            _msgConsumer.AddHandler(new send_channel_select(this));
            _msgConsumer.AddHandler(new send_chara_create(this));
            _msgConsumer.AddHandler(new send_chara_delete(this));
            _msgConsumer.AddHandler(new send_chara_draw_bonuspoint(this));
            _msgConsumer.AddHandler(new send_chara_get_createinfo(this));
            _msgConsumer.AddHandler(new send_chara_get_inheritinfo(this));
            _msgConsumer.AddHandler(new send_chara_get_list(this));
            _msgConsumer.AddHandler(new send_chara_select(this));
            _msgConsumer.AddHandler(new send_chara_select_back(this));
            _msgConsumer.AddHandler(new send_chara_select_back_soul_select(this));
            _msgConsumer.AddHandler(new send_friend_reply_to_link2(this));
            _msgConsumer.AddHandler(new send_friend_request_delete_friend(this));
            _msgConsumer.AddHandler(new send_friend_request_link_target(this));
            _msgConsumer.AddHandler(new send_friend_accept_request_link(this));
            _msgConsumer.AddHandler(new send_friend_request_load_msg(this));
            _msgConsumer.AddHandler(new send_soul_authenticate_passwd(this));
            _msgConsumer.AddHandler(new send_soul_create(this));
            _msgConsumer.AddHandler(new send_soul_delete(this));
            _msgConsumer.AddHandler(new send_soul_rename(this));
            _msgConsumer.AddHandler(new send_soul_select(this));
            _msgConsumer.AddHandler(new send_soul_select_C44F(this));
            _msgConsumer.AddHandler(new send_soul_set_passwd(this));
            _msgConsumer.AddHandler(new send_system_register_error_report(this));
            _msgConsumer.AddHandler(new send_skill_request_info(this));
            _msgConsumer.AddHandler(new send_union_reply_to_invite2(this));
            _msgConsumer.AddHandler(new send_union_request_change_role(this));
            _msgConsumer.AddHandler(new send_union_request_disband(this));
            _msgConsumer.AddHandler(new send_union_request_expel_member(this));
            _msgConsumer.AddHandler(new send_union_request_invite_target(this));
            _msgConsumer.AddHandler(new send_union_request_member_priv(this));
            _msgConsumer.AddHandler(new send_union_request_news(this));
            _msgConsumer.AddHandler(new send_union_request_secede(this));
            _msgConsumer.AddHandler(new send_union_request_set_info(this));
            _msgConsumer.AddHandler(new send_union_request_set_mantle(this));

            // Area Handler
            _areaConsumer.AddHandler(new SendDisconnect(this));
            _areaConsumer.AddHandler(new SendHeartbeat(this));
            _areaConsumer.AddHandler(new SendUnknown1(this));
            _areaConsumer.AddHandler(new send_auction_bid(this));
            _areaConsumer.AddHandler(new send_auction_cancel_bid(this));
            _areaConsumer.AddHandler(new send_auction_cancel_exhibit(this));
            _areaConsumer.AddHandler(new send_auction_close(this));
            _areaConsumer.AddHandler(new send_auction_exhibit(this));
            _areaConsumer.AddHandler(new send_auction_search(this));
            _areaConsumer.AddHandler(new send_base_check_version_area(this));
            _areaConsumer.AddHandler(new send_base_enter(this));
            _areaConsumer.AddHandler(new send_battle_attack_exec(this));
            _areaConsumer.AddHandler(new send_battle_attack_exec_direct(this));
            _areaConsumer.AddHandler(new send_battle_attack_next(this));
            _areaConsumer.AddHandler(new send_battle_attack_pose(this));
            _areaConsumer.AddHandler(new send_battle_attack_start(this));
            _areaConsumer.AddHandler(new send_battle_guard_end(this));
            _areaConsumer.AddHandler(new send_battle_guard_start(this));
            _areaConsumer.AddHandler(new send_battle_release_attack_pose(this));
            _areaConsumer.AddHandler(new send_blacklist_clear(this));
            _areaConsumer.AddHandler(new send_blacklist_close(this));
            _areaConsumer.AddHandler(new send_blacklist_lock(this));
            _areaConsumer.AddHandler(new send_blacklist_open(this));
            _areaConsumer.AddHandler(new send_blacklist_unlock(this));
            _areaConsumer.AddHandler(new send_cash_shop_open_by_menu(this));
            _areaConsumer.AddHandler(new send_chara_pose_ladder(this));
            _areaConsumer.AddHandler(new send_chara_pose(this));
            _areaConsumer.AddHandler(new send_charabody_access_start(this));
            _areaConsumer.AddHandler(new send_character_view_offset(this));
            _areaConsumer.AddHandler(new SendChatPostMessageHandler(this));
            _areaConsumer.AddHandler(new SendCmdExecHandler(this));
            _areaConsumer.AddHandler(new send_comment_set(this));
            _areaConsumer.AddHandler(new send_comment_switch(this));
            _areaConsumer.AddHandler(new send_create_package(this));
            _areaConsumer.AddHandler(new send_data_get_self_chara_data_request(this));
            _areaConsumer.AddHandler(new send_emotion_update_type(this));
            _areaConsumer.AddHandler(new send_event_access_object(this));
            _areaConsumer.AddHandler(new send_event_quest_order_r(this));
            _areaConsumer.AddHandler(new send_event_removetrap_end(this));
            _areaConsumer.AddHandler(new send_event_removetrap_select(this));
            _areaConsumer.AddHandler(new send_event_removetrap_skill(this));
            _areaConsumer.AddHandler(new send_event_request_int_r(this));
            _areaConsumer.AddHandler(new send_event_script_play_r(this));
            _areaConsumer.AddHandler(new send_event_select_channel_r(this));
            _areaConsumer.AddHandler(new send_event_select_exec_r(this));
            _areaConsumer.AddHandler(new send_event_soul_rankup_close(this));
            _areaConsumer.AddHandler(new Send_event_soul_storage_close(this));
            _areaConsumer.AddHandler(new send_event_sync_r(this));
            _areaConsumer.AddHandler(new send_event_tresurebox_end(this));
            _areaConsumer.AddHandler(new send_help_new_remove(this));
            _areaConsumer.AddHandler(new send_item_drop(this));
            _areaConsumer.AddHandler(new send_item_equip(this));
            _areaConsumer.AddHandler(new send_item_move(this));
            _areaConsumer.AddHandler(new send_item_sort(this));
            _areaConsumer.AddHandler(new send_item_unequip(this));
            _areaConsumer.AddHandler(new send_logout_cancel_request(this));
            _areaConsumer.AddHandler(new send_logout_start_request(this));
            _areaConsumer.AddHandler(new send_loot_access_object(this));
            _areaConsumer.AddHandler(new send_map_change_force_r(this));
            _areaConsumer.AddHandler(new send_map_enter(this));
            _areaConsumer.AddHandler(new send_map_entry(this));
            _areaConsumer.AddHandler(new send_map_get_info(this));
            _areaConsumer.AddHandler(new send_movement_info(this));
            _areaConsumer.AddHandler(new send_open_mailbox(this));
            _areaConsumer.AddHandler(new send_package_all_delete(this));
            _areaConsumer.AddHandler(new send_party_accept_to_invite(this));
            _areaConsumer.AddHandler(new send_party_decline_to_invite(this));
            _areaConsumer.AddHandler(new send_party_establish(this));
            _areaConsumer.AddHandler(new send_party_invite(this));
            _areaConsumer.AddHandler(new send_party_leave(this));
            _areaConsumer.AddHandler(new send_party_regist_member_recruit(this));
            _areaConsumer.AddHandler(new send_party_regist_party_recruit(this));
            _areaConsumer.AddHandler(new send_party_search_recruited_member(this));
            _areaConsumer.AddHandler(new send_party_search_recruited_party(this));
            _areaConsumer.AddHandler(new send_quest_abort(this));
            _areaConsumer.AddHandler(new send_quest_check_target(this));
            _areaConsumer.AddHandler(new send_quest_get_mission_quest_history(this));
            _areaConsumer.AddHandler(new send_quest_get_soul_mission_quest_history(this));
            _areaConsumer.AddHandler(new send_quest_get_story_quest_history(this));
            _areaConsumer.AddHandler(new send_random_box_close(this));
            _areaConsumer.AddHandler(new send_refusallist_add_user(this));
            _areaConsumer.AddHandler(new send_select_package_update(this));
            _areaConsumer.AddHandler(new send_shop_close(this));
            _areaConsumer.AddHandler(new send_shop_identify(this));
            _areaConsumer.AddHandler(new send_shop_sell_check(this));
            _areaConsumer.AddHandler(new send_shop_sell(this));
            _areaConsumer.AddHandler(new send_shortcut_request_regist(this));
            _areaConsumer.AddHandler(new send_skill_cast_cancel_request(this));
            _areaConsumer.AddHandler(new send_skill_exec(this));
            _areaConsumer.AddHandler(new send_skill_onhit(this));
            _areaConsumer.AddHandler(new send_skill_request_gain(this));
            _areaConsumer.AddHandler(new send_skill_start_cast(this));
            _areaConsumer.AddHandler(new send_soul_dispitem_request_data(this));
            _areaConsumer.AddHandler(new send_stall_close(this));
            _areaConsumer.AddHandler(new send_stall_deregist_item(this));
            _areaConsumer.AddHandler(new send_stall_open(this));
            _areaConsumer.AddHandler(new send_stall_regist_item(this));
            _areaConsumer.AddHandler(new send_stall_set_item_price(this));
            _areaConsumer.AddHandler(new send_stall_set_name(this));
            _areaConsumer.AddHandler(new send_stall_shopping_abort(this));
            _areaConsumer.AddHandler(new send_stall_shopping_start(this));
            _areaConsumer.AddHandler(new send_storage_deposit_item(this));
            _areaConsumer.AddHandler(new send_storage_deposit_money(this));
            _areaConsumer.AddHandler(new send_storage_draw_money(this));
            _areaConsumer.AddHandler(new send_sv_conf_option_request(this));
            _areaConsumer.AddHandler(new send_temple_close(this));
            _areaConsumer.AddHandler(new send_temple_cure_curse(this));
            _areaConsumer.AddHandler(new send_trade_abort(this));
            _areaConsumer.AddHandler(new send_trade_add_item(this));
            _areaConsumer.AddHandler(new send_trade_fix(this));
            _areaConsumer.AddHandler(new send_trade_invite(this));
            _areaConsumer.AddHandler(new send_trade_offer(this));
            _areaConsumer.AddHandler(new send_trade_remove_item(this));
            _areaConsumer.AddHandler(new send_trade_reply(this));
            _areaConsumer.AddHandler(new send_trade_revert(this));
            _areaConsumer.AddHandler(new send_trade_set_money(this));
            _areaConsumer.AddHandler(new send_union_close_window(this));
            _areaConsumer.AddHandler(new send_union_mantle_close(this));
            _areaConsumer.AddHandler(new send_union_request_establish(this));
            _areaConsumer.AddHandler(new send_wanted_entry(this));
            _areaConsumer.AddHandler(new send_wanted_jail_close(this));
            _areaConsumer.AddHandler(new send_wanted_jail_draw_point(this));
            _areaConsumer.AddHandler(new send_wanted_jail_payment(this));
            _areaConsumer.AddHandler(new send_wanted_list_close(this));
            //_areaConsumer.AddHandler(new send_gem_close(this));  
            _areaConsumer.AddHandler(new send_get_refusallist(this));
            _areaConsumer.AddHandler(new send_party_request_draw_item_list(this));
            _areaConsumer.AddHandler(new send_quest_get_mission_quest_works(this));
            _areaConsumer.AddHandler(new send_quest_get_soul_mission_quest_works(this));
            _areaConsumer.AddHandler(new send_quest_get_story_quest_works(this));
            _areaConsumer.AddHandler(new send_shortcut_request_data(this));
            _areaConsumer.AddHandler(new send_skill_request_info(this));
            _areaConsumer.AddHandler(new send_sv_conf_option_change(this));
            _areaConsumer.AddHandler(new send_charabody_self_salvage_notify_r(this));
            _areaConsumer.AddHandler(new send_return_home_request_exec(this));
            _areaConsumer.AddHandler(new send_event_select_map_and_channel_r(this));
            _areaConsumer.AddHandler(new send_gimmick_access_object(this));
            _areaConsumer.AddHandler(new send_door_open(this));
            _areaConsumer.AddHandler(new send_door_close(this));
            _areaConsumer.AddHandler(new send_quest_check_time_limit(this));
            _areaConsumer.AddHandler(new send_quest_display(this));
            _areaConsumer.AddHandler(new send_charabody_access_abort(this));
            _areaConsumer.AddHandler(new send_charabody_salvage_request(this));
            _areaConsumer.AddHandler(new send_charabody_salvage_request_cancel(this));
            _areaConsumer.AddHandler(new send_charabody_salvage_abort(this));
            _areaConsumer.AddHandler(new send_party_disband(this));
            _areaConsumer.AddHandler(new send_event_system_message_timer_r(this));
            _areaConsumer.AddHandler(new send_raisescale_open_cash_shop(this));
            _areaConsumer.AddHandler(new send_raisescale_move_money(this));
            _areaConsumer.AddHandler(new send_raisescale_view_close_request(this));
            _areaConsumer.AddHandler(new send_raisescale_add_item(this));
            _areaConsumer.AddHandler(new send_raisescale_request_revive_event(this));
            _areaConsumer.AddHandler(new send_raisescale_request_revive(this));
            _areaConsumer.AddHandler(new send_shop_repair(this));
            _areaConsumer.AddHandler(new send_storage_draw_item(this));
            _areaConsumer.AddHandler(new send_union_request_detail(this)); // ORIGINALLY A MSG SEND
            _areaConsumer.AddHandler(new send_friend_request_load_area(this)); // ORIGINALLY A MSG SEND
            _areaConsumer.AddHandler(new send_party_entry_draw(this));
            _areaConsumer.AddHandler(new send_party_pass_draw(this));
            _areaConsumer.AddHandler(new send_party_change_leader(this));
            _areaConsumer.AddHandler(new send_party_kick(this));
            _areaConsumer.AddHandler(new send_party_change_mode(this));
            _areaConsumer.AddHandler(new send_party_cancel_member_recruit(this));
            _areaConsumer.AddHandler(new send_party_apply(this));
            _areaConsumer.AddHandler(new send_party_accept_to_apply(this));
            _areaConsumer.AddHandler(new send_party_decline_to_apply(this));
            _areaConsumer.AddHandler(new send_message_board_close(this));
            _areaConsumer.AddHandler(new send_refusallist_remove_user(this));
            _areaConsumer.AddHandler(new send_union_request_rename(this));
            _areaConsumer.AddHandler(new send_event_quest_report_list_end(this));
            _areaConsumer.AddHandler(new send_event_quest_report_select(this));
            _areaConsumer.AddHandler(new send_buff_shop_buy(this));
            _areaConsumer.AddHandler(new send_equip_honor(this));
            _areaConsumer.AddHandler(new send_update_honor(this));
            _areaConsumer.AddHandler(new send_forge_check(this));
            _areaConsumer.AddHandler(new send_gem_close(this));
            _areaConsumer.AddHandler(new send_shop_buy(this));
            _areaConsumer.AddHandler(new send_event_union_storage_close(this));
            _areaConsumer.AddHandler(new send_union_storage_deposit_money(this));
            _areaConsumer.AddHandler(new send_union_storage_move_item(this));
            _areaConsumer.AddHandler(new send_union_storage_draw_money(this));
            _areaConsumer.AddHandler(new send_chara_update_battle_target(this));
            _areaConsumer.AddHandler(new send_storage_open_cash_shop(this));
            _areaConsumer.AddHandler(new send_event_treasurebox_select(this));
            _areaConsumer.AddHandler(new send_job_change_close(this));
            _areaConsumer.AddHandler(new send_login_news_get_url(this));
            _areaConsumer.AddHandler(new send_echo(this));
            _areaConsumer.AddHandler(new send_soul_partner_status_open(this));
            _areaConsumer.AddHandler(new send_party_mentor_create(this));
            _areaConsumer.AddHandler(new send_party_mentor_remove(this));
            _areaConsumer.AddHandler(new send_forge_execute(this));
        }
    }
}
