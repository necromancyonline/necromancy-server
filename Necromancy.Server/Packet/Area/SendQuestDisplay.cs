using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestDisplay : ClientHandler
    {
        public SendQuestDisplay(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_quest_display;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_quest_display_r, res, ServerType.Area);
        }

    }
}
