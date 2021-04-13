using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvShopNotifySellSurrogateFee : PacketResponse
    {
        public RecvShopNotifySellSurrogateFee()
            : base((ushort)AreaPacketId.recv_shop_notify_sell_surrogate_fee, ServerType.Area)
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
