// ReSharper disable InconsistentNaming

namespace Necromancy.Server.Packet.Id
{
    /// <summary>
    ///     Necromancy Authentication Server OP Codes
    /// </summary>
    public enum AuthPacketId : ushort
    {
        // Recv OP Codes - Switch: 0x4DE650 - ordered by op code
        recv_base_authenticate_hangame_r = 0xA53,
        recv_base_authenticate_r = 0x15C7,
        recv_base_authenticate_soe_r = 0x174A,
        recv_cpf_notify_error = 0x73D7,
        recv_base_authenticate_soe_sessionid = 0x7EEA,
        recv_base_select_world_r = 0x848C,
        recv_base_get_worldlist_r = 0xB717,
        recv_cpf_authenticate = 0xBA73,
        recv_base_ping_r = 0xD2D6,
        recv_base_check_version2_r = 0xEC0A,
        recv_base_check_version_r = 0xEFDD,

        // Send OP Codes - ordered by op code
        send_base_select_world = 0x203F,
        send_base_get_worldlist = 0x53CF,
        send_base_check_version = 0x5705,
        send_base_authenticate = 0xAD93,
        send_base_authenticate_hangame = 0x0,
        send_base_authenticate_niconico_app = 0x0
    }
}
