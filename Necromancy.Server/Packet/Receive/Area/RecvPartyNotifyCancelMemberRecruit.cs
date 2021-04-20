using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyCancelMemberRecruit : PacketResponse
    {
        public RecvPartyNotifyCancelMemberRecruit()
            : base((ushort)AreaPacketId.recv_party_notify_cancel_member_recruit, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
