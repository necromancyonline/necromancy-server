using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionDeregistSearchItemCond : ClientHandler
    {
        public SendAuctionDeregistSearchItemCond(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_auction_deregist_search_item_cond;

        public override void Handle(NecClient client, NecPacket packet)
        {
            byte preset = packet.data.ReadByte(); //Provides the preset number to delete.

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_auction_deregist_search_item_cond_r, res, ServerType.Area);
        }
    }
}
