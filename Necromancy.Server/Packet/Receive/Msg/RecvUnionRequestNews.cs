using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvUnionRequestNews : PacketResponse
    {
        public RecvUnionRequestNews()
            : base((ushort) MsgPacketId.recv_union_request_news_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            int numEntries = 0x3E8;
            res.WriteInt32(numEntries);//Less than or equal to 0x3E8
            for (int i = 0; i < numEntries; i++)
            {
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31); //Fixed string of size 0x31
                res.WriteFixedString("", 0x5B); //Fixed string of size 0x5B
                res.WriteInt32(0);
                res.WriteFixedString("", 0x49); //Fixed string of size 0x49
                res.WriteFixedString("", 0x49); //Fixed string of size 0x49
                res.WriteInt32(0);
            }
            return res;
        }
    }
}
