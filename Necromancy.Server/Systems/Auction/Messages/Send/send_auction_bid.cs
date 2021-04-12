using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Item
{
    public class SendAuctionBid : ClientHandler
    {
        public SendAuctionBid(NecServer server) : base(server)
        {
            //TODO find out why this is here and if its needed.
        }

        public override ushort id => (ushort)AreaPacketId.send_auction_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //AuctionService auctionService = new AuctionService(client);
            int error = 0;
            try
            {
                //auctionService.Bid(); //TODO find data
            }
            catch (AuctionException e)
            {
                error = (int)e.type;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(error);
            router.Send(client.map, (ushort)AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
