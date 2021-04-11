using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0xFDCB : PacketResponse
    {
        public recv_0xFDCB()
            : base((ushort) AreaPacketId.recv_0xFDCB, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
