using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendQuestGetSoulMissionQuestHistory : ClientHandler
    {
        public SendQuestGetSoulMissionQuestHistory(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_quest_get_soul_mission_quest_history;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);//Bool
            //Commented out until further testing. this is called at login
            //Router.Send(client, (ushort) AreaPacketId.recv_quest_get_soul_mission_quest_history, res, ServerType.Area);
        }
    }
}
