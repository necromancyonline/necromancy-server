using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvSoulPartnerStatusIntimacyPoint : PacketResponse
    {
        public RecvSoulPartnerStatusIntimacyPoint()
            : base((ushort)AreaPacketId.recv_soul_partner_status_intimacy_point, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteCString("What");
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            return res;
        }
    }
}
