using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_shop_close : ClientHandler
    {
        public send_shop_close(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_shop_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            RecvShopClose shopClose = new RecvShopClose();
            Router.Send(shopClose, client);

            //RecvShopNotifyClose notifyClose = new RecvShopNotifyClose();
            //Router.Send(client.Map, notifyClose, client);//Causes other client's shops to close, can't be used on self.

            RecvEventSync syncEvent = new RecvEventSync();
            Router.Send(syncEvent, client);
        }
    }
}
