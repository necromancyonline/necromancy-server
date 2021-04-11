using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleOpenCashShop : ClientHandler
    {
        public SendRaisescaleOpenCashShop(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_raisescale_open_cash_shop;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_raisescale_open_cash_shop_r, res, ServerType.Area);
        }
    }
}
