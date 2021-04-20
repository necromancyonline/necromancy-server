using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyCancelInvitation : PacketResponse
    {
        public RecvPartyNotifyCancelInvitation()
            : base((ushort)MsgPacketId.recv_party_notify_cancel_invitation, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            return res;
        }
    }
}
