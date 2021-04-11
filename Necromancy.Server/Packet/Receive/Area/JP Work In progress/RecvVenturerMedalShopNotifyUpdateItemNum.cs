using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_venturer_medal_shop_notify_update_item_num : PacketResponse
    {
        public recv_venturer_medal_shop_notify_update_item_num()
            : base((ushort) AreaPacketId.recv_venturer_medal_shop_notify_update_item_num, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
