using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateSkillApCostCut : PacketResponse
    {
        public RecvCharaUpdateSkillApCostCut()
            : base((ushort)AreaPacketId.recv_chara_update_skill_ap_cost_cut, ServerType.Area)
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
