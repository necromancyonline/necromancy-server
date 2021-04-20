using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Auth
{
    public class RecvCpfAuthenticate : PacketResponse
    {
        private readonly string _cpfString;

        public RecvCpfAuthenticate(string cpfString)
            : base((ushort)AuthPacketId.recv_cpf_authenticate, ServerType.Auth)
        {
            _cpfString = cpfString;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int size = _cpfString.Length;
            res.WriteInt32(size); //Less than or equal to 0x80
            res.WriteFixedString($"{_cpfString}", size);
            return res;
        }
    }
}
