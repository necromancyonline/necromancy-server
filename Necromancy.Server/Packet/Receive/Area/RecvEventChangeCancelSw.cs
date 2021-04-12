using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventChangeCancelSw : PacketResponse
    {
        public RecvEventChangeCancelSw()
            : base((ushort)AreaPacketId.recv_event_change_cancel_sw, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0); //Bool
            return res;
        }
    }
}
