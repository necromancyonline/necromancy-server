using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvXigncodePacketCl : PacketResponse
    {
        public RecvXigncodePacketCl()
            : base((ushort)MsgPacketId.recv_xigncode_packet_cl, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //missing
            return res;
        }
    }
}
