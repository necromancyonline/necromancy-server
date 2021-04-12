using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X531B : PacketResponse
    {
        public Recv0X531B()
            : base((ushort)AreaPacketId.recv_0x531B, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            res.WriteFloat(0);
            res.WriteCString(""); //0x31
            res.WriteCString(""); //0x5B
            return res;
        }
    }
}
