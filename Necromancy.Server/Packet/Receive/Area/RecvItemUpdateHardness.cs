using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemUpdateHardness : PacketResponse
    {
        private readonly ulong _instanceId;
        private readonly byte _level;

        public RecvItemUpdateHardness(ulong instanceId, byte level)
            : base((ushort)AreaPacketId.recv_item_update_hardness, ServerType.Area)
        {
            _instanceId = instanceId;
            _level = level;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt64(_instanceId);
            res.WriteByte(_level);
            return res;
        }
    }
}
