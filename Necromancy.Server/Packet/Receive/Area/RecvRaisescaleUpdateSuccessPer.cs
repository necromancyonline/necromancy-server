using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRaisescaleUpdateSuccessPer : PacketResponse
    {
        public RecvRaisescaleUpdateSuccessPer()
            : base((ushort)AreaPacketId.recv_raisescale_update_success_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
