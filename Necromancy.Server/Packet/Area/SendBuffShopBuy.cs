using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_buff_shop_buy : ClientHandler
    {
        public send_buff_shop_buy(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_buff_shop_buy;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvBuffShopBuyR buffShopBuy = new RecvBuffShopBuyR();
            Router.Send(buffShopBuy, client);
        }
    }
}
