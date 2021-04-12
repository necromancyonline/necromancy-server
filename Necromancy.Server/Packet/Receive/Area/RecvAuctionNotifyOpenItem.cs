using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionNotifyOpenItem : PacketResponse
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(RecvAuctionNotifyOpenItem));
        private readonly List<ItemInstance> _auctionList;

        public RecvAuctionNotifyOpenItem(NecClient necClient, List<ItemInstance> auctionList) : base((ushort)AreaPacketId.recv_auction_notify_open_item, ServerType.Area)
        {
            clients.Add(necClient);
            _auctionList = auctionList;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_auctionList.Count); // cmp to 0x64 = 100
            int i = 0;
            foreach (ItemInstance auctionItem in _auctionList)
            {
                res.WriteInt32(auctionItem.location.slot); //row identifier?
                res.WriteUInt64(auctionItem.instanceId);
                res.WriteUInt64(auctionItem.minimumBid);
                res.WriteUInt64(auctionItem.buyoutPrice);
                res.WriteFixedString(auctionItem.consignerSoulName, 49);
                res.WriteByte(0); // appears to be boolean if owner criminal it is 1
                res.WriteFixedString(auctionItem.comment, 385);
                res.WriteInt16((short)auctionItem.currentBid);
                res.WriteInt32(auctionItem.secondsUntilExpiryTime);
                i++;
            }

            return res;
        }
    }
}
