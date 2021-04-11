using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0x2B7A : PacketResponse
    {
        public recv_0x2B7A()
            : base((ushort) AreaPacketId.recv_0x2B7A, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);

            return res;
        }
    }
}
