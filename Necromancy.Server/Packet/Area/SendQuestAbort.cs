using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_abort : ClientHandler
    {
        public send_quest_abort(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_quest_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int abortQuestNumber = packet.Data.ReadInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(abortQuestNumber);
            Router.Send(client, (ushort) AreaPacketId.recv_quest_abort_r, res, ServerType.Area);
        }

    }
}
