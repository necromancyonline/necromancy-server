using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0XE983 : PacketResponse
    {
        public Recv0XE983()
            : base((ushort) AreaPacketId.recv_0xE983, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");//find max size
            return res;
        }
    }
}
