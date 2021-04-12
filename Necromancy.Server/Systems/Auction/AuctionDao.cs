using System;
using Necromancy.Server.Model;

namespace Necromancy.Server.Systems.Auction
{
    public class AuctionDao : DatabaseAccessObject
    {
        private const string _SqlCreateItemsUpForAuctionView = @"
            DROP VIEW IF EXISTS items_up_for_auction;
            CREATE VIEW IF NOT EXISTS items_up_for_auction
	            (
		            id,
                    consigner_id,
		            consigner_name,
		            instance_id,
		            quantity,
		            expiry_datetime,
		            min_bid,
		            buyout_price,
		            current_bid,
		            bidder_id,
		            comment,
                    is_cancellable
	            )
            AS
            SELECT
                nec_auction.id,
                consigner.id,
                consigner.name,
                nec_auction.instance_id,
                nec_auction.quantity,
                nec_auction.expiry_datetime,
                nec_auction.min_bid,
                nec_auction.buyout_price,
                nec_auction.current_bid,
                nec_auction.bidder_id,
                nec_auction.comment,
                nec_auction.is_cancellable
            FROM
                nec_auction
			INNER JOIN
				nec_item_instance item_instance
			ON
				nec_auction.instance_id = item_instance.id
            INNER JOIN
                nec_character consigner
            ON
                item_instance.owner_id = consigner.id";

        private const string _SqlInsertLot = @"
            INSERT INTO
                nec_auction
                (
                    slot
                    instance_id,
                    quantity,
                    expiry_datetime,
                    min_bid,
                    buyout_price,
                    comment
                )
            VALUES
                (
                    @slot
                    @instance_id,
                    @quantity,
                    @expiry_datetime,
                    @min_bid,
                    @buyout_price,
                    @comment
                )";

        private const string _SqlUpdateBid = @"
            UPDATE
                nec_auction_item
            SET
                bidder_id = @bidder_id,
                current_bid = @current_bid,
                is_cancellable = (bidder_id IS NULL)
            WHERE
                id = @id";

        private const string _SqlSelectBids = @"
            SELECT
                *
            FROM
                items_up_for_auction
            WHERE
                bidder_id = @character_id";

        private const string _SqlSelectLots = @"
            SELECT
                *
            FROM
                items_up_for_auction
            WHERE
                consigner_id = @character_id";

        private const string _SqlSelectItem = @"
            SELECT
                *
            FROM
                items_up_for_auction
            WHERE
                id = @id";

        private const string _SqlSelectItemsByCriteria = @"";


        public AuctionDao()
        {
            CreateView();
        }

        private void CreateView()
        {
            ExecuteNonQuery(_SqlCreateItemsUpForAuctionView, command => { });
        }

        //public bool InsertLot(ItemInstance auctionLot)
        //{
        //      int rowsAffected = ExecuteNonQuery(SqlInsertLot, command =>
        //        {
        //            AddParameter(command, "@slot", auctionLot.Slot);
        //            AddParameter(command, "@instance_id", auctionLot.ItemInstanceId);
        //            AddParameter(command, "@quantity", auctionLot.Quantity);
        //            AddParameter(command, "@expiry_datetime", CalcExpiryTime(auctionLot.SecondsUntilExpiryTime));
        //            AddParameter(command, "@min_bid", auctionLot.MinimumBid);
        //            AddParameter(command, "@buyout_price", auctionLot.BuyoutPrice);
        //            AddParameter(command, "@comment", auctionLot.Comment);
        //        });
        //    return rowsAffected > 0;
        //}

        //public AuctionLot SelectItem(int auctionItemId)
        //{
        //    AuctionLot auctionItem = new AuctionLot();
        //    ExecuteReader(SqlSelectItem,
        //        command =>
        //        {
        //            AddParameter(command, "@id", auctionItemId);
        //        }, reader =>
        //        {
        //            MakeAuctionLot(reader);
        //        });
        //    return auctionItem;
        //}

