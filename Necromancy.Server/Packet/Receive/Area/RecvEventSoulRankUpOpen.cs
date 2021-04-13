using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSoulRankUpOpen : PacketResponse
    {
        public RecvEventSoulRankUpOpen()
            : base((ushort)AreaPacketId.recv_event_soul_rankup_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
