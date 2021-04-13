using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventSelectReady : PacketResponse
    {
        public RecvEventSelectReady()
            : base((ushort)AreaPacketId.recv_event_select_ready, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteByte(0); // bool
            return res;
        }
    }
}
