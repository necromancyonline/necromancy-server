using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvUnionMantleOpen : PacketResponse
    {
        public RecvUnionMantleOpen()
            : base((ushort)AreaPacketId.recv_union_mantle_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            for (int i = 0; i < 0x10; i++) res.WriteByte(0);
            return res;
        }
    }
}
