using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X94B9 : PacketResponse
    {
        public Recv0X94B9()
            : base((ushort)AreaPacketId.recv_0x94B9, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(1); //item row id
            res.WriteInt64(20000002); //item instance ID
            res.WriteInt64(2); //item sale price
            return res;
        }
    }
}
