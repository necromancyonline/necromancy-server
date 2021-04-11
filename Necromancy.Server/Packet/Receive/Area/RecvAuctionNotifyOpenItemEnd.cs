using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionNotifyOpenItemEnd : PacketResponse
    {
        public RecvAuctionNotifyOpenItemEnd(NecClient necClient) : base((ushort) AreaPacketId.recv_auction_notify_open_item_end, ServerType.Area)
        {
            clients.Add(necClient);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            return res;
        }
    }
}
