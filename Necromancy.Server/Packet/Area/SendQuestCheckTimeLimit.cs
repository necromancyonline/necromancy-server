using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestCheckTimeLimit : ClientHandler
    {
        public SendQuestCheckTimeLimit(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_quest_check_time_limit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(1); //Continuous send of this packet if 0(time left is 0 if this is 0)
            router.Send(client, (ushort)AreaPacketId.recv_quest_check_time_limit_r, res, ServerType.Area);
        }
    }
}
