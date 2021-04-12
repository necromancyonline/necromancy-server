using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateActionProhibitCamp : PacketResponse
    {
        public RecvCharaUpdateActionProhibitCamp()
            : base((ushort)AreaPacketId.recv_chara_update_action_prohibit_camp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0); //bool
            return res;
        }
    }
}
