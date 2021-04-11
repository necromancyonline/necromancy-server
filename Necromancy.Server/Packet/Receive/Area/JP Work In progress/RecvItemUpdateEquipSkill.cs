using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_item_update_equip_skill : PacketResponse
    {
        public recv_item_update_equip_skill()
            : base((ushort) AreaPacketId.recv_item_update_equip_skill, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(numEntries); //less than 0x5
            for (int j = 0; j < numEntries; j++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("Ok", 0x2);
                res.WriteInt16(0);
                res.WriteInt16(0);
            }
            return res;
        }
    }
}
