using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateAlignment : PacketResponse
    {
        private readonly uint _alignment;

        public RecvCharaUpdateAlignment(uint alignment)
            : base((ushort)AreaPacketId.recv_chara_update_alignment, ServerType.Area)
        {
            _alignment = alignment;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(_alignment);
            return res;
        }
    }
}
