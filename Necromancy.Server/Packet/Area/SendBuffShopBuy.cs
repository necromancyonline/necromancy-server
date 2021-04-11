using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendBuffShopBuy : ClientHandler
    {
        public SendBuffShopBuy(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_buff_shop_buy;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvBuffShopBuyR buffShopBuy = new RecvBuffShopBuyR();
            router.Send(buffShopBuy, client);
        }
    }
}
