using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0x9454 : PacketResponse
    {
        public recv_0x9454()
            : base((ushort) AreaPacketId.recv_0x9454, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFixedString("ok", 0x2);
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
