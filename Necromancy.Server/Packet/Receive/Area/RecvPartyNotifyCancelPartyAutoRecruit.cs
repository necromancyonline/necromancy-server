using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyCancelPartyAutoRecruit : PacketResponse
    {
        public RecvPartyNotifyCancelPartyAutoRecruit()
            : base((ushort) AreaPacketId.recv_party_notify_cancel_party_auto_recruit, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No Structure

            return res;
        }
    }
}
