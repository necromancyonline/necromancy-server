using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_open_mailbox : ClientHandler
    {
        public send_open_mailbox(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_open_mailbox;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);

            Router.Send(client, (ushort) AreaPacketId.recv_mail_open_r, res, ServerType.Area);
        }
    }
}
