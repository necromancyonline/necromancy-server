using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyPartnerDamageMp : PacketResponse
    {
        public RecvBattleReportNotifyPartnerDamageMp()
            : base((ushort) AreaPacketId.recv_battle_report_notify_partner_damage_mp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
