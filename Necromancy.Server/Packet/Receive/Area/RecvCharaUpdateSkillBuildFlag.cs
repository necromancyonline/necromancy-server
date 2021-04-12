using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateSkillBuildFlag : PacketResponse
    {
        public RecvCharaUpdateSkillBuildFlag()
            : base((ushort)AreaPacketId.recv_chara_update_skill_build_flag, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(numEntries); //less than 0x4
            for (int k = 0; k < numEntries; k++) res.WriteInt64(0);
            return res;
        }
    }
}
