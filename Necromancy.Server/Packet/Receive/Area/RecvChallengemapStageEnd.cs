using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapStageEnd : PacketResponse
    {
        public RecvChallengemapStageEnd()
            : base((ushort)AreaPacketId.recv_challengemap_stage_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
