using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvStorageUpdateViewPageR : PacketResponse
    {
        public RecvStorageUpdateViewPageR()
            : base((ushort) AreaPacketId.recv_storage_update_view_page_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);//throws last chance if 0.

            return res;
        }
    }
}
