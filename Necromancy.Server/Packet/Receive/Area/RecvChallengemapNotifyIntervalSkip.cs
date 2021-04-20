using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapNotifyIntervalSkip : PacketResponse
    {
        public RecvChallengemapNotifyIntervalSkip()
            : base((ushort)AreaPacketId.recv_challengemap_notify_interval_skip, ServerType.Area)
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
