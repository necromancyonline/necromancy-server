using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive
{
    public class RecvPartyNotifyGetItem : PacketResponse
    {
        private readonly byte _count;
        private readonly int _itemId;
        private readonly string _message;

        public RecvPartyNotifyGetItem(int itemId, string message, byte count)
            : base((ushort)MsgPacketId.recv_party_notify_get_item, ServerType.Msg)
        {
            _itemId = itemId;
            _message = message;
            _count = count;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(_itemId);
            res.WriteCString(_message);
            res.WriteByte(_count);

            return res;
        }
    }
}
