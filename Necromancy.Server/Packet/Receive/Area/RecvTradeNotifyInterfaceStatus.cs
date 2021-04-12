using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvTradeNotifyInterfaceStatus : PacketResponse
    {
        private int _status;
        public RecvTradeNotifyInterfaceStatus(int status)
            : base((ushort) AreaPacketId.recv_trade_notify_interface_status, ServerType.Area)
        {
            _status = status;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_status);
            return res;
        }
    }
}
