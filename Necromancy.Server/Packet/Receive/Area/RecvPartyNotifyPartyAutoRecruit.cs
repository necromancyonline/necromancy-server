using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyPartyAutoRecruit : PacketResponse
    {
        public RecvPartyNotifyPartyAutoRecruit()
            : base((ushort) AreaPacketId.recv_party_notify_party_auto_recruit, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteCString("What");
            res.WriteCString("What");
            return res;
        }
    }
}
