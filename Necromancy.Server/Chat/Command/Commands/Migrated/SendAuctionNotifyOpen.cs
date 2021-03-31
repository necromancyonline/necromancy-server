using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
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
            ItemService itemService = new ItemService(client.Character);
            List<ItemInstance> lots = itemService.GetLots();
            List<ItemInstance> bids = itemService.GetBids();
            List<AuctionEquipmentSearchConditions> equipSearch = itemService.GetEquipmentSearchConditions();
            List<AuctionItemSearchConditions> itemSearch = itemService.GetItemSearchConditions();
            const byte isInMaintenanceMode = 0x0;
            const int max_lots = 15;

            IBuffer res = BufferProvider.Provide();

            foreach (ItemInstance lotItem in lots)
            {
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, lotItem);
                Router.Send(recvItemInstance);
            }
            int j = 0;
            res.WriteInt32(lots.Count); //Less than or equal to 15
            foreach (ItemInstance lotItem in lots)
            {
                res.WriteByte((byte)j); // row number?
                res.WriteInt32(j); // row number ??
                res.WriteUInt64(lotItem.InstanceID);
                res.WriteUInt64(lotItem.MinimumBid);
                res.WriteUInt64(lotItem.BuyoutPrice);
                res.WriteFixedString(lotItem.ConsignerName, 49);
                res.WriteByte(0); // criminal status of seller?
                res.WriteFixedString(lotItem.Comment, 385);
                res.WriteInt16((short)lotItem.CurrentBid); // Bid why convert to short?
                res.WriteInt32(lotItem.SecondsUntilExpiryTime);

                res.WriteInt64(0); //unknown
                res.WriteInt32(0); //unknown
                res.WriteInt32(0); //unknown
                j++;
            }

            foreach (ItemInstance bidItem in bids)
            {
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, bidItem);
                Router.Send(recvItemInstance);
            }
            j = 0;
            res.WriteInt32(bids.Count); //Less than or equal to 0xE
            foreach (ItemInstance bidItem in bids)
            {
                res.WriteByte((byte)j); // row number?
                res.WriteInt32(j); // row number ??
                res.WriteUInt64(bidItem.InstanceID);
                res.WriteUInt64(bidItem.MinimumBid);
                res.WriteUInt64(bidItem.BuyoutPrice);
                res.WriteFixedString(bidItem.ConsignerName, 49);
                res.WriteByte(0); // criminal status of seller?
                res.WriteFixedString(bidItem.Comment, 385);
                res.WriteInt16((short)bidItem.CurrentBid); // The current bid, why convert to short?
                res.WriteInt32(bidItem.SecondsUntilExpiryTime);

                res.WriteInt64(5000); //Your current bid
                res.WriteInt32(0); //0: you are the highest bidder, 1: you won the item, 2: you were outbid, 3: seller cancelled
                j++;
            }

            res.WriteInt32(equipSearch.Count); //Less than or equal to 0x8
            foreach (AuctionEquipmentSearchConditions equipCond in equipSearch)
            {
                res.WriteFixedString(equipCond.Text, AuctionEquipmentSearchConditions.MAX_TEXT_LENGTH); //V| Search Text
                res.WriteByte(equipCond.ForgePriceMin); //V| Grade min
                res.WriteByte(equipCond.ForgePriceMax); //V| Grade max
                res.WriteByte(equipCond.SoulRankMin); //V| Level min
                res.WriteByte(equipCond.SoulRankMax); //V| Level max
                res.WriteInt32((int)equipCond.Class); // class?
                res.WriteInt16((short)equipCond.Race); // race?
                res.WriteInt16((short)equipCond.Qualities); //V| Qualities
                res.WriteUInt64(equipCond.GoldCost); //V| Gold
                res.WriteByte(Convert.ToByte(equipCond.IsLessThanGoldCost));

                res.WriteByte(Convert.ToByte(equipCond.HasGemSlot)); //V| Effectiveness
                res.WriteByte((byte)equipCond.GemSlotType1); //V| Gem slot 1
                res.WriteByte((byte)equipCond.GemSlotType2); //V| Gem slot 2
                res.WriteByte((byte)equipCond.GemSlotType3); //V| Gem slot 3

                res.WriteInt64(0); //TODO UNKNOWN
                res.WriteInt64(0);
                res.WriteFixedString(equipCond.Description, AuctionEquipmentSearchConditions.MAX_DESCRIPTION_LENGTH); //v| Saved Search Description
                res.WriteByte(0); //TODO UNKNOWN
                res.WriteByte(0); //TODO UNKNOWN
            }

            int numEntries = 1;
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
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_auction_notify_open, res, ServerType.Area);

            RecvAuctionNotifyOpenItemStart recvAuctionNotifyOpenItemStart = new RecvAuctionNotifyOpenItemStart(client);
            RecvAuctionNotifyOpenItemEnd recvAuctionNotifyOpenItemEnd = new RecvAuctionNotifyOpenItemEnd(client);

            List<ItemInstance> auctionList = itemService.GetItemsUpForAuction();

            foreach (ItemInstance auctionItem in auctionList)
            {
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, auctionItem);
                Router.Send(recvItemInstance);
            }

            Router.Send(recvAuctionNotifyOpenItemStart);
            int divideBy100 = auctionList.Count / 100 + (auctionList.Count % 100 == 0 ? 0 : 1); // TOTAL NUMBER OF RECVS TO SEND
            for (int i = 0; i < divideBy100; i++)
            {
                RecvAuctionNotifyOpenItem recvAuctionNotifyOpenItem;
                if (i == divideBy100 - 1)
                {
                    recvAuctionNotifyOpenItem = new RecvAuctionNotifyOpenItem(client, auctionList.GetRange(i, auctionList.Count % 100));
                }
                else
                {
                    recvAuctionNotifyOpenItem = new RecvAuctionNotifyOpenItem(client, auctionList.GetRange(i, 100));
                }
                Router.Send(recvAuctionNotifyOpenItem);
            }
            Router.Send(recvAuctionNotifyOpenItemEnd);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "auct";
    }
}
