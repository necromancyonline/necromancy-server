using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvRoguemapEntryReadyNewR : PacketResponse
    {
        public RecvRoguemapEntryReadyNewR()
            : base((ushort)AreaPacketId.recv_roguemap_entry_ready_new_r, ServerType.Area)
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
