using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguemapSetMapFragmentChipiconR : PacketResponse
    {
        public RecvRoguemapSetMapFragmentChipiconR()
            : base((ushort) AreaPacketId.recv_roguemap_set_map_fragment_chipicon_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            return res;
        }
    }
}
