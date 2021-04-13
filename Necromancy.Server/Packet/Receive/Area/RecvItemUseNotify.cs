using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUseNotify : PacketResponse
    {
        private readonly float _coolTime;
        private readonly long _itemId; //Item instance ID

        public RecvItemUseNotify(long itemId, float coolTime)
            : base((ushort)AreaPacketId.recv_item_use_notify, ServerType.Area)
        {
            _itemId = itemId;
            _coolTime = coolTime;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(_itemId);
            res.WriteFloat(_coolTime);
            return res;
        }
    }
}