        //public bool UpdateBid(AuctionLot auctionItem)
        //{
        //    int rowsAffected = ExecuteNonQuery(SqlUpdateBid, command =>
        //    {
        //        AddParameter(command, "@bidder_id", auctionItem.BidderId);
        //        AddParameter(command, "@current_bid", auctionItem.CurrentBid);
        //    });
        //    return rowsAffected > 0;
        //}

        //public AuctionLot[] SelectBids(Character character)
        //{
        //    AuctionLot[] bids = new AuctionLot[AuctionService.MAX_BIDS];
        //    int i = 0;
        //    ExecuteReader(SqlSelectBids,
        //        command =>
        //        {
        //            AddParameter(command, "@character_id", character.Id);
        //        }, reader =>
        //        {
        //            while (reader.Read())
        //            {
        //                if (i >= AuctionService.MAX_BIDS) break;
        //                AuctionLot bid = MakeAuctionLot(reader);
        //                bids[i] = bid;
        //                i++;
        //            }
        //        });
        //    AuctionLot[] truncatedBids = new AuctionLot[i];
        //    Array.Copy(bids, truncatedBids, i);
        //    return truncatedBids;
        //}

        //public AuctionLot[] SelectLots(Character character)
        //{
        //    AuctionLot[] lots = new AuctionLot[AuctionService.MAX_LOTS];
        //    int i = 0;
        //    ExecuteReader(SqlSelectLots,
        //        command =>
        //        {
        //            AddParameter(command, "@character_id", character.Id);
        //        }, reader =>
        //        {
        //            while (reader.Read())
        //            {
        //                if (i >= AuctionService.MAX_LOTS) break;
        //                AuctionLot lot = MakeAuctionLot(reader);
        //                lots[i] = lot;
        //                i++;
        //            }
        //        });
        //    AuctionLot[] truncatedLots = new AuctionLot[i];
        //    Array.Copy(lots, truncatedLots, i);
        //    return truncatedLots;
        //}

        //private AuctionLot MakeAuctionLot(DbDataReader reader)
        //{
        //    AuctionLot auctionItem = new AuctionLot();
        //    auctionItem.Id = reader.GetInt32("id");
        //    auctionItem.ConsignerId = reader.GetInt32("consigner_id");
        //    auctionItem.ConsignerName = reader.GetString("consigner_name");
        //    auctionItem.ItemInstanceId = (ulong) reader.GetInt64("spawn_id");
        //    auctionItem.Quantity = reader.GetInt32("quantity");
        //    auctionItem.SecondsUntilExpiryTime = CalcSecondsToExpiry(reader.GetInt64("expiry_datetime"));
        //    auctionItem.MinimumBid = (ulong) reader.GetInt64("min_bid");
        //    auctionItem.BuyoutPrice = (ulong) reader.GetInt64("buyout_price");
        //    auctionItem.CurrentBid = reader.GetInt32("current_bid");
        //    auctionItem.BidderId = reader.GetInt32("bidder_id");
        //    auctionItem.Comment = reader.GetString("comment");
        //    return auctionItem;
        //}

        private int CalcSecondsToExpiry(long unixTimeSecondsExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return (int)(unixTimeSecondsExpiry - dOffsetNow.ToUnixTimeSeconds());
        }

        private long CalcExpiryTime(int secondsToExpiry)
        {
            DateTime dNow = DateTime.Now;
            DateTimeOffset dOffsetNow = new DateTimeOffset(dNow);
            return dOffsetNow.ToUnixTimeSeconds() + secondsToExpiry;
        }

        public int SelectGold(Character character)
        {
            throw new NotImplementedException();
        }

        public void AddGold(Character character, int amount)
        {
            throw new NotImplementedException();
        }

        public void SubtractGold(Character character, int amount)
        {
            throw new NotImplementedException();
        }

        //public AuctionLot[] SelectItemsByCriteria(SearchCriteria searchCriteria)
        //{
        //    AuctionLot[] results = new AuctionLot[1];
        //    ExecuteReader(SqlSelectItemsByCriteria,
        //        command =>
        //        {

        //        }, reader =>
        //        {
        //            while (reader.Read()) {
        //            //TODO do something
        //            }
        //        });
        //    throw new NotImplementedException();
        //}
    }
}
