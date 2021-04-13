using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvFriendRequestSetComment : PacketResponse
    {
        public RecvFriendRequestSetComment()
            : base((ushort)MsgPacketId.recv_friend_request_set_comment_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString("");
            return res;
        }
    }
}
