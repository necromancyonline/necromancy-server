using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillStartCastExR : PacketResponse
    {
        private readonly float _castingTime;
        private readonly uint _instanceId;
        private readonly int _skillId;

        public RecvSkillStartCastExR(uint instanceId, int skillId, float castingTime)
            : base((ushort)AreaPacketId.recv_skill_start_cast_ex_r, ServerType.Area)
        {
            _instanceId = instanceId;
            _castingTime = castingTime;
            _skillId = skillId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error check     | 0 - success  See other codes above in SendSkillStartCast
            res.WriteFloat(_castingTime); //casting time (countdown before auto-cast)    ./Skill_base.csv   Column L

            res.WriteInt32(100); //Cast Script?     ./Skill_base.csv   Column T
            res.WriteInt32(100); //Effect Script    ./Skill_base.csv   Column V
            res.WriteInt32(100); //Effect ID?   ./Skill_base.csv   Column X 
            res.WriteInt32(0100); //Effect ID 2     ./Skill_base.csv   Column Z 

            res.WriteInt32(_skillId); //

            res.WriteInt32(10000); //Distance?              ./Skill_base.csv   Column AN 
            res.WriteInt32(10000); //Height?                 ./Skill_base.csv   Column AO 
            res.WriteInt32(500); //??                          ./Skill_base.csv   Column AP 
            res.WriteInt32(1); //??                       ./Skill_base.csv   Column AQ 

            res.WriteInt32(5); // Effect time?
            return res;
        }
    }
}
