using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_battle_report_action_attack_long_reload_cancel : PacketResponse
    {
        public recv_battle_report_action_attack_long_reload_cancel()
            : base((ushort) AreaPacketId.recv_battle_report_action_attack_long_reload_cancel, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No structure

            return res;
        }
    }
}
