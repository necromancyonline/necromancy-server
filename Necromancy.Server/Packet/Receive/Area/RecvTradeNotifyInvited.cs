using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyInvited : PacketResponse
    {
        private readonly uint _instanceId;

        public RecvTradeNotifyInvited(uint instanceId)
            : base((ushort)AreaPacketId.recv_trade_notify_invited, ServerType.Area)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            return res;
        }
    }
}
