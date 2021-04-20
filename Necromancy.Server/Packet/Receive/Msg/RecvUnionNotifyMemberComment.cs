using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionNotifyMemberComment : PacketResponse
    {
        public RecvUnionNotifyMemberComment()
            : base((ushort)MsgPacketId.recv_union_notify_member_comment, ServerType.Msg)
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
