using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportPhyDamageHp : PacketResponse
    {
        private readonly int _damage;
        private readonly uint _instanceId;

        public RecvBattleReportPhyDamageHp(uint instanceId, int damage)
            : base((ushort)AreaPacketId.recv_battle_report_notify_phy_damage_hp, ServerType.Area)
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
