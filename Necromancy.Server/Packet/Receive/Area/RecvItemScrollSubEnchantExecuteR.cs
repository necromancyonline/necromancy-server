using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_item_scroll_sub_enchant_execute_r : PacketResponse
    {
        public recv_item_scroll_sub_enchant_execute_r()
            : base((ushort) AreaPacketId.recv_item_scroll_sub_enchant_execute_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            int numEntries = 0x2;
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteByte(0);
            res.WriteInt32(numEntries); //less than 5
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteInt16(0);
            }
            res.WriteInt32(numEntries); //less than 5
            for (int k = 0; k < numEntries; k++)
            {
                res.WriteByte(0);
            }
            res.WriteInt32(0);
            return res;
        }
    }
}
