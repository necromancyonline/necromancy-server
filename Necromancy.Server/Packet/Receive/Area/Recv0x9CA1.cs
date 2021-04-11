using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X9Ca1 : PacketResponse
    {
        public Recv0X9Ca1()
            : base((ushort) AreaPacketId.recv_0x9CA1, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); //0x31
            res.WriteCString(""); //0x5B
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
