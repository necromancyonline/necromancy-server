using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvUnionRenameOpen : PacketResponse
    {
        public RecvUnionRenameOpen()
            : base((ushort)AreaPacketId.recv_union_rename_open, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); //Length 0x31
            return res;
        }
    }
}
