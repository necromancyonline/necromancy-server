using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyDamageAc : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _damage;
        public RecvBattleReportNotifyDamageAc(uint instanceId, int damage)
            : base((ushort) AreaPacketId.recv_battle_report_notify_damage_ac, ServerType.Area)
        {
            _instanceId = instanceId;
            _damage = damage;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteInt32(_damage);
            return res;
        }
    }
}
