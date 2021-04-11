using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGemSetSpallR : PacketResponse
    {
        public RecvGemSetSpallR()
            : base((ushort) AreaPacketId.recv_gem_set_spall_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//errcheck

            return res;
        }
    }
}
