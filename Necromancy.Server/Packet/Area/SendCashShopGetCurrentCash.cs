using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCashShopGetCurrentCash : ClientHandler
    {
        public SendCashShopGetCurrentCash(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_cash_shop_get_current_cash;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvCashShopGetCurrentCash getCash = new RecvCashShopGetCurrentCash();
            router.Send(getCash, client);
        }
    }
}
