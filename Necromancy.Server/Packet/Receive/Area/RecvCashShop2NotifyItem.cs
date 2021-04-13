using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShop2NotifyItem : PacketResponse
    {
        public RecvCashShop2NotifyItem()
            : base((ushort)AreaPacketId.recv_cash_shop2_notify_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt32(0x10);
            for (int i = 0; i < 0x10; i++) res.WriteByte(0);
            res.WriteCString(""); //find max size
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteCString(""); //find max size
            return res;
        }
    }
}
