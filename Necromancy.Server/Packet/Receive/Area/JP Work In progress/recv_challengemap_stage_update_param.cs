using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_stage_update_param : PacketResponse
    {
        public recv_challengemap_stage_update_param()
            : base((ushort) AreaPacketId.recv_challengemap_stage_update_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            res.WriteUInt32(0);
            return res;
        }
    }
}
