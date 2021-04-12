using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class Recv0X4978 : PacketResponse
    {
        public Recv0X4978()
            : base((ushort)AreaPacketId.recv_0x4978, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);

            int numEntries = 0xA;
            res.WriteInt32(0xA); //<a
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt16(1);
                int numEntries2 = 0xC1;
                res.WriteInt64(numEntries2);
                for (int j = 0; j < numEntries2; j++) res.WriteByte(1);
            }

            return res;
        }
    }
}
