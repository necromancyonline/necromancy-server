using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillStartCast : PacketResponse
    {
        private readonly float _castingTime;
        private readonly uint _instanceId;
        private readonly int _skillId;

        public RecvSkillStartCast(uint instanceId, int skillId, float castingTime)
            : base((ushort)AreaPacketId.send_skill_start_cast, ServerType.Area)
        {
            _instanceId = instanceId;
            _skillId = skillId;
            _castingTime = castingTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId); //Error check     | 0 - success  
            res.WriteInt32(_skillId); //previously Skill ID
            res.WriteFloat(_castingTime);
            return res;
        }
    }
}
