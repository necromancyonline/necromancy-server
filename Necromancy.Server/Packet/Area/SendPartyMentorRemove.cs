using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyMentorRemove : ClientHandler
    {
        public SendPartyMentorRemove(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_party_mentor_remove;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client.map, (ushort)AreaPacketId.recv_party_mentor_remove_r, res, ServerType.Area);
        }
    }
}
