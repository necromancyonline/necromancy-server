using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_shop_notify_remove_item : PacketResponse
    {
        public recv_shop_notify_remove_item()
            : base((ushort) AreaPacketId.recv_shop_notify_remove_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            return res;
        }
    }
}
