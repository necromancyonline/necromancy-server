using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventBlockMessageEnd : PacketResponse
    {
        public RecvEventBlockMessageEnd()
            : base((ushort)AreaPacketId.recv_event_block_message_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No structure

            return res;
        }
    }
}
