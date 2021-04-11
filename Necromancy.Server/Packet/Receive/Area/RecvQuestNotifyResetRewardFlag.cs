using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_quest_notify_reset_reward_flag : PacketResponse
    {
        public recv_quest_notify_reset_reward_flag()
            : base((ushort) AreaPacketId.recv_quest_notify_reset_reward_flag, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("What");
            return res;
        }
    }
}
