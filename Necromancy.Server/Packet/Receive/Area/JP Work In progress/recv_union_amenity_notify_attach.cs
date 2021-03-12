using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_union_amenity_notify_attach : PacketResponse
    {
        public recv_union_amenity_notify_attach()
            : base((ushort) AreaPacketId.recv_union_amenity_notify_attach, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
