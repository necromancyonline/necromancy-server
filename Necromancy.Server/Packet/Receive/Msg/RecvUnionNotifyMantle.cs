using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionNotifyMantle : PacketResponse
    {
        public RecvUnionNotifyMantle()
            : base((ushort)MsgPacketId.recv_union_notify_mantle, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            return res;
        }
    }
}
