using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRaiseScaleViewClose : PacketResponse
    {
        public RecvRaiseScaleViewClose()
            : base((ushort)AreaPacketId.recv_raisescale_view_close, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            return res;
        }
    }
}
