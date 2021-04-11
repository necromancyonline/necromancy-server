using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_xigncode_packet_sv : PacketResponse
    {
        public recv_xigncode_packet_sv()
            : base((ushort) AreaPacketId.recv_xigncode_packet_sv, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries); //less than or = 0xA00 
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteByte(0);
            }
            return res;
        }
    }
}
