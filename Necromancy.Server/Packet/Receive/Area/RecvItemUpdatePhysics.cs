using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdatePhysics : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly short _level;

        public RecvItemUpdatePhysics(ulong instanceId, short level)
            : base((ushort)AreaPacketId.recv_item_update_physics, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId); //item instance id
            res.WriteInt16(_level); //item's attack stat
            return res;
        }
    }
}
