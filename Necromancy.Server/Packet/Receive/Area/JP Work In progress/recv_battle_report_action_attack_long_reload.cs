using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_battle_report_action_attack_long_reload : PacketResponse
    {
        public recv_battle_report_action_attack_long_reload()
            : base((ushort) AreaPacketId.recv_battle_report_action_attack_long_reload, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no Structure

            return res;
        }
    }
}
