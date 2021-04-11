using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_auction_bid : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_auction_bid));
        public send_auction_bid(NecServer server) : base(server)  {    }

        public override ushort Id => (ushort) AreaPacketId.send_auction_bid;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte isBuyout = packet.Data.ReadByte();
            int slot = packet.Data.ReadInt32();
            ulong bid = packet.Data.ReadUInt64();
            Logger.Debug(isBuyout.ToString() + " " + slot.ToString() + " " +  bid.ToString());
            int auctionError = 0;
            ItemService itemService = new ItemService(client.Character);
            try
            {
                itemService.Bid(isBuyout, slot, bid);
            }
            catch (AuctionException e) { auctionError = (int)e.Type;
                Logger.Debug(e.ToString());
            }
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(auctionError);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
