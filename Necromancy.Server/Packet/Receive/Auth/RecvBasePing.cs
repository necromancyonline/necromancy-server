using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvBasePing : PacketResponse
    {
        public RecvBasePing()
            : base((ushort)AuthPacketId.recv_base_ping_r, ServerType.Auth)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            return res;
        }
    }
}
