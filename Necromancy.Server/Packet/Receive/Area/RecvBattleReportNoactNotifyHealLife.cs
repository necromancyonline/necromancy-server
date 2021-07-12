using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNoactNotifyHealLife : PacketResponse
    {
        private readonly int _damage;
        private readonly uint _instanceId;
        public RecvBattleReportNoactNotifyHealLife(uint instanceId, int damage)
            : base((ushort)AreaPacketId.recv_battle_report_noact_notify_heal_life, ServerType.Area)
        {
            _instanceId = instanceId;
            _damage = damage;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteInt32(System.Math.Abs(_damage));
            res.WriteByte(0);
            return res;
        }
    }
}
