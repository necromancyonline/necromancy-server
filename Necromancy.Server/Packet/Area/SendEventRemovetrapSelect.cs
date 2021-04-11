using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendEventRemovetrapSelect : ClientHandler
    {
        public SendEventRemovetrapSelect(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_event_removetrap_select;

        public override void Handle(NecClient client, NecPacket packet)
        {

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort)AreaPacketId.recv_event_removetrap_select_r, res, ServerType.Area);
        }

    }
}
