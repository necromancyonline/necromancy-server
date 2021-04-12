using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGemRebuildInfo : PacketResponse
    {
        public RecvGemRebuildInfo()
            : base((ushort)AreaPacketId.recv_gem_rebuild_info, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x10;
            res.WriteInt32(numEntries); //less than or equal to 0x1E
            for (int i = 0; i < numEntries; i++) res.WriteByte(1);

            res.WriteInt64(1);
            return res;
        }
    }
}
