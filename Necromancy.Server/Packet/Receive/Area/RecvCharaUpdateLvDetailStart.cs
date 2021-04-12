using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvCharaUpdateLvDetailStart : PacketResponse
    {
        public RecvCharaUpdateLvDetailStart()
            : base((ushort)AreaPacketId.recv_chara_update_lv_detail_start, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();

            return res;
        }
    }
}
