using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfSoulToggleAbilityNotify : PacketResponse
    {
        public RecvSelfSoulToggleAbilityNotify()
            : base((ushort)AreaPacketId.recv_self_soul_toggle_ability_notify, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            return res;
        }
    }
}
