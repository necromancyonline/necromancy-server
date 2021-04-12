using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionMonsterSkillExec : PacketResponse
    {
        private readonly int _skillId;

        public RecvBattleReportActionMonsterSkillExec(int skillId)
            : base((ushort)AreaPacketId.recv_battle_report_action_monster_skill_exec, ServerType.Area)
        {
            _skillId = skillId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_skillId);
            return res;
        }
    }
}
