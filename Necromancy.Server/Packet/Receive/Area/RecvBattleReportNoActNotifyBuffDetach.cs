using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNoActNotifyBuffDetach : PacketResponse
    {
        public RecvBattleReportNoActNotifyBuffDetach()
            : base((ushort)AreaPacketId.recv_battle_report_noact_notify_buff_detach, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0); //bool
            return res;
        }
    }
}
