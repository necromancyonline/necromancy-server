using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestCheckTarget : ClientHandler
    {
        public SendQuestCheckTarget(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort) AreaPacketId.send_quest_check_target;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) AreaPacketId.recv_quest_check_target_r, res, ServerType.Area); //Virtual function call?  add logic at later time
        }

    }
}
