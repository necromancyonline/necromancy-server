using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction_House;
using Necromancy.Server.Systems.Auction_House.Logic;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens auction house
    public class SendAuctionNotifyOpen : ServerChatCommand
    {
        public SendAuctionNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //AuctionService auctionHouse = new AuctionService(client);
            AuctionLot[] lots = new AuctionLot[0]; //TODO auctionHouse.GetLots();
            AuctionLot[] bids = new AuctionLot[0]; //TODO auctionHouse.GetBids();
            const byte isInMaintenanceMode = 0x0;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(lots.Length);

            for (int i = 0; i < lots.Length; i++)
            {
                res.WriteByte((byte)i); // row number?
                res.WriteInt32(i); // row number ??
                res.WriteInt64(lots[i].ItemID); //spawned item id
                res.WriteInt32(lots[i].MinimumBid);
                res.WriteInt32(lots[i].BuyoutPrice);
                res.WriteFixedString(lots[i].ConsignerName, 49);
                res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
                res.WriteFixedString(lots[i].Comment, 385);
                res.WriteInt16((short)lots[i].CurrentBid); // Bid why convert to short?
                res.WriteInt32(lots[i].SecondsUntilExpiryTime);

                res.WriteInt32(0); // unknown
                res.WriteInt32(0); // unknown
            }

            res.WriteInt32(bids.Length); // must be< = 8 | why?

            for (int i = 0; i < bids.Length; i++)
            {
                res.WriteByte((byte)i); // row number?
                res.WriteInt32(i); // row number ??
                res.WriteInt64(bids[i].ItemID);
                res.WriteInt32(bids[i].MinimumBid); // Lowest
                res.WriteInt32(bids[i].BuyoutPrice); // Buy Now
                res.WriteFixedString(bids[i].ConsignerName, 49);
                res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
                res.WriteFixedString(bids[i].Comment, 385); // Comment in the item information
                res.WriteInt16((short)bids[i].CurrentBid); // Bid why convert to short?
                res.WriteInt32(bids[i].SecondsUntilExpiryTime);

                res.WriteInt32(0); // unknown
                res.WriteInt32(0); // unknown
            }

            res.WriteByte(isInMaintenanceMode); // bool  IsInMaintenanceMode
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "auct";
    }
}
