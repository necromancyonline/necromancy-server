using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNoactNotifyHealAp : PacketResponse
    {
        public RecvBattleReportNoactNotifyHealAp()
            : base((ushort)AreaPacketId.recv_battle_report_noact_notify_heal_ap, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(200000002);
            return res;
        }
    }
}
