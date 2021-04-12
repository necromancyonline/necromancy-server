using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyExp2 : PacketResponse
    {
        public RecvBattleReportNotifyExp2()
            : base((ushort)AreaPacketId.recv_battle_report_notify_exp2, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
