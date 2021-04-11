using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_map_chip_data : PacketResponse
    {
        public recv_map_chip_data()
            : base((ushort) AreaPacketId.recv_map_chip_data, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x190
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteInt32(0);
            }
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
