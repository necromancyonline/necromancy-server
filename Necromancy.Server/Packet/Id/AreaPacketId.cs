// ReSharper disable InconsistentNaming
namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    /// Necromancy Area Server OP Codes
    ///
    /// 0x86EA
    /// 0x3DD0 - proto_area_implement_client::recv_event_soul_storage_open
    /// 0x1F5A - proto_area_implement_client::recv_item_update_durability
    /// 0x10DA
    /// 0x794 - proto_area_implement_client::recv_wanted_jail_update_money
    /// 0x494 - proto_area_implement_client::recv_event_tresurebox_select_r
    /// 0x2FA - proto_area_implement_client::recv_charabody_notify_loot_start_cancel
    /// 0x1DA - proto_area_implement_client::recv_sv_conf_option_request_r
    /// 0x1A6 - proto_area_implement_client::recv_cash_shop_get_url_common_r
    /// 0x1AB (0x1A6 + 0x5)
    /// 0xC24E - proto_area_implement_client::recv_chara_update_action_prohibit_camp
    /// 0xA7BF - proto_area_implement_client::recv_party_regist_member_recruit_r
    /// 0x9899
    /// 0x903A - proto_area_implement_client::recv_skill_request_gain_r
    /// 0x8C2F - proto_area_implement_client::recv_chara_update_notify_honor
    /// 0x88FB - proto_area_implement_client::recv_trade_abort_r
    /// 0x8820 - proto_area_implement_client::recv_battle_report_action_removetrap_ontrap
    /// 0x8778 - proto_area_implement_client::recv_union_storage_deposit_money_r
    /// 0x877C (0x8778 + 0x4)
    /// 0xDE90 - proto_area_implement_client::recv_shop_notify_sell_surrogate_fee
    /// 0xD170 - proto_area_implement_client::recv_select_package_update_r
    /// 0xCB6D - proto_area_implement_client::recv_party_notify_failed_draw
    /// 0xC54F - proto_area_implement_client::recv_wanted_jail_draw_point_r
    /// 0xC3EE - proto_area_implement_client::recv_cash_shop2_notify_item
    /// 0xC2A1
    /// 0xEFA4 - proto_area_implement_client::recv_trade_invite_r
    /// 0xE819 - proto_area_implement_client::recv_item_update_spirit_eqmask
    /// 0xE1F8 - proto_area_implement_client::recv_event_select_ready
    /// 0xE051 - proto_area_implement_client::recv_data_notify_goldobject_data
    /// 0xDF31 - proto_area_implement_client::recv_blacklist_open_r
    /// 0xDEB7 - proto_area_implement_client::recv_cash_shop_fitting_end
    /// 0xDEC2 (0xDEB7 + 0xB)
    /// 0xFA0B
    /// 0xF447 - proto_area_implement_client::recv_shop_notify_item
    /// 0xF1A0 - proto_area_implement_client::recv_battle_attack_exec_r
    /// 0xEFDD - recv_base_check_version_r
    /// 0xFDB2 - proto_area_implement_client::recv_package_all_delete_r
    /// 0xFCC0 - proto_area_implement_client::recv_return_home_request_exec_r
    /// 0xFC28 - proto_area_implement_client::recv_auction_cancel_exhibit_r
    /// 0xFB79
    /// 0xFC1A - proto_area_implement_client::recv_job_change_close_r
    /// 0xFED8 - proto_area_implement_client::recv_cloak_notify_open
    /// 0xFE2F - proto_area_implement_client::recv_party_leave_r
    /// 0xFDB8
    /// 0xFDE9 (0xFDB8 + 0x31)
    /// 0xFF00
    /// 0xFEB7 - proto_area_implement_client::recv_event_removetrap_select_r
    /// 0xFCF3
    /// 0xFC75 - proto_area_implement_client::recv_battle_report_start_notify
    /// 0xFCAD (0xFC75 + 0x38)
    /// 0xF71C - proto_area_implement_client::recv_chara_pose_r
    /// 0xF633 - proto_area_implement_client::recv_battle_attack_pose_end_notify
    /// 0xF495 - proto_area_implement_client::recv_party_search_recruited_member_r
    /// 0xF602 - proto_area_implement_client::recv_raisescale_add_item_r
    /// 0xF95B - proto_area_implement_client::recv_emotion_notify_type
    /// 0xF7E7
    /// 0xF7F0 (0xF7E7 + 0x9)
    /// 0xF9F9 - proto_area_implement_client::recv_map_entry_r
    /// 0xF6AF - proto_area_implement_client::recv_npc_affection_rank_update_notify
    /// 0xF393 - proto_area_implement_client::recv_battle_report_notify_damage_ac
    /// 0xF212 - proto_area_implement_client::recv_battle_report_action_attack_onhit
    /// 0xF330 - proto_area_implement_client::recv_cloak_close_r
    /// 0xF3A1 - proto_area_implement_client::recv_inherit_start_r
    /// 0xEDA6 - proto_area_implement_client::recv_map_get_info_r
    /// 0xEA1C - proto_area_implement_client::recv_dbg_chara_unequipped
    /// 0xE897
    /// 0xEE43 - proto_area_implement_client::recv_skill_start_item_cast_r
    /// 0xEDB3
    /// 0xEEB7
    /// 0xECBA - proto_area_implement_client::recv_shop_identify_r
    /// 0xEB47 - proto_area_implement_client::recv_battle_report_notify_exp_bonus2
    /// 0xEB7B (0xEB47 + 0x34)
    /// 0xED4C
    /// 0xE748 - proto_area_implement_client::recv_party_notify_add_draw_item
    /// 0xE4B2 - proto_area_implement_client::recv_skill_start_cast_r
    /// 0xE207 - proto_area_implement_client::recv_party_kick_r
    /// 0xE462 - proto_area_implement_client::recv_union_request_rename_r
    /// 0xE7BB
    /// 0xE526 - proto_area_implement_client::recv_battle_report_notify_exp
    /// 0xE5FF - proto_area_implement_client::recv_quest_select_error
    /// 0xE07E
    /// 0xE039 - proto_area_implement_client::recv_shop_notify_item_sell_price
    /// 0xE03D (0xE039 + 0x4)
    /// 0xD688 - proto_area_implement_client::recv_charabody_salvage_end
    /// 0xD46E - proto_area_implement_client::recv_quest_started
    /// 0xD349 - proto_area_implement_client::recv_map_fragment_flag
    /// 0xD1CB - proto_area_implement_client::recv_auction_close_r
    /// 0xD1A9 - proto_area_implement_client::recv_random_box_next_open_r
    /// 0xD1BD (0xD1A9 + 0x14)
    /// 0xDA4A
    /// 0xD7D8 - proto_area_implement_client::recv_battle_report_notify_invalid_target
    /// 0xD68C
    /// 0xDBF1 - proto_area_implement_client::recv_cash_shop_fitting_begin
    /// 0xDB5E - proto_area_implement_client::recv_battle_report_noact_notify_buff_effect
    /// 0xDA5C - proto_area_implement_client::recv_base_exitr
    /// 0xDB53 - proto_area_implement_client::recv_battle_report_action_removetrap_success
    /// 0xDD52 - proto_area_implement_client::recv_wanted_update_state
    /// 0xDC5B - proto_area_implement_client::recv_battle_report_action_steal_unidentified
    /// 0xDCB6 (0xDC5B + 0x5B)
    /// 0xDDD3 - proto_area_implement_client::recv_wanted_entry_r
    /// 0xDB88 - proto_area_implement_client::recv_charabody_self_salvage_request_cancel
    /// 0xD909
    /// 0xD804 - proto_area_implement_client::recv_gimmick_access_object_notify
    /// 0xD8D5 - proto_area_implement_client::recv_battle_report_noact_notify_heal_mp
    /// 0xD972 - proto_area_implement_client::recv_chara_pose_ladderr
    /// 0xD597 - proto_area_implement_client::recv_cash_shop_notify_item
    /// 0xD493
    /// 0xD5B5
    /// 0xD400
    /// 0xD1F6
    /// 0xD2D6 - proto_area_implement_client::recv_base_ping_r
    /// 0xCF24 - proto_area_implement_client::recv_event_select_map_and_channel
    /// 0xCC54 - proto_area_implement_client::recv_battle_report_action_cover
    /// 0xCB94 - proto_area_implement_client::recv_trade_notify_interface_status
    /// 0xCFDC - proto_area_implement_client::recv_data_notify_itemobject_data
    /// 0xCF29
    /// 0xD04A
    /// 0xCDC9 - proto_area_implement_client::recv_battle_report_noact_notify_dead
    /// 0xCCE2 - proto_area_implement_client::recv_chara_move_speed_per
    /// 0xCD63 - proto_area_implement_client::recv_dbg_select_raise_r
    /// 0xCE36 - proto_area_implement_client::recv_battle_report_noact_notify_buff_attach_failed
    /// 0xC8AD - proto_area_implement_client::recv_storage_deposit_item2_r
    /// 0xC6F2 - proto_area_implement_client::recv_wanted_list_open
    /// 0xC8AD - proto_area_implement_client::recv_storage_deposit_item2_r
    /// 0xC6F2 - proto_area_implement_client::recv_wanted_list_open
    /// 0xC68B - proto_area_implement_client::recv_wanted_jail_update_state
    /// 0xC6EF (0xC68B + 0x64)
    /// 0xCA35 - proto_area_implement_client::recv_battle_report_noact_notify_heal_life
    /// 0xC96F - proto_area_implement_client::recv_buff_request_detach_r
    /// 0xC9FF - proto_area_implement_client::recv_battle_report_notify_soul_material
    /// 0xCAB1 - proto_area_implement_client::recv_talkring_create_masterring_r
    /// 0xC701 - proto_area_implement_client::recv_battle_report_action_skill_onhit
    /// 0xC7E1 - proto_area_implement_client::recv_item_update_sp_level
    /// 0xC542 - proto_area_implement_client::recv_skill_start_cast_self
    /// 0xC444 - proto_area_implement_client::recv_random_box_get_item_r
    /// 0xC47A (0xC444 + 0x36)
    /// 0xC543 - proto_area_implement_client::recv_skill_combo_cast_r
    /// 0x840E
    /// 0xAF76 - proto_area_implement_client::recv_battle_report_notify_attack_hitstate
    /// 0xAB14
    /// 0xA90C
    /// 0xA7E8 - proto_area_implement_client::recv_chara_notify_map_fragment
    /// 0xBD7E - proto_area_implement_client::recv_event_tresurebox_begin
    /// 0xBA61
    /// 0xB684
    /// 0xB586
    /// 0xB417
    /// 0xB435 (0xB417 + 0x1E)
    /// 0xC0BB
    /// 0xBFEA
    /// 0xBF0D
    /// 0xBD90
    /// 0xBD99 (0xBD90 + 0x9) - proto_area_implement_client::recv_battle_report_notify_rookie
    /// 0xC1DC - proto_area_implement_client::recv_chara_update_notify_rookie
    /// 0xC0D8
    /// 0xC206
    /// 0xC003
    /// 0xBF34
    /// 0xBFE9
    /// 0xBBA5
    /// 0xBA71
    /// 0xBCC2
    /// 0xBC0A
    /// 0xBCAB
    /// 0xBD72
    /// 0xB813
    /// 0xB6B1
    /// 0xB782
    /// 0xBA11
    /// 0xB619
    /// 0xB631 (0xB619 + 0x18)
    /// 0xB195
    /// 0xB0BF
    /// 0xAF7F
    /// 0xB319
    /// 0xB1CA
    /// 0xB317
    /// 0xB0E5
    /// 0xAE27
    /// 0xABF2
    /// 0xAB8A
    /// 0xABC5 (0xAB8A + 0x3B)
    /// 0xAF49
    /// 0xAE2B
    /// 0xAEE9
    /// 0xAF6D
    /// 0xAD6D
    /// 0xADC8 (0xAD6D + 0x5B)
    /// 0xAA8F
    /// 0xA938
    /// 0xA9C2
    /// 0xAADD - proto_area_implement_client::recv_talkring_rename_request
    /// 0xA084
    /// 0x9D52
    /// 0x9B08
    /// 0x9A44
    /// 0x98D3
    /// 0x998F
    /// 0xA4D3
    /// 0xA21E
    /// 0xA0E3
    /// 0xA54C
    /// 0xA508
    /// 0xA611
    /// 0xA45C
    /// 0xA2B7
    /// 0xA43B
    /// 0xA4A5
    /// 0x9F31
    /// 0x9DE2
    /// 0x9D5A
    /// 0x9D96 (0x9D5A + 0x3C)
    /// 0x9F70
    /// 0x9E19
    /// 0x9E75 (0x9E19 + 0x5C)
    /// 0x9CA1
    /// 0x9BC6
    /// 0x9C3A (0x9BC6 + 0x74)
    /// 0x9CCF
    /// 0x9A79
    /// 0x9AA9 (0x9A79 + 0x30)
    /// 0x9578
    /// 0x924E
    /// 0x919C
    /// 0x906A
    /// 0x90E8 (0x906A + 0x7E)
    /// 0x9700
    /// 0x9666
    /// 0x95E6
    /// 0x965E (0x95E6 + 0x78)
    /// 0x97D9
    /// 0x971B
    /// 0x9761 (0x971B + 0x46)
    /// 0x9870
    /// 0x96EA
    /// 0x935B
    /// 0x9289
    /// 0x92DE (0x9289 + 0x55)
    /// 0x951B
    /// 0x9201
    /// 0x9227 (0x9201 + 0x26)
    /// 0x8D9B
    /// 0x8CC6
    /// 0x8F15
    /// 0x8DBC
    /// 0x8E92
    /// 0x8F84
    /// 0x8BB4
    /// 0x89FF
    /// 0x8A06 (0x89FF + 0x7)
    /// 0x8BD2
    /// 0x88A1
    /// 0x88AE (0x88A1 + 0xD)
    /// 0x662F
    /// 0x531B
    /// 0x4C6F
    /// 0x4440
    /// 0x42B8
    /// 0x3F38
    /// 0x3E0B
    /// 0x3E7F (0x3E0B + 0x74)
    /// 0x77A7
    /// 0x6FB2
    /// 0x6A7A
    /// 0x6912
    /// 0x68AA
    /// 0x66EC
    /// 0x679B
    /// 0x7FC5
    /// 0x7CF0
    /// 0x7A6F
    /// 0x79A2
    /// 0x789E
    /// 0x793E
    /// 0x839A
    /// 0x825D
    /// 0x8066
    /// 0x855C - proto_area_implement_client::recv_party_decline_to_apply_r
    /// 0x8487
    /// 0x85C6
    /// 0x8299
    /// 0x7D53
    /// 0x7D1C
    /// 0x7F34 - proto_area_implement_client::recv_skill_custom_slot_set_r
    /// 0x7D75 - proto_area_implement_client::recv_union_open_window
    /// 0x7F09 - proto_area_implement_client::recv_cash_shop_get_url_r
    /// 0x7F50 - proto_area_implement_client::recv_skill_next_cast_r
    /// 0x7BB3 - proto_area_implement_client::recv_party_change_leader_r
    /// 0x7B5D - proto_area_implement_client::recv_object_point_move_r
    /// 0x7B86 (0x7B5D + 0x29)
    /// 0x7CB2 - proto_area_implement_client::recv_battle_attack_pose_start_notify
    /// 0x79A9 - proto_area_implement_client::recv_storage_drawmoney
    /// 0x7A5C - proto_area_implement_client::recv_chara_update_ap_cost_diff
    /// 0x73A1
    /// 0x717D
    /// 0x70B7
    /// 0x7004
    /// 0x708B
    /// 0x755C
    /// 0x73D1
    /// 0x7697
    /// 0x7576
    /// 0x75A5 (0x7576 + 0x2F)
    /// 0x772A
    /// 0x7341
    /// 0x7181
    /// 0x732D
    /// 0x735E
    /// 0x70F6
    /// 0x715E (0x70F6 + 0x68)
    /// 0x6D46
    /// 0x6BDC
    /// 0x6B6A
    /// 0x6B74 (0x6B6A + 0xA)
    /// 0x6E06
    /// 0x6DB4
    /// 0x6DF4 (0x6DB4 + 0x40)
    /// 0x6F32
    /// 0x6C79
    /// 0x6C7B (0x6C79 + 0x2)
    /// 0x6979
    /// 0x692A
    /// 0x692B (0x692A + 0x1)
    /// 0x6A56
    /// 0x68E7
    /// 0x6911 (0x68E7 + 0x2A)
    /// 0x5E79
    /// 0x58E7
    /// 0x556E
    /// 0x5513
    /// 0x536A
    /// 0x5393 (0x536A + 0x29)
    /// 0x62E2
    /// 0x6030
    /// 0x5F6C
    /// 0x5EDB
    /// 0x5F1A (0x5EDB + 0x3F)
    /// 0x6554
    /// 0x6539
    /// 0x63B2
    /// 0x6510
    /// 0x65A6
    /// 0x6547
    /// 0x61C8
    /// 0x60E5
    /// 0x6175
    /// 0x621A
    /// 0x6001
    /// 0x6023 (0x6001 + 0x22)
    /// 0x5CF8
    /// 0x5C15
    /// 0x5A5F
    /// 0x5AE9
    /// 0x5D52
    /// 0x5C47
    /// 0x5899
    /// 0x570B
    /// 0x5842
    /// 0x58A8
    /// 0x551E
    /// 0x5527 (0x551E + 0x9)
    /// 0x4EF4
    /// 0x4E10
    /// 0x4CF3
    /// 0x4C74
    /// 0x4C8B (0x4C74 + 0x17)
    /// 0x50D1
    /// 0x505E
    /// 0x4F10
    /// 0x5016
    /// 0x5243
    /// 0x50B2
    /// 0x4D70
    /// 0x4DF8
    /// 0x4981
    /// 0x465C
    /// 0x4537
    /// 0x462E
    /// 0x4ABB
    /// 0x4978
    /// 0x4992 (0x4978 + 0x1A)
    /// 0x4B94
    /// 0x488E
    /// 0x488D (0x4883 + 0xA)
    /// 0x442A
    /// 0x42DE
    /// 0x4404
    /// 0x443C
    /// 0x3F5B
    /// 0x42B6
    /// 0x2BAB
    /// 0x2513
    /// 0x22E7
    /// 0x2063
    /// 0x1F73
    /// 0x32F7
    /// 0x300A
    /// 0x2DCE
    /// 0x2CAF
    /// 0x2BBF
    /// 0x2C30 (0x2BBF + 0x71)
    /// 0x3B77
    /// 0x36DC
    /// 0x3544
    /// 0x32FF
    /// 0x3428
    /// 0x3C68
    /// 0x3B9F
    /// 0x3D6C
    /// 0x3C81
    /// 0x3C89 (0x3C81 + 0x8)
    /// 0x3D9F
    /// 0x39FD
    /// 0x3806
    /// 0x394F
    /// 0x3A0E
    /// 0x362D
    /// 0x36A6 (0x362D + 0x79)
    /// 0x322F
    /// 0x316F
    /// 0x30BE
    /// 0x30FB (0x30BE + 0x3D)
    /// 0x3247
    /// 0x3223
    /// 0x3F2F
    /// 0x2E17
    /// 0x2F0E
    /// 0x2FFF
    /// 0x2CB0
    /// 0x2D6D
    /// 0x28A0
    /// 0x267D
    /// 0x2631
    /// 0x2576
    /// 0x260E
    /// 0x2A3F
    /// 0x28E7
    /// 0x287A
    /// 0x2A82
    /// 0x2AB0 (0x2A82 + 0x2E)
    /// 0x2BA4
    /// 0x27D6
    /// 0x26B8
    /// 0x2790
    /// 0x2849
    /// 0x266C
    /// 0x267B (0x266C + 0xF)
    /// 0x2470
    /// 0x23D3
    /// 0x2353
    /// 0x239D (0x2353 + 0x4A)
    /// 0x2478
    /// 0x23E5
    /// 0x2467
    /// 0x2246
    /// 0x213C
    /// 0x218A (0x213C + 0x4E)
    /// 0x2257
    /// 0x1837
    /// 0x14DA
    /// 0x129B
    /// 0x11FA
    /// 0x1105
    /// 0x1198
    /// 0x1C15
    /// 0x1A0F
    /// 0x18CC
    /// 0x1DBE
    /// 0x1D49
    /// 0x1C20
    /// 0x1C6C (0x1C20 + 0x4C)
    /// 0x1E65
    /// 0x1D68
    /// 0x1BD6
    /// 0x1AA8
    /// 0x1B5C
    /// 0x1C0A
    /// 0x170B
    /// 0x15D0
    /// 0x14F6
    /// 0x15B0
    /// 0x179D
    /// 0x170F
    /// 0x173C (0x170F + 0x2D)
    /// 0x182B
    /// 0x166B
    /// 0x1701
    /// 0x1392
    /// 0x12A4
    /// 0x12E0 (0x12A4 + 0x3C)
    /// 0x1489
    /// 0x124C
    /// 0x125E (0x124C + 0x12)
    /// 0xCFF
    /// 0x9F5
    /// 0x8CD
    /// 0xEF9
    /// 0xE45
    /// 0xD0D
    /// 0xE0B
    /// 0x102E
    /// 0xECF
    /// 0xC5F
    /// 0xB2F
    /// 0xBFE
    /// 0xCE7
    /// 0x604
    /// 0x54E
    /// 0x723
    /// 0x65C
    /// 0x661 (0x65C + 0x5)
    /// 0x780
    /// 0x397
    /// 0x22C
    /// 0x2C5
    /// 0xF02B (0xEFDD + 0x4E) - proto_area_implement_client::recv_random_box_get_item_all_r
    /// 0xFFD6 (0xFF00 + 0xD6) 
    /// 0xFD78 (0xFCF3 + 0x85) 
    /// 0xE990 (0xE897 + 0xF9) 
    /// 0xEE18 (0xEDB3 + 0x65) 
    /// 0xEF8D (0xEEB7 + 0xD6) 
    /// 0xE7E7 (0xE7BB + 0x2C) 
    /// 0xE14B (0xE07E + 0xCD) 
    /// 0xD752 (0xD68C + 0xC6) 
    /// 0xD57A (0xD493 + 0xE7) - proto_area_implement_client::recv_battle_report_action_item_use
    /// 0xD67D (0xD5B5 + 0xC8) - proto_area_implement_client::recv_chara_update_lv_detail2
    /// 0xD43F (0xD400 + 0x3F) - proto_area_implement_client::recv_event_removetrap_skill_r2
    /// 0xCC37 (0xCB94 + 0xA3) 
    /// 0xCF52 (0xCF29 + 0x29) 
    /// 0xD133 (0xD04A + 0xE9) 
    /// 0xA8BB (0xA7E8 + 0xD3) 
    /// 0xC1AF (0xC0D8 + 0xD7) 
    /// 0xC06C (0xC026 + 0x46) 
    /// 0xC078 (0xC003 + 0x75) 
    /// 0xBB65 (0xBA71 + 0xF4)  
    /// 0xB037 (0xAF7F + 0xB8) 
    /// 0xB292 (0xB1CA + 0xC8) 
    /// 0xB39D (0xB317 + 0x86) 
    /// 0xB10F (0xB0E5 + 0x2A) 
    /// 0xA18D (0xA0E3 + 0xAA) 
    /// 0xA549 (0xA508 + 0x41) 
    /// 0xA6F8 (0xA611 + 0xE7) 
    /// 0xA005 (0x9F70 + 0x95) 
    /// 0x8D92 (0x8CC6 + 0xCC) 
    /// 0x8091 (0x8066 + 0x2B) 
    /// 0x8549 (0x8487 + 0xC2) 
    /// 0x86A5 (0x85C6 + 0xDF) 
    /// 0x8395 (0x8299 + 0xFC) 
    /// 0x7D2B (0x7D1C + 0xF)  
    /// 0x746F (0x73D1 + 0x9E) 
    /// 0x65EE (0x65A6 + 0x48) 
    /// 0x5E48 (0x5D52 + 0xF6) 
    /// 0x5307 (0x5243 + 0xC4) 
    /// 0x4E8D (0x4E17 + 0x76) 
    /// 0x201C (0x1F73 + 0xA9) 
    /// 0x3C41 (0x3B9F + 0xA2) 
    /// 0x32ED (0x3247 + 0xA6) 
    /// 0x29C5 (0x28E7 + 0xDE) 
    /// 0x250E (0x2478 + 0x96) 
    /// 0x19C3 (0x18CC + 0xF7) 
    /// 0x1F32 (0x1E65 + 0xCD) 
    /// 0x99D  (0x8CD + 0xD0)  
    /// 0x10C5 (0x102E + 0x97) 
    /// 0x5F8  (0x54E + 0xAA)  
    /// 0x411  (0x397 + 0x7A)  
    /// 0xC374 (0xC2A1 + 0xD3)
    /// 
    /// Ranges:
    /// 0xEFDD + 0x4E - 0x004CEC2C
    /// 0xFF00 + 0xD6 - 0x004D13C3
    /// 0xFCF3 + 0x85 - 0x004D042D
    /// 0xE897 + 0xF9 - 0x004CC897
    /// 0xEDB3 + 0x65 - 0x004CDE73
    /// 0xEEB7 + 0xD6 - 0x004CE472
    /// 0xE7BB + 0x2C - 0x004CC261
    /// 0xE07E + 0xCD - 0x004CB292
    /// 0xD68C + 0xC6 - 0x004C8EE5
    /// 0xD493 + 0xE7 - 0x004C7D1F
    /// 0xD5B5 + 0xC8 - 0x004C86F2
    /// 0xD400 + 0x3F - 0x004C7645
    /// 0xCB94 + 0xA3 - 0x004C5085
    /// 0xCF29 + 0x29 - 0x004C5F8E
    /// 0xD04A + 0xE9 - 0x004C6663
    /// 0xA7E8 + 0xD3 - 0x004BB352
    /// 0xC0D8 + 0xD7 - 0x004C21A0
    /// 0xC026 + 0x46 - 0x004C2984
    /// 0xC003 + 0x75 - 0x004C1A1E
    /// 0xBA71 +  0xF - 0x004C02F0
    /// 0xAF7F + 0xB8 - 0x004BD4CC
    /// 0xB1CA + 0xC8 - 0x004BE4A3
    /// 0xB317 + 0x86 - 0x004BEB5D 
    /// 0xB0E5 + 0x2A - 0x004BDD95 
    /// 0xA0E3 + 0xAA - 0x004B98BB 
    /// 0xA508 + 0x41 - 0x004BA68E
    /// 0xA611 + 0xE7 - 0x004BAC6C 
    /// 0x9F70 + 0x95 - 0x004B9266 
    /// 0x8CC6 + 0xCC - 0x004B483D 
    /// 0x8066 + 0x2B - 0x004B1728 
    /// 0x8487 + 0xC2 - 0x004B2631
    /// 0x85C6 + 0xDF - 0x004B2C99
    /// 0x8299 + 0xFC - 0x004B1EE6
    /// 0x7D1C +  0xF - 0x004B09BA
    /// 0x73D1 + 0x9E - 0x004AEEA5
    /// 0x65A6 + 0x48 - 0x004AB883
    /// 0x5D52 + 0xF6 - 0x004A9C48
    /// 0x5243 + 0xC4 - 0x004A7ED1
    /// 0x4E17 + 0x76 - 0x004A732D
    /// 0x1F73 + 0xA9 - 0x0049CF7A
    /// 0x3B9F + 0xA2 - 0x004A40E1
    /// 0x3247 + 0xA6 - 0x004A29D7
    /// 0x28E7 + 0xDE - 0x004A064B
    /// 0x2478 + 0x96 - 0x0049EE79
    /// 0x18CC + 0xF7 - 0x0049B302
    /// 0x1E65 + 0xCD - 0x0049C989
    /// 0x8CD  + 0xD0 - 0x004979DC
    /// 0x102E + 0x97 - 0x00498F03
    /// 0x54E  + 0xAA - 0x00496B4F
    /// 0x397  + 0x7A - 0x004964AA
    /// 0xC2A1 + 0xD3 - 0x004C2F8E
    /// </summary>
    
    public enum AreaPacketId : ushort
    {
        // Recv OP Codes - Switch: 0x495B88 - ordered by op code
        send_base_check_version = 0x5705,
        recv_base_check_version_r = 0xEFDD,
        send_base_enter = 0xAE43
        
        // Send OP Codes - ordered by op code
    }
}
