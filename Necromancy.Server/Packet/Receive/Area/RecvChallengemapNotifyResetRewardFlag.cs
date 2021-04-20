using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapNotifyResetRewardFlag : PacketResponse
    {
        public RecvChallengemapNotifyResetRewardFlag()
            : base((ushort)AreaPacketId.recv_challengemap_notify_reset_reward_flag, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No Structure

            return res;
        }
    }
}
