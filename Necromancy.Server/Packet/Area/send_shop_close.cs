using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
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
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_shop_close_r, res, ServerType.Area);
            SendShopNotifyClose(client);
        }

        private void SendShopNotifyClose(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_shop_notify_close, res, ServerType.Area, client);

            IBuffer res2 = BufferProvider.Provide();
            Router.Send(client, (ushort)AreaPacketId.recv_event_sync, res2, ServerType.Area);
        }
    }
}
