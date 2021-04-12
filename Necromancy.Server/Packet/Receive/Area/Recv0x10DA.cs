using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X10Da : PacketResponse
    {
        public Recv0X10Da()
            : base((ushort)AreaPacketId.recv_0x10DA, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x5;
            res.WriteInt32(numEntries); //less than or equal to 5
            for (int i = 0; i < numEntries; i++)
                res.WriteInt32(0);
            return res;
        }
    }
}
