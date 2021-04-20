using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionSearch : PacketResponse
    {
        public RecvAuctionSearch()
            : base((ushort)AreaPacketId.recv_auction_search_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0x64); // cmp to 0x64 = 100

            int numEntries4 = 0x64;
            for (int i = 0; i < numEntries4; i++)
            {
                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(0);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);
            }

            return res;
        }
    }
}
