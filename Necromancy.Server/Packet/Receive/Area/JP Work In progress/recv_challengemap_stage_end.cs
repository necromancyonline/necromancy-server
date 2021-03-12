using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_stage_end : PacketResponse
    {
        public recv_challengemap_stage_end()
            : base((ushort) AreaPacketId.recv_challengemap_stage_end, ServerType.Area)
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
