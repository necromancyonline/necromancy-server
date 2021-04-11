using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_sync_r : ClientHandler
    {
        public send_event_sync_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_sync_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            //Router.Send(client.Map, (ushort)AreaPacketId.recv_event_sync, res, ServerType.Area);

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteByte(0);
            Router.Send(client, (ushort)AreaPacketId.recv_event_end, res9, ServerType.Area);
        }


    }
}
