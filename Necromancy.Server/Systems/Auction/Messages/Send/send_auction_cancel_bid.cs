using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction.Logic;

namespace Necromancy.Server.Systems.Auction
{
    public class send_auction_cancel_bid : ClientHandler
    {
        public send_auction_cancel_bid(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_auction_cancel_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            AuctionService auctionService = new AuctionService(client);
            int error = 0;
            try
            {
                //auctionService.Bid(); //TODO find data
            }
            catch (AuctionException e)
            {
                error = (int)e.Type;
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_cancel_bid_r, res, ServerType.Area);
        }
    }
}
