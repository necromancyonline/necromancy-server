using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_class_advancement_notify_week_exp_reset : PacketResponse
    {
        public recv_class_advancement_notify_week_exp_reset()
            : base((ushort) AreaPacketId.recv_class_advancement_notify_week_exp_reset, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no Structure

            return res;
        }
    }
}
