using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Auction;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Chat.Command.Commands
{
    //opens auction house
    public class SendAuctionNotifyOpen : ServerChatCommand
    {

        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SendAuctionNotifyOpen));
        public SendAuctionNotifyOpen(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            //AuctionService auctionHouse = new AuctionService(client);
            ItemInstance[] lots = new ItemInstance[0]; //TODO auctionHouse.GetLots();
            ItemInstance[] bids = new ItemInstance[0]; //TODO auctionHouse.GetBids();
            const byte isInMaintenanceMode = 0x0;

            //IBuffer res = BufferProvider.Provide();
            //res.WriteInt32(lots.Length);

            //for (int i = 0; i < lots.Length; i++)
            //{
            //    res.WriteByte((byte)i); // row number?
            //    res.WriteInt32(i); // row number ??
            //    res.WriteInt64(lots[i].ItemID); //spawned item id
            //    res.WriteInt32(lots[i].MinimumBid);
            //    res.WriteInt32(lots[i].BuyoutPrice);
            //    res.WriteFixedString(lots[i].ConsignerName, 49);
            //    res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
            //    res.WriteFixedString(lots[i].Comment, 385);
            //    res.WriteInt16((short)lots[i].CurrentBid); // Bid why convert to short?
            //    res.WriteInt32(lots[i].SecondsUntilExpiryTime);

            //    res.WriteInt32(0); // unknown
            //    res.WriteInt32(0); // unknown
            //}

            //res.WriteInt32(bids.Length); // must be< = 8 | why?

            //for (int i = 0; i < bids.Length; i++)
            //{
            //    res.WriteByte((byte)i); // row number?
            //    res.WriteInt32(i); // row number ??
            //    res.WriteInt64(bids[i].ItemID);
            //    res.WriteInt32(bids[i].MinimumBid); // Lowest
            //    res.WriteInt32(bids[i].BuyoutPrice); // Buy Now
            //    res.WriteFixedString(bids[i].ConsignerName, 49);
            //    res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
            //    res.WriteFixedString(bids[i].Comment, 385); // Comment in the item information
            //    res.WriteInt16((short)bids[i].CurrentBid); // Bid why convert to short?
            //    res.WriteInt32(bids[i].SecondsUntilExpiryTime);

            //    res.WriteInt32(0); // unknown
            //    res.WriteInt32(0); // unknown
            //}

            //res.WriteByte(isInMaintenanceMode); // bool  IsInMaintenanceMode

            IBuffer res = BufferProvider.Provide();
            int numEntries = 0;
            res.WriteInt32(numEntries); //Less than or equal to 0xF

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte((byte)i); // row number?
                res.WriteInt32(i); // row number ??
                res.WriteInt64(bids[i].BaseID);
                res.WriteUInt64(bids[i].MinimumBid); // Lowest
                res.WriteUInt64(bids[i].BuyoutPrice); // Buy Now
                res.WriteFixedString(bids[i].ConsignerName, 49);
                res.WriteByte(1); // 1 permit to show item in the search section ?? flags?
                res.WriteFixedString(bids[i].Comment, 385); // Comment in the item information
                res.WriteInt16((short)bids[i].CurrentBid); // Bid why convert to short?
                res.WriteInt32(bids[i].SecondsUntilExpiryTime);

                res.WriteInt64(0);
                res.WriteInt32(0);
                res.WriteInt32(0);
            }

            res.WriteInt32(numEntries); //Less than or equal to 0xE

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);

                res.WriteInt32(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("soulname", 49);
                res.WriteByte(1);
                res.WriteFixedString("ToBeFound", 385);
                res.WriteInt16(0);
                res.WriteInt32(0);

                res.WriteInt64(0);
                res.WriteInt32(0);
            }

            numEntries = 8;
            res.WriteInt32(numEntries); //Less than or equal to 0x8
            Logger.Debug(((short)ItemQualities.All).ToString());
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFixedString("Joe Bob", 0x49); //V| Search Text
                res.WriteByte(97); //V| Grade min
                res.WriteByte(99); //V| Grade max
                res.WriteByte(97); //V| Level min
                res.WriteByte(99); //V| Level max
                res.WriteInt32(1); // class?
                res.WriteInt16(2); // race?
                res.WriteInt16((short)ItemQualities.All); //V| Qualities
                res.WriteInt64(4); //V| Gold
                res.WriteByte(7);

                res.WriteByte(0); //V| Effectiveness
                res.WriteByte(0); //V| Gem slot 1
                res.WriteByte(0); //V| Gem slot 2
                res.WriteByte(0); //V| Gem slot 3

                res.WriteInt64(8);
                res.WriteInt64(8);
                res.WriteFixedString("Test Search", 0xC1); //v| Saved Search Title
                res.WriteByte(1);
                res.WriteByte(1);
            }

            numEntries = 1;
            res.WriteInt32(numEntries); //Less than or equal to 0x8

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteFixedString("fs0x49V2", 0x49);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteByte(0);
                res.WriteInt64(0);
                res.WriteByte(0);
                res.WriteInt64(0);
                res.WriteInt64(0);
                res.WriteFixedString("fs0xC1V2", 0xC1);//Fixed string of 0xC1 or 0xC1 bytes.
                res.WriteByte(0);
                res.WriteByte(0);
            }

            res.WriteByte(0); //Bool
            res.WriteInt32(60);
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "auct";
    }
}
