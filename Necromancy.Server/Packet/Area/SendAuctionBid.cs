using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionBid : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendAuctionBid));

        public SendAuctionBid(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_auction_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte isBuyout = packet.data.ReadByte();
            int slot = packet.data.ReadInt32();
            ulong bid = packet.data.ReadUInt64();
            _Logger.Debug(isBuyout + " " + slot + " " + bid);
            int auctionError = 0;
            ItemService itemService = new ItemService(client.character);
            try
            {
                itemService.Bid(isBuyout, slot, bid);
            }
            catch (AuctionException e)
            {
                auctionError = (int)e.type;
                _Logger.Debug(e.ToString());
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError);
            router.Send(client.map, (ushort)AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
