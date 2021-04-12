using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvCpfNotifyError : PacketResponse
    {
        public RecvCpfNotifyError()
            : base((ushort)AuthPacketId.recv_cpf_notify_error, ServerType.Auth)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            return res;
        }
    }
}
