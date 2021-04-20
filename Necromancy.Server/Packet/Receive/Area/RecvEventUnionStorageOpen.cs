using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventUnionStorageOpen : PacketResponse
    {
        public RecvEventUnionStorageOpen()
            : base((ushort)AreaPacketId.recv_event_union_storage_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt64(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
