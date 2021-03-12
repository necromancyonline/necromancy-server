using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_auction_regist_search_equipment_cond_r : PacketResponse
    {
        public recv_auction_regist_search_equipment_cond_r()
            : base((ushort) AreaPacketId.recv_auction_regist_search_equipment_cond_r, ServerType.Area)
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
