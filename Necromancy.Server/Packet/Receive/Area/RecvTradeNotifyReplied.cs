using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyReplied : PacketResponse
    {
        private readonly int _error;

        public RecvTradeNotifyReplied(int error)
            : base((ushort)AreaPacketId.recv_trade_notify_replied, ServerType.Area)
        {
            _error = error;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_error);
            return res;
        }
    }
}
