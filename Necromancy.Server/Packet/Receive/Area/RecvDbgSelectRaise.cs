using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDbgSelectRaise : PacketResponse
    {
        public RecvDbgSelectRaise()
            : base((ushort) AreaPacketId.recv_dbg_select_raise_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(200000005);
            return res;
        }
    }
}
