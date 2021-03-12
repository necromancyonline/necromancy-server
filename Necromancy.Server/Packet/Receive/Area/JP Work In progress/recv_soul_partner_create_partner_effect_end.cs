using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_create_partner_effect_end : PacketResponse
    {
        public recv_soul_partner_create_partner_effect_end()
            : base((ushort) AreaPacketId.recv_soul_partner_create_partner_effect_end, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            //No structure

            return res;
        }
    }
}
