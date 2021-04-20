using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvEventUnionStorageUpdateMoney : PacketResponse
    {
        public RecvEventUnionStorageUpdateMoney()
            : base((ushort)AreaPacketId.recv_event_union_storage_update_money, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0);
            res.WriteInt64(0);
            return res;
        }
    }
}
