using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvPartyNotifyGetMoney : PacketResponse
    {
        private uint _instanceId; //???
        public RecvPartyNotifyGetMoney(uint instanceId)
            : base((ushort) MsgPacketId.recv_party_notify_get_money, ServerType.Msg)
        {
            _instanceId = instanceId;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_instanceId);
            res.WriteInt64(100);
            return res;
        }
    }
}
