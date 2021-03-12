using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionThreadGet : PacketResponse
    {
        public RecvUnionThreadGet()
            : base((ushort) MsgPacketId.recv_union_thread_get_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x64;
            res.WriteInt32(numEntries);//Less than or equal to 0x64
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x25); //Fixed string of size 0x25
                res.WriteFixedString("", 0xC1); //Fixed string of size 0xC1
            }
            return res;
        }
    }
}
