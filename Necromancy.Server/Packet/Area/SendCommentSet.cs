using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCommentSet : ClientHandler
    {
        public SendCommentSet(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_comment_set;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_comment_set_r, res, ServerType.Area);
        }
    }
}
