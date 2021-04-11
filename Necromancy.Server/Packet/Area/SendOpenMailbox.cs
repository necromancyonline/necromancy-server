using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendOpenMailbox : ClientHandler
    {
        public SendOpenMailbox(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_open_mailbox;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);

            router.Send(client, (ushort) AreaPacketId.recv_mail_open_r, res, ServerType.Area);
        }
    }
}
