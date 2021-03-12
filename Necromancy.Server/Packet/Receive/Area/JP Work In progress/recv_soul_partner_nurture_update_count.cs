using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_nurture_update_count : PacketResponse
    {
        public recv_soul_partner_nurture_update_count()
            : base((ushort) AreaPacketId.recv_soul_partner_nurture_update_count, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteByte(0); //0x3 loop broken up
            res.WriteByte(0);
            res.WriteByte(0);
            return res;
        }
    }
}
