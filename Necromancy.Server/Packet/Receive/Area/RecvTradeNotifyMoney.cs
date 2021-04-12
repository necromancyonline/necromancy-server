using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyMoney : PacketResponse
    {
        private readonly ulong _gold;
        public RecvTradeNotifyMoney(ulong gold)
            : base((ushort) AreaPacketId.recv_trade_notify_money, ServerType.Area)
        {
            _gold = gold;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_gold);
            return res;
        }
    }
}
