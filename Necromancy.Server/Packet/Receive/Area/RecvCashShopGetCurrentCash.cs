using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCashShopGetCurrentCash : PacketResponse
    {
        public RecvCashShopGetCurrentCash()
            : base((ushort)AreaPacketId.recv_cash_shop_get_current_cash_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(29);

            res.WriteInt32(28);
            return res;
        }
    }
}
