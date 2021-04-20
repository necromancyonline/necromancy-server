using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionSkillStartCast : PacketResponse
    {
        private readonly int _skillId;

        public RecvBattleReportActionSkillStartCast(int skillId)
            : base((ushort)AreaPacketId.recv_battle_report_action_skill_start_cast, ServerType.Area)
        {
            _skillId = skillId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_skillId); // From skill_base.csv
            return res;
        }
    }
}
