using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_challengemap_point_shop_notify_open : PacketResponse
    {
        public recv_challengemap_point_shop_notify_open()
            : base((ushort) AreaPacketId.recv_challengemap_point_shop_notify_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
