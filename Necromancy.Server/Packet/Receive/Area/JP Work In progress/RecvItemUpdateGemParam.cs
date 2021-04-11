using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class recv_item_update_gem_param : PacketResponse
    {
        public recv_item_update_gem_param()
            : base((ushort) AreaPacketId.recv_item_update_gem_param, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            int numEntries = 0x2;
            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(0);
            res.WriteInt32(numEntries); //less than 0x3
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteByte(0);
                res.WriteInt32(0);
                int numEntries5 = 0x2; //must be 0x2
                for (int k = 0; k < numEntries5; k++)
                {
                    res.WriteInt32(0);
                }
            }
            return res;
        }
    }
}
