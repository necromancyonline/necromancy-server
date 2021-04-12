using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvDbgMessage : PacketResponse
    {
        public RecvDbgMessage()
            : base((ushort)MsgPacketId.recv_dbg_message, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteCString(""); //max size 0x601
            return res;
        }
    }
}
