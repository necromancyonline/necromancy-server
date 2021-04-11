using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerUpdateCost : PacketResponse
    {
        public RecvSoulPartnerUpdateCost()
            : base((ushort) AreaPacketId.recv_soul_partner_update_cost, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
