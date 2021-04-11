using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_status_update_exp : PacketResponse
    {
        public recv_soul_partner_status_update_exp()
            : base((ushort) AreaPacketId.recv_soul_partner_status_update_exp, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            return res;
        }
    }
}
