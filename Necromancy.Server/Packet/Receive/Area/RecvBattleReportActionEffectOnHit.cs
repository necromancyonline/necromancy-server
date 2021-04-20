using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionEffectOnHit : PacketResponse
    {
        private readonly int _effectId;

        public RecvBattleReportActionEffectOnHit(int effectId)
            : base((ushort)AreaPacketId.recv_battle_report_action_effect_onhit, ServerType.Area)
        {
            _effectId = effectId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_effectId);
            return res;
        }
    }
}
