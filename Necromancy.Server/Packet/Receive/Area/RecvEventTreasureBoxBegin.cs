using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventTreasureboxBegin : PacketResponse
    {
        public RecvEventTreasureboxBegin()
            : base((ushort) AreaPacketId.recv_event_treasurebox_begin, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x10;
            res.WriteInt32(numEntries); //less than 0x10
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
