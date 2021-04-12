using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionEqBreak : PacketResponse
    {
        public RecvBattleReportActionEqBreak()
            : base((ushort)AreaPacketId.recv_battle_report_action_eq_break, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // leave as 0
            res.WriteInt32(0); //number of slot? to break
            return res;
        }
    }
}
