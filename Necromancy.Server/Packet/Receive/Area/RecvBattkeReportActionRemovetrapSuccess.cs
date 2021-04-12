using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattkeReportActionRemovetrapSuccess : PacketResponse
    {
        public RecvBattkeReportActionRemovetrapSuccess()
            : base((ushort)AreaPacketId.recv_battle_report_action_removetrap_success, ServerType.Area)
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
