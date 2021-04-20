using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvXigncodePacketSv : PacketResponse
    {
        public RecvXigncodePacketSv()
            : base((ushort)MsgPacketId.recv_xigncode_packet_sv, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0xA00;
            res.WriteInt32(numEntries); //Less than or equal to 0xA00
            for (int i = 0; i < numEntries; i++)
                res.WriteByte(0);
            return res;
        }
    }
}
