using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvCpfAuthenticateMsg : PacketResponse
    {
        public RecvCpfAuthenticateMsg()
            : base((ushort) MsgPacketId.recv_cpf_authenticate, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x80;
            res.WriteInt32(numEntries); //less than or equal to 0x80
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);
            }
            return res;
        }
    }
}
