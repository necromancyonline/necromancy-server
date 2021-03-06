using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerSummonCastStartR : PacketResponse
    {
        public RecvSoulPartnerSummonCastStartR()
            : base((ushort)AreaPacketId.recv_soul_partner_summon_cast_start_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteFloat(0);
            return res;
        }
    }
}
