using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyHitEffectName : PacketResponse
    {
        public RecvBattleReportNotifyHitEffectName()
            : base((ushort)AreaPacketId.recv_battle_report_notify_hit_effect_name, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteCString("ToBeFound");

            res.WriteCString("ToBeFound_2");
            return res;
        }
    }
}
