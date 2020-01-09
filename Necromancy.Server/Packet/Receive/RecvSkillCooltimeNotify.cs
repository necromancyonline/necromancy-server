using Arrowgene.Services.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvSkillCooltimeNotify : PacketResponse
    {
        private readonly uint _instanceId;
        private readonly float _coolTime1;
        private readonly float _coolTime2;
        public RecvSkillCooltimeNotify(uint instanceId, float coolTime1, float coolTime2)
            : base((ushort) AreaPacketId.recv_skill_cooltime_notify, ServerType.Area)
        {
            _instanceId = instanceId;
            _coolTime1 = coolTime1;
            _coolTime2 = coolTime2;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_instanceId); //previously Skill ID
            res.WriteFloat(_coolTime1);
            return res;
        }
    }
}
