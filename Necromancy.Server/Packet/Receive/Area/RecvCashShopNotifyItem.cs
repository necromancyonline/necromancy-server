using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopNotifyItem : PacketResponse
    {
        public RecvCashShopNotifyItem()
            : base((ushort)AreaPacketId.recv_cash_shop_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);

            res.WriteInt32(0); // exp next

            res.WriteInt32(0); // exp next

            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteByte(0);


            res.WriteCString("ToBeFound");

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteByte(0);

            res.WriteInt32(0);

            res.WriteInt32(0);

            res.WriteByte(0);

            res.WriteCString("ToBeFound2");

            res.WriteByte(0);

            res.WriteByte(0);
            return res;
        }
    }
}
