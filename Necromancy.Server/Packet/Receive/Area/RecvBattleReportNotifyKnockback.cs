using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyKnockback : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly float _targetFreezeDuration;
        private readonly float _targetKnockbackAnimationDuration;

        public RecvBattleReportNotifyKnockback(uint instanceId, float targetFreezeDuration,
            float targetKnockbackAnimationDuration)
            : base((ushort)AreaPacketId.recv_battle_report_noact_notify_knockback, ServerType.Area)
        {
            _instanceId = instanceId;
            _targetFreezeDuration = targetFreezeDuration;
            _targetKnockbackAnimationDuration = targetKnockbackAnimationDuration;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteFloat(_targetFreezeDuration);
            res.WriteFloat(_targetKnockbackAnimationDuration); // delay in seconds
            return res;
        }
    }
}
