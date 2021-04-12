using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvStallSellItem : PacketResponse
    {
        public RecvStallSellItem()
            : base((ushort)AreaPacketId.recv_stall_sell_item, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("ToBeFound"); // find max size 
            res.WriteCString("ToBeFound"); // find max size 
            res.WriteInt64(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt16(0);
            res.WriteInt32(0);
            return res;
        }
    }
}
