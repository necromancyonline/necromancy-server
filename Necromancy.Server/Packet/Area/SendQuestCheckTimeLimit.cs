using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class send_quest_check_time_limit : ClientHandler
    {
        public send_quest_check_time_limit(NecServer server) : base(server)
        {
        }
        

        public override ushort Id => (ushort) AreaPacketId.send_quest_check_time_limit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //Continuous send of this packet if 0(time left is 0 if this is 0)
            Router.Send(client, (ushort) AreaPacketId.recv_quest_check_time_limit_r, res, ServerType.Area);
        }

    }
}
