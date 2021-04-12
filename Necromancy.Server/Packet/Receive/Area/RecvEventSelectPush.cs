using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectPush : PacketResponse
    {
        public RecvEventSelectPush()
            : base((ushort)AreaPacketId.recv_event_select_push, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); //Length 0x601
            return res;
        }
    }
}
