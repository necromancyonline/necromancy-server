using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_class_advancement_notify_cnt_reset : PacketResponse
    {
        public recv_class_advancement_notify_cnt_reset()
            : base((ushort) AreaPacketId.recv_class_advancement_notify_cnt_reset, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no structure

            return res;
        }
    }
}
