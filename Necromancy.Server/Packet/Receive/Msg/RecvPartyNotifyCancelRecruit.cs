using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyCancelRecruit : PacketResponse
    {
        public RecvPartyNotifyCancelRecruit()
            : base((ushort)MsgPacketId.recv_party_notify_cancel_recruit, ServerType.Msg)
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
