using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateWeight : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly int _level;

        public RecvItemUpdateWeight(ulong instanceId, int level)
            : base((ushort)AreaPacketId.recv_item_update_weight, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId); //item instance id
            res.WriteInt32(_level); //item's Weight stat
            return res;
        }
    }
}
