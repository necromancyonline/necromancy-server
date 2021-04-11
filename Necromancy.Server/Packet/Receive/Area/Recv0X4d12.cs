using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X4D12 : PacketResponse
    {
        public Recv0X4D12()
            : base((ushort) AreaPacketId.recv_0x4D12, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
