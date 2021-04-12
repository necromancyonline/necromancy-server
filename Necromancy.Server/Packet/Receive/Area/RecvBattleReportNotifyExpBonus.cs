using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyExpBonus : PacketResponse
    {
        public RecvBattleReportNotifyExpBonus()
            : base((ushort)AreaPacketId.recv_battle_report_notify_exp_bonus, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0); //current exp
            res.WriteInt32(0); //bonus exp
            return res;
        }
    }
}
