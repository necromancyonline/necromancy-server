using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_notify_sg : PacketResponse
    {
        public recv_soul_partner_notify_sg()
            : base((ushort) AreaPacketId.recv_soul_partner_notify_sg, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt64(0);
            res.WriteInt16(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
