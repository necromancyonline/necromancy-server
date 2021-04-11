using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_ticket_notify_close : PacketResponse
    {
        public recv_challengemap_ticket_notify_close()
            : base((ushort) AreaPacketId.recv_challengemap_ticket_notify_close, ServerType.Area)
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
