using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventSystemMessageTimerR : ClientHandler
    {
        public SendEventSystemMessageTimerR(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_system_message_timer_r;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound");
            router.Send(client.map, (ushort)AreaPacketId.recv_event_system_message_timer, res, ServerType.Area);
        }

    }
}
