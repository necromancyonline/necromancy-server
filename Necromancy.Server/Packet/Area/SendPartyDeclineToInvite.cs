using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyDeclineToInvite : ClientHandler
    {
        public SendPartyDeclineToInvite(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_decline_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);

            router.Send(client, (ushort) AreaPacketId.recv_party_decline_to_invite_r, res, ServerType.Area);
        }
    }
}
