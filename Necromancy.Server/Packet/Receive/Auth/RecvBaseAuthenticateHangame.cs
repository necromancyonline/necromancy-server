using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvBaseAuthenticateHangame : PacketResponse
    {
        public RecvBaseAuthenticateHangame()
            : base((ushort)AuthPacketId.recv_base_authenticate_hangame_r, ServerType.Auth)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
