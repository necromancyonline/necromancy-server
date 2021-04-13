using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvDbgMessage : PacketResponse
    {
        public RecvDbgMessage()
            : base((ushort)AreaPacketId.recv_dbg_message, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);

            res.WriteCString("ToBeFound");
            return res;
        }
    }
}
