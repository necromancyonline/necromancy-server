using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Packet.Receive.Area
{
    class RecvAuctionNotifyOpenItemStart : PacketResponse
    {
        public RecvAuctionNotifyOpenItemStart(NecClient necClient) : base((ushort)AreaPacketId.recv_auction_notify_open_item_start, ServerType.Area)
        {
            Clients.Add(necClient);
        }
        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            return res;
        }
    }
}