using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvChannelSelect : PacketResponse
    {
        public RecvChannelSelect()
            : base((ushort)MsgPacketId.recv_channel_select_r, ServerType.Msg)
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
