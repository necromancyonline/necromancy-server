using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventBlockMessageNoObject : PacketResponse
    {
        public RecvEventBlockMessageNoObject()
            : base((ushort)AreaPacketId.recv_event_block_message_no_object, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound"); // find max size 
            res.WriteCString("ToBeFound"); // find max size 
            res.WriteCString("ToBeFound"); // find max size
            return res;
        }
    }
}
