using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopNotifyItem : PacketResponse
    {
        public RecvShopNotifyItem()
            : base((ushort)AreaPacketId.recv_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0); //idx
            res.WriteInt32(0); // item Serial id
            res.WriteInt64(0); // item price
            res.WriteInt64(69); // new
            res.WriteInt64(692); // new
            res.WriteByte(1); //Bool new
            res.WriteFixedString("Item Name", 0x10); // ?
            res.WriteInt32(6969); //new
            res.WriteInt16(15); //new
            return res;
        }
    }
}
