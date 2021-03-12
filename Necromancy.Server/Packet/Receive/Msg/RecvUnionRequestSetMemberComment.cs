using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionRequestSetMemberComment : PacketResponse
    {
        public RecvUnionRequestSetMemberComment()
            : base((ushort) MsgPacketId.recv_union_request_set_member_comment_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //missing
            return res;
        }
    }
}
