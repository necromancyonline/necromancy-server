using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvGetRefusallList : PacketResponse
    {
        public RecvGetRefusallList()
            : base((ushort)AreaPacketId.recv_get_refusallist_r, ServerType.Area)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);

            res.WriteInt32(0x32); //cmp to 0x32

            int numEntries = 0x32;
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("ToBeFound", 0x31);
            }

            res.WriteInt32(0x32); //cmp to 0x32

            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("ToBeFound", 0x31);
            }

            return res;
        }
    }
}
