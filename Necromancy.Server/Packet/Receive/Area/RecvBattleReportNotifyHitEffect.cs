using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyHitEffect : PacketResponse
    {
        private readonly uint _instanceId;

        public RecvBattleReportNotifyHitEffect(uint instanceId)
            : base((ushort)AreaPacketId.recv_battle_report_notify_hit_effect, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            return res;
        }
    }
}
