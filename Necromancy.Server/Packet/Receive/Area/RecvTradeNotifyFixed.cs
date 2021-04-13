using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyFixed : PacketResponse
    {
        private readonly int _result;

        public RecvTradeNotifyFixed(int result)
            : base((ushort)AreaPacketId.recv_trade_notify_fixed, ServerType.Area)
        {
            _result = result;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_result);
            return res;
        }
    }
}
