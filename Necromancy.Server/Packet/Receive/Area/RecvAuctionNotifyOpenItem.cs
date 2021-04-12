using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionNotifyOpenItem : PacketResponse
    {

        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(RecvAuctionNotifyOpenItem));
        List<ItemInstance> _auctionList;
        public RecvAuctionNotifyOpenItem(NecClient necClient, List<ItemInstance> auctionList) : base((ushort) AreaPacketId.recv_auction_notify_open_item, ServerType.Area) 
        {
            Clients.Add(necClient);
            _auctionList = auctionList;
        }
        protected override IBuffer ToBuffer() 
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_auctionList.Count); // cmp to 0x64 = 100
            int i = 0;
            foreach (ItemInstance auctionItem in _auctionList)
            {
                res.WriteInt32(auctionItem.Location.Slot); //row identifier?
                res.WriteUInt64(auctionItem.InstanceID);
                res.WriteUInt64(auctionItem.MinimumBid);
                res.WriteUInt64(auctionItem.BuyoutPrice);
                res.WriteFixedString(auctionItem.ConsignerSoulName, 49);
                res.WriteByte(0); // appears to be boolean if owner criminal it is 1
                res.WriteFixedString(auctionItem.Comment, 385);
                res.WriteInt16((short)auctionItem.CurrentBid);
                res.WriteInt32(auctionItem.SecondsUntilExpiryTime);
                i++;
            }
            return res;
        }
    }
}
