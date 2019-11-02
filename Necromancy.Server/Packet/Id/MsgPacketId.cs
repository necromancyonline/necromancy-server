// ReSharper disable InconsistentNaming

namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Message Server OP Codes
    /// </summary>
    public enum MsgPacketId : ushort
    {
        // Recv OP Codes - Switch: 0x4E4268 - ordered by op code
        recv_0x6C49 = 0x6C94,
        recv_0x8D74 = 0x8D74,
        recv_0x9A9B = 0x9A9B,
        recv_0x95E6 = 0x95E6,
        recv_0x831C = 0x831C,
        recv_0xC8D2 = 0xC8D2,
        recv_0xC991 = 0xC991,
        recv_0xD2D6 = 0xD2D6, //recv_base_ping_r
        recv_base_check_version_r = 0xEFDD,
        recv_base_login_r = 0xA68E,
        recv_cash_buy_premium_r = 0x6FF,
        recv_cash_buy_start_r = 0x9E49,
        recv_cash_get_url_commerce_r = 0x4426,
        recv_cash_get_url_common_r = 0xD6AD,
        recv_cash_get_url_common_steam_r = 0x64B5,
        recv_cash_get_url_r = 0xBE62,
        recv_cash_get_url_web_goods_r = 0x689B,
        recv_cash_update_r = 0xE2BE,
        recv_channel_select_r = 0x58F8,
        recv_chara_create_r = 0xC680,
        recv_chara_delete_r = 0x9AD1,
        recv_chara_draw_bonuspoint_r = 0x6AC0,
        recv_chara_get_createinfo_r = 0xAF33,
        recv_chara_get_inheritinfo_r = 0x1EC8,
        recv_chara_get_list_r = 0x3310,
        recv_chara_notify_data = 0xA57C,
        recv_chara_notify_data_complete = 0x3784,
        recv_chara_select_back_r = 0x1C9,
        recv_chara_select_back_soul_select_r = 0x91EE,
        recv_chara_select_channel_r = 0xA535,
        recv_chara_select_r = 0x482,
        recv_chat_notify_message = 0xC003,
        recv_cpf_authentication = 0xBA73,
        recv_cpf_notify_error = 0x73D7,
        recv_dbg_message = 0x9DE2,
        recv_easy_friend_notify_add_member = 0x7566,
        recv_easy_friend_notify_delete_member = 0xC1E6,
        recv_easy_friend_notify_member_state = 0x1688,
        recv_friend_notify_add_member_r = 0x1817,
        recv_friend_notify_cancel_link = 0x8FB2,
        recv_friend_notify_delete_member = 0xDA1F,
        recv_friend_notify_delete_member_souldelete = 0x7E3A,
        recv_friend_notify_link_invite = 0xBA8B,
        recv_friend_notify_member = 0x7AC9,
        recv_friend_notify_member_state = 0x7205,
        recv_friend_reply_to_link_r = 0x213A,
        recv_friend_request_delete_friend_r = 0x9BFE,
        recv_friend_request_link_target_r = 0xBD6B,
        recv_friend_request_load_r = 0xB7D,
        recv_friend_result_reply_link2 = 0x8BB1,
        recv_party_notify_accept_to_apply = 0x4CD5,
        recv_party_notify_add_member = 0x2C1C,
        recv_party_notify_apply = 0x8D52,
        recv_party_notify_attach_buff = 0x2E98,
        recv_party_notify_cancel_application = 0x32FA,
        recv_party_notify_cancel_invitation = 0xD6ED,
        recv_party_notify_cancel_recruit = 0x5D93,
        recv_party_notify_change_leader = 0x542B,
        recv_party_notify_change_mode = 0xD1A8,
        recv_party_notify_dead = 0x182B,
        recv_party_notify_decline_to_apply = 0x8EEE,
        recv_party_notify_disband = 0x5F9A,
        recv_party_notify_establish = 0xD0AC,
        recv_party_notify_get_item = 0xF7F0,
        recv_party_notify_get_money = 0x1DC4,
        recv_party_notify_invite = 0x8B76,
        recv_party_notify_kick = 0x8E0A,
        recv_party_notify_raise = 0x9AD4,
        recv_party_notify_remove_member = 0x95FE,
        recv_party_notify_update_ac = 0x1421,
        recv_party_notify_update_ap = 0x55FF,
        recv_party_notify_update_body_pos = 0x1D09,
        recv_party_notify_update_deadstate = 0xE51,
        recv_party_notify_update_dragon = 0xFC36,
        recv_party_notify_update_hp = 0xEEB6,
        recv_party_notify_update_job = 0xAD3,
        recv_party_notify_update_level = 0x9C7D,
        recv_party_notify_update_map = 0x4090,
        recv_party_notify_update_maxap = 0x96DB,
        recv_party_notify_update_maxhp = 0x2D92,
        recv_party_notify_update_maxmp = 0xD9D7,
        recv_party_notify_update_mp = 0x1AF3,
        recv_party_notify_update_pos = 0xC87,
        recv_party_notify_update_premium_service_notify_flag = 0x31B4,
        recv_party_notify_update_soulrank = 0xF2C3,
        recv_refusallist_notify_remove_user_souldelete = 0x5386,
        recv_soul_authenticate_passwd_r = 0x5652,
        recv_soul_create_r = 0x763,
        recv_soul_delete_r = 0x5B32,
        recv_soul_rename_r = 0x7C40,
        recv_soul_select_r = 0xC561,
        recv_soul_set_passwd_r = 0xB2B7,
        recv_soul_update_premium_flags = 0x3B93,
        recv_system_notify_announce = 0xF8B4,
        recv_system_register_error_report_r = 0xEB6E,
        recv_union_notify_cancel_invitat = 0x9D6A,
        recv_union_notify_changed_role = 0x7A60,
        recv_union_notify_detail = 0xC5F1,
        recv_union_notify_detail_member = 0x8D24,
        recv_union_notify_disband = 0x7DB1,
        recv_union_notify_expelled_member = 0x7F64,
        recv_union_notify_growth = 0x5B94,
        recv_union_notify_info = 0x755C,
        recv_union_notify_invite = 0xAAB5,
        recv_union_notify_joined_member = 0x5315,
        recv_union_notify_levelup = 0xC8D0,
        recv_union_notify_mantle = 0xA878,
        recv_union_notify_member_state = 0x8D3A,
        recv_union_notify_secede_member = 0xA0E8,
        recv_union_reply_to_invite_r = 0x392F,
        recv_union_request_change_role_r = 0x853F,
        recv_union_request_detail_r = 0x51EB,
        recv_union_request_disband_r = 0x5F0A,
        recv_union_request_expel_member_r = 0xE4DE,
        recv_union_request_invite_target_r = 0x19C,
        recv_union_request_member_priv_r = 0xF5BF,
        recv_union_request_news_r = 0x1935,
        recv_union_request_secede_r = 0x1E68,
        recv_union_request_set_info_r = 0x1C6,
        recv_union_request_set_mantle_r = 0x557F,
        recv_union_result_reply_invitat2 = 0xF94C,

        // Send OP Codes - ordered by op code
        send_base_check_version = 0x5705,
        send_base_login = 0xA53D,
        send_cash_buy_premium = 0x001, // placeholder until found
        send_cash_get_url_common = 0x733E,
        send_cash_get_url_common_steam = 0x002, // placeholder until found
        send_cash_update = 0x003, // placeholder until found
        send_channel_select = 0x2F41,
        send_chara_create = 0xC2B,
        send_chara_delete = 0x9057,
        send_chara_draw_bonuspoint = 0x99EC,
        send_chara_get_createinfo = 0x7E62,
        send_chara_get_inheritinfo = 0x005, // placeholder until found
        send_chara_get_list = 0xF56C,
        send_chara_select = 0x610,
        send_chara_select_back = 0x4772, 
        send_chara_select_back_soul_select = 0x007, // placeholder until found
        send_friend_reply_to_link2 = 0x1852,
        send_friend_request_delete_friend = 0x009, // placeholder until found
        send_friend_request_link_target = 0x470C,
        send_friend_accept_request_link = 0x46FB,
        send_friend_request_load = 0xE1DF,
        send_soul_authenticate_passwd = 0xB4BB,
        send_skill_request_info = 0x4EB5,
        send_soul_create = 0xCE74,
        send_soul_delete = 0x5208,
        send_soul_rename = 0x011, // placeholder until found
        send_soul_select = 0xA869, // 0xC44F, // :0xA869
        send_soul_select_C44F = 0xC44F,
        send_soul_set_passwd = 0x8C9D,
        send_system_register_error_report = 0xC789, 
        send_union_reply_to_invite2 = 0x013, // placeholder until found
        send_union_request_change_role = 0x014, // placeholder until found
        send_union_request_detail = 0x7950,
        send_union_request_disband = 0x015, // placeholder until found
        send_union_request_expel_member = 0x016, // placeholder until found
        send_union_request_invite_target = 0x017, // placeholder until found
        send_union_request_member_priv = 0x018, // placeholder until found
        send_union_request_news = 0x019, // placeholder until found
        send_union_request_secede = 0x020, // placeholder until found
        send_union_request_set_info = 0x021, // placeholder until found
        send_union_request_set_mantle = 0x5088,
        send_comment_set = 0xFC37,
    }
}
