using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionCancelBid : ClientHandler
    {
        public SendAuctionCancelBid(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_auction_cancel_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte slot = packet.data.ReadByte();

            int auctionError = 0;
            ItemService itemService = new ItemService(client.character);
            try
            {
                itemService.CancelBid(slot);
            }
            catch (AuctionException e)
            {
                auctionError = (int)e.type;
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError);
            router.Send(client.map, (ushort)AreaPacketId.recv_auction_cancel_bid_r, res, ServerType.Area);
        }
    }
}
