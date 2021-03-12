using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_status_update_bonus_ability_r : PacketResponse
    {
        public recv_soul_partner_status_update_bonus_ability_r()
            : base((ushort) AreaPacketId.recv_soul_partner_status_update_bonus_ability_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            return res;
        }
    }
}
