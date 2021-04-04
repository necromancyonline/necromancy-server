using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_cancel_bid : ClientHandler
    {
        public send_auction_cancel_bid(NecServer server) : base(server) { }
        public override ushort Id => (ushort) AreaPacketId.send_auction_cancel_bid;
        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_cancel_bid_r, res, ServerType.Area);
        }
    }
}
