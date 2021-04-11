using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateSkillTrapCasttime : PacketResponse
    {
        public RecvCharaUpdateSkillTrapCasttime()
            : base((ushort) AreaPacketId.recv_chara_update_skill_trap_casttime, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
