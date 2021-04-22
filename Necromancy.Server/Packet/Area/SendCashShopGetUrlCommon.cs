using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCashShopGetUrlCommon : ClientHandler
    {
        public SendCashShopGetUrlCommon(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_cash_shop_get_url_common;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int data = packet.data.ReadInt32();

            RecvCashShopGetUrlCommon getUrlCommon = new RecvCashShopGetUrlCommon();
            router.Send(getUrlCommon, client);
        }
    }
}
