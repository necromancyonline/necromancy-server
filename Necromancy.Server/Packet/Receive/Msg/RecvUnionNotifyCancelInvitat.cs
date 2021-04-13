using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionNotifyCancelInvitat : PacketResponse
    {
        public RecvUnionNotifyCancelInvitat()
            : base((ushort)MsgPacketId.recv_union_notify_cancel_invitat, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteCString(""); //max size 0x31
            return res;
        }
    }
}
