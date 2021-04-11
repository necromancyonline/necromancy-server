using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_0x5418 : PacketResponse
    {
        public recv_0x5418()
            : base((ushort) AreaPacketId.recv_0x5418, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(1);
            res.WriteUInt32(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            res.WriteByte(2);
            res.WriteFixedString("Xeno", 0x10);
            res.WriteInt32(23);
            res.WriteInt16(1);
            return res;
        }
    }
}
