using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X3Fb7 : PacketResponse
    {
        public Recv0X3Fb7()
            : base((ushort)AreaPacketId.recv_0x3FB7, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //Error!!!  maybe try values other than 0...
            res.WriteInt16(0);

            return res;
        }
    }
}
