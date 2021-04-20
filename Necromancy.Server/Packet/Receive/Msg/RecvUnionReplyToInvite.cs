using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionReplyToInvite : PacketResponse
    {
        public RecvUnionReplyToInvite()
            : base((ushort)MsgPacketId.recv_union_reply_to_invite_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
