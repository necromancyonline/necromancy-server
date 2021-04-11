using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendStorageOpenCashShop : ClientHandler
    {
        public SendStorageOpenCashShop(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_storage_open_cash_shop;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            //Router.Send(client.Map, (ushort) AreaPacketId.recv_auction_bid_r, res, ServerType.Area);
        }
    }
}
