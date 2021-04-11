using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendFriendRequestLoadMsg : ClientHandler
    {
        public SendFriendRequestLoadMsg(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_friend_request_load_msg;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) MsgPacketId.recv_friend_request_load_r, res, ServerType.Msg);
        }
    }
}
