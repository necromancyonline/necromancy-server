using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapNotifyAreaMemberCancel : PacketResponse
    {
        public RecvChallengemapNotifyAreaMemberCancel()
            : base((ushort)AreaPacketId.recv_challengemap_notify_area_member_cancel, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //no structure
            return res;
        }
    }
}
