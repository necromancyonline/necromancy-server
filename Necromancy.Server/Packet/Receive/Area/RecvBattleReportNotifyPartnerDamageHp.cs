using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyPartnerDamageHp : PacketResponse
    {
        public RecvBattleReportNotifyPartnerDamageHp()
            : base((ushort) AreaPacketId.recv_battle_report_notify_partner_damage_hp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//objectId
            res.WriteInt32(0);//Damage
            return res;
        }
    }
}
