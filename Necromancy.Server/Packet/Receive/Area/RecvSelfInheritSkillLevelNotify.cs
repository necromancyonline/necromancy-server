using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfInheritSkillLevelNotify : PacketResponse
    {
        public RecvSelfInheritSkillLevelNotify()
            : base((ushort)AreaPacketId.recv_self_inherit_skill_level_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            return res;
        }
    }
}
