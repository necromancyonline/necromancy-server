using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_temple_close : ClientHandler
    {
        public send_temple_close(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_temple_close;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_temple_close_r, res, ServerType.Area);
            SendTempleNotifyClose(client);
        }

        private void SendTempleNotifyClose(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            Router.Send(client.Map, (ushort) AreaPacketId.recv_temple_notify_close, res, ServerType.Area, client);

        }
    }
}
