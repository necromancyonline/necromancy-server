using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPartyNotifyRemoveDrawItem : PacketResponse
    {
        public RecvPartyNotifyRemoveDrawItem()
            : base((ushort)AreaPacketId.recv_party_notify_remove_draw_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            return res;
        }
    }
}
