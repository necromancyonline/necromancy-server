using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvWantedUpdateRewardPoint : PacketResponse
    {
        public RecvWantedUpdateRewardPoint()
            : base((ushort)AreaPacketId.recv_wanted_update_reward_point, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);

            res.WriteInt64(0);
            return res;
        }
    }
}
