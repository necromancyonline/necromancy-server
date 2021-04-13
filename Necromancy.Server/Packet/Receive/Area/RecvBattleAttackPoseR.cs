using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleAttackPoseR : PacketResponse
    {
        private readonly uint _instanceId;

        public RecvBattleAttackPoseR(uint instanceId)
            : base((ushort)AreaPacketId.recv_battle_attack_pose_self, ServerType.Area)
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
