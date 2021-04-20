using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillStartCastSelf : PacketResponse
    {
        private readonly float _castingTime;
        private readonly int _skillId;

        public RecvSkillStartCastSelf(int skillId, float castingTime)
            : base((ushort)AreaPacketId.recv_skill_start_cast_self, ServerType.Area)
        {
            _skillId = skillId;
            _castingTime = castingTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_skillId); //previously Skill ID
            res.WriteFloat(_castingTime);
            return res;
        }
    }
}
