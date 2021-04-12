using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChallengemapPointShopNotifyItem : PacketResponse
    {
        public RecvChallengemapPointShopNotifyItem()
            : base((ushort)AreaPacketId.recv_challengemap_point_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt16(0);
            res.WriteInt32(0);
            res.WriteFixedString("Xeno", 0x10);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt16(0);
            return res;
        }
    }
}
