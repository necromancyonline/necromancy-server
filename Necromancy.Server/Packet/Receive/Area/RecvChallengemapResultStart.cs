using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapResultStart : PacketResponse
    {
        public RecvChallengemapResultStart()
            : base((ushort)AreaPacketId.recv_challengemap_result_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt64(1);
            res.WriteInt64(1);
            return res;
        }
    }
}
