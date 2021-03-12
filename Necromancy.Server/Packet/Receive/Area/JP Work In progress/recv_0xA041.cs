using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0xA041 : PacketResponse
    {
        public recv_0xA041()
            : base((ushort) AreaPacketId.recv_0xA041, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteCString("What");
            return res;
        }
    }
}
