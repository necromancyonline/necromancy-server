using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvEasyFriendNotifyDeleteMember : PacketResponse
    {
        public RecvEasyFriendNotifyDeleteMember()
            : base((ushort)MsgPacketId.recv_easy_friend_notify_delete_member, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
