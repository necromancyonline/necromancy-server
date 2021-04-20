using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportNotifyTargetBonus : PacketResponse
    {
        public RecvBattleReportNotifyTargetBonus()
            : base((ushort)AreaPacketId.recv_battle_report_notify_target_bonus, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //third time.  forgot to write down the structure.  geez xdbg
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
