using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvItemScrollEnchantCheckR : PacketResponse
    {
        public RecvItemScrollEnchantCheckR()
            : base((ushort) AreaPacketId.recv_item_scroll_enchant_check_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            //4964E0
            res.WriteInt64(0);
            int numEntries3 = 0x3; //must be 0x3
            for (int i = 0; i < numEntries3; i++)
            {
                //4929F0
                res.WriteInt64(0);
                //4929a0
                res.WriteInt16(0);

                int numEntries4 = 0x2; //must be 0x2
                for (int j = 0; j < numEntries4; j++)
                {
                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    int numEntries5 = 0x2; //must be 0x2
                    for (int k = 0; k < numEntries5; k++)
                    {
                        res.WriteInt16(0);
                    }
                }
                int numEntries2 = 0x5; //must be 0x5
                for (int j = 0; j < numEntries2; j++)
                {
                    res.WriteInt16(0);

                    res.WriteInt32(0);
                    res.WriteInt32(0);
                    int numEntries5 = 0x2; //must be 0x2
                    for (int k = 0; k < numEntries5; k++)
                    {
                        res.WriteInt16(0);
                    }
                }
            }
            return res;
        }
    }
}
