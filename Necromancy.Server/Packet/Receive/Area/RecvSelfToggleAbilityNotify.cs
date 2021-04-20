using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSelfToggleAbilityNotify : PacketResponse
    {
        public RecvSelfToggleAbilityNotify()
            : base((ushort)AreaPacketId.recv_self_toggle_ability_notify, ServerType.Area)
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
