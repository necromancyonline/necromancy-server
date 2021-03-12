using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionThreadAlwaysGet : PacketResponse
    {
        public RecvUnionThreadAlwaysGet()
            : base((ushort) MsgPacketId.recv_union_thread_always_get_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0xA;
            res.WriteInt32(numEntries); //less than or equal to 0xA
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteInt32(0);
                res.WriteFixedString("", 0x25);
                res.WriteFixedString("", 0xC1);
            }
            return res;
        }
    }
}
