using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyRegistPartyRecruit : ClientHandler
    {
        public SendPartyRegistPartyRecruit(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_regist_party_recruit;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int objectiveId = packet.data.ReadInt32(); // always 0, 1, or 2. based on drop down selection
            int detailsId = packet.data.ReadInt32(); // Can Be Dungeon ID or Quest ID based on objective
            uint targetId = packet.data.ReadUInt32(); // Unknown. possbly Target ID???
            int otherId = packet.data.ReadInt32(); // Selected check box under other
            string comment = packet.data.ReadFixedString(60); //Comment Box accepts up to 60 characters.

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(objectiveId);
            res.WriteInt32(detailsId);
            res.WriteUInt32(targetId);
            res.WriteInt32(otherId);

            if (targetId != 0)
                router.Send(server.clients.GetByCharacterInstanceId(targetId), (ushort)AreaPacketId.recv_party_notify_recruit_request, res, ServerType.Area);
            else
                router.Send(client, (ushort)AreaPacketId.recv_party_notify_recruit_request, res, ServerType.Area);
            // make an event popup message that says A  p  p  l  y  .  v  i  a  .  t  h  e  .  P  a  r  t  y  .  R  e  c  e  p  t  i  o  n  .  S  t  a  f  f  .  P
        }
    }
}
