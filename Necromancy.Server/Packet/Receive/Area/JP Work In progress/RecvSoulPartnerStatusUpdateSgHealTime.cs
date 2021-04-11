using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_status_update_sg_heal_time : PacketResponse
    {
        public recv_soul_partner_status_update_sg_heal_time()
            : base((ushort) AreaPacketId.recv_soul_partner_status_update_sg_heal_time, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteCString("What");
            res.WriteInt32(0);
            return res;
        }
    }
}
