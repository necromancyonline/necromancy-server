using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyMentorNotifyCreate : PacketResponse
    {
        public RecvPartyMentorNotifyCreate()
            : base((ushort) MsgPacketId.recv_party_mentor_notify_create, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            
            return res;
        }
    }
}
