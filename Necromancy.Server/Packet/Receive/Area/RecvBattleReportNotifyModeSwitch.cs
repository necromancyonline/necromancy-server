using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyModeSwitch : PacketResponse
    {
        public RecvBattleReportNotifyModeSwitch()
            : base((ushort) AreaPacketId.recv_battle_report_notify_mode_switch, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
