using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X5513 : PacketResponse
    {
        public Recv0X5513()
            : base((ushort)AreaPacketId.recv_0x5513, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);

            res.WriteFloat(0); //x
            res.WriteFloat(0); //y
            res.WriteFloat(0); //z
            res.WriteByte(0); //offset

            res.WriteCString(""); //601
            return res;
        }
    }
}
