using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateMaxDur : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly int _level;
        public RecvItemUpdateMaxDur(ulong instanceId, int level)
            : base((ushort) AreaPacketId.recv_item_update_maxdur, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId); //item instance id
            res.WriteInt32(_level); //item's MaxDur stat
            return res;
        }
    }
}
