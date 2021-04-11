using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_soul_partner_notify_avatar : PacketResponse
    {
        public recv_soul_partner_notify_avatar()
            : base((ushort) AreaPacketId.recv_soul_partner_notify_avatar, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(100001);
            res.WriteByte(1);
            return res;
        }
    }
}
