using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendShopClose : ClientHandler
    {
        public SendShopClose(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_shop_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvShopClose shopClose = new RecvShopClose();
            router.Send(shopClose, client);

            //RecvShopNotifyClose notifyClose = new RecvShopNotifyClose();
            //Router.Send(client.Map, notifyClose, client);//Causes other client's shops to close, can't be used on self.

            RecvEventSync syncEvent = new RecvEventSync();
            router.Send(syncEvent, client);
        }
    }
}
