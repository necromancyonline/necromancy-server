using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Systems.Auction
{
    public class SendAuctionReceiveItem : ClientHandler
    {
        public SendAuctionReceiveItem(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_auction_receive_item;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); /*

            0 = Work.

            error code :

            1. This item may not be listed.
            2. You may not list the equiped items
            */
            router.Send(client.map, (ushort)AreaPacketId.recv_auction_receive_item_r, res, ServerType.Area);
        }
    }
}
