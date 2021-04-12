using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X6B6A : PacketResponse
    {
        public Recv0X6B6A()
            : base((ushort)AreaPacketId.recv_0x6B6A, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0); //X
            res.WriteFloat(0); //Y
            res.WriteFloat(0); //Z
            res.WriteByte(0); //View Offset
            res.WriteByte(0);
            return res;
        }
    }
}
