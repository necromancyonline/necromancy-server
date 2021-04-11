using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventSyncR : ClientHandler
    {
        public SendEventSyncR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_sync_r;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            //Router.Send(client.Map, (ushort)AreaPacketId.recv_event_sync, res, ServerType.Area);

            IBuffer res9 = BufferProvider.Provide();
            res9.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res9, ServerType.Area);
        }


    }
}
