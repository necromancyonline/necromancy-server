using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCpfAuthenticate : PacketResponse
    {
        public RecvCpfAuthenticate()
            : base((ushort)AreaPacketId.recv_cpf_authenticate, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x80;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++) res.WriteByte(0);
            return res;
        }
    }
}
