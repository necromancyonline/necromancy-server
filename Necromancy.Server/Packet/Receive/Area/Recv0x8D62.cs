using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X8D62 : PacketResponse
    {
        public Recv0X8D62()
            : base((ushort) AreaPacketId.recv_0x8D62, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0xA;// <=0xA
            res.WriteInt32(numEntries); //// <=0xA
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16(0);
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
