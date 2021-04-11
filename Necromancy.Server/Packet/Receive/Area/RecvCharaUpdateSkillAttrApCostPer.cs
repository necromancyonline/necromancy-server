using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_chara_update_skill_attr_ap_cost_per : PacketResponse
    {
        public recv_chara_update_skill_attr_ap_cost_per()
            : base((ushort) AreaPacketId.recv_chara_update_skill_attr_ap_cost_per, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
