using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReleaseAttackPoseSelf : PacketResponse
    {
        public RecvBattleReleaseAttackPoseSelf()
            : base((ushort)AreaPacketId.recv_battle_release_attack_pose_self, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            return res;
        }
    }
}
