using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionNotifyGrowth : PacketResponse
    {
        public RecvUnionNotifyGrowth()
            : base((ushort) MsgPacketId.recv_union_notify_growth, ServerType.Msg)
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
