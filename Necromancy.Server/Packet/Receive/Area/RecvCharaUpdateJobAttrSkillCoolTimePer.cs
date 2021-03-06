using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateJobAttrSkillCoolTimePer : PacketResponse
    {
        public RecvCharaUpdateJobAttrSkillCoolTimePer()
            : base((ushort)AreaPacketId.recv_chara_update_job_attr_skill_cooltime_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt16(0); //Percentage most likely
            return res;
        }
    }
}
