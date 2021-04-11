using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_request_load_msg : ClientHandler
    {
        public send_friend_request_load_msg(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_friend_request_load_msg;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_friend_request_load_r, res, ServerType.Msg);
        }
    }
}
