using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0XC078Wrong2 : PacketResponse
    {
        public Recv0XC078Wrong2()
            : base((ushort)AreaPacketId.recv_0xC078, ServerType.Area)
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
