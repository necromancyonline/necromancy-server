using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0XBa61 : PacketResponse
    {
        public Recv0XBa61()
            : base((ushort)AreaPacketId.recv_0xBA61, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0xA;
            res.WriteInt32(numEntries);
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16(0);
                res.WriteInt64(0);
                res.WriteFixedString("", 0xC1);
            }

            return res;
        }
    }
}
