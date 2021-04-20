using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Item
{
    public class SendAuctionCancelBid : ClientHandler
    {
        public SendAuctionCancelBid(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_auction_cancel_bid;

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
            res.WriteInt32(0);
            router.Send(client.map, (ushort)AreaPacketId.recv_auction_cancel_bid_r, res, ServerType.Area);
        }
    }
}
