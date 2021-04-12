using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSkillExecR : PacketResponse
    {
        private readonly float _coolTime;
        private readonly int _errorCode;
        private readonly float _rigidityTime;

        public RecvSkillExecR(int errorCode, float coolTime, float rigidityTime)
            : base((ushort)AreaPacketId.recv_skill_exec_r, ServerType.Area)
        {
            _errorCode = errorCode;
            _coolTime = coolTime;
            _rigidityTime = rigidityTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_errorCode); //see sys_msg.csv
            /*
                -1      Unable to use skill
                -1322   Incorrect target
                -1325   Insufficient usage count for Power Level
                1       Not enough distance
                GENERIC Unable to use skill: < errcode >
            */
            res.WriteFloat(_coolTime); //Cool time   2   ./Skill_base.csv   Column J 
            res.WriteFloat(_rigidityTime); //Rigidity time 1 ./Skill_base.csv   Column L  
            return res;
        }
    }
}
