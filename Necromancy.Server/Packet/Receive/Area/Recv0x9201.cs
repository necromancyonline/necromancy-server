using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X9201 : PacketResponse
    {
        public Recv0X9201()
            : base((ushort) AreaPacketId.recv_0x9201, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            for (int i = 0; i < 5; i++)
            {
                res.WriteFixedString("", 0x31);
                res.WriteFixedString("", 0x25);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
            }
            res.WriteFixedString("", 0x31);
            res.WriteFixedString("", 0x25);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
