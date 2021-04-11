using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_event_system_message_timer_r : ClientHandler
    {
        public send_event_system_message_timer_r(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort)AreaPacketId.send_event_system_message_timer_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound");
            Router.Send(client.Map, (ushort)AreaPacketId.recv_event_system_message_timer, res, ServerType.Area);
        }

    }
}
