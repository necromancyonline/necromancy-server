using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerExpeditionResultGetAllR : PacketResponse
    {
        public RecvSoulPartnerExpeditionResultGetAllR()
            : base((ushort) AreaPacketId.recv_soul_partner_expedition_result_get_all_r, ServerType.Area)
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
