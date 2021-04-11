using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendAuctionReExhibit : ClientHandler
    {
        public SendAuctionReExhibit(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_auction_re_exhibit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0);
            router.Send(client.map, (ushort) AreaPacketId.recv_auction_re_exhibit_r, res, ServerType.Area);
        }
    }
}
