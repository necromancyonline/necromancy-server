using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendCommentSwitch : ClientHandler
    {
        public SendCommentSwitch(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_comment_switch;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            res.WriteInt32(0);

            router.Send(client, (ushort)AreaPacketId.recv_comment_switch_r, res, ServerType.Area);
        }
    }
}
