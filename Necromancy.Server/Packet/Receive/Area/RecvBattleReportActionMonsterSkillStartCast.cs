using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvBattleReportActionMonsterSkillStartCast : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly int _skillId;

        public RecvBattleReportActionMonsterSkillStartCast(uint instanceId, int skillId)
            : base((ushort)AreaPacketId.recv_battle_report_action_monster_skill_start_cast, ServerType.Area)
        {
            _instanceId = instanceId;
            _skillId = skillId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_skillId); // From skill_base.csv
            res.WriteUInt32(_instanceId); //  ????????????????????
            res.WriteFloat(2.0F); //  ????????????????????
            return res;
        }
    }
}
