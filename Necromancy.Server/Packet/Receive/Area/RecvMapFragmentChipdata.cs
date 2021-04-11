using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvMapFragmentChipdata : PacketResponse
    {
        public RecvMapFragmentChipdata()
            : base((ushort) AreaPacketId.recv_map_fragment_chipdata, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(numEntries); //less than 0x190
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
            }
            res.WriteInt32(0);
            return res;
        }
    }
}
