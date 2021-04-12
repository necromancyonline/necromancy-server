using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvPackageAllDelete : PacketResponse
    {
        public RecvPackageAllDelete()
            : base((ushort)AreaPacketId.recv_package_all_delete_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            int numEntries = 0x64;
            res.WriteInt32(numEntries); // cmp to 0x64 = 100

            for (int i = 0; i < numEntries; i++) res.WriteInt32(0);
            return res;
        }
    }
}
