using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvBaseAuthenticateSoeSessionId : PacketResponse
    {
        public RecvBaseAuthenticateSoeSessionId()
            : base((ushort)AuthPacketId.recv_base_authenticate_soe_sessionid, ServerType.Auth)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); // Max size is 0x11
            return res;
        }
    }
}
