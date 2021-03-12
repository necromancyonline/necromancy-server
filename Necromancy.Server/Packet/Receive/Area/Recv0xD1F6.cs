using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0xD1F6 : PacketResponse
    {
        public Recv0xD1F6()
            : base((ushort) AreaPacketId.recv_0xD1F6, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteInt16(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteFloat(0);
            res.WriteFloat(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
