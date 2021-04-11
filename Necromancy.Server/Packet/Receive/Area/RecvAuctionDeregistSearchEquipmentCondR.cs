using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvAuctionDeregistSearchEquipmentCondR : PacketResponse
    {
        public RecvAuctionDeregistSearchEquipmentCondR()
            : base((ushort) AreaPacketId.recv_auction_deregist_search_equipment_cond_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error message
            return res;
        }
    }
}
