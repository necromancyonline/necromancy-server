using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class Recv0Xc991 : PacketResponse
    {
        public Recv0Xc991()
            : base((ushort)MsgPacketId.recv_0xC991, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(0x3E8);
            for (int i = 0; i < 0x3E8; i++) //(int32 above)
            {
                res.WriteInt32(0);
                res.WriteFixedString("", 0x31); //size is 0x31
                res.WriteFixedString("", 0x5B); //size is 0x5B
                res.WriteInt32(0);
                res.WriteFixedString("", 0x49); //size is 0x49
                res.WriteFixedString("", 0x49); //size is 0x49
                res.WriteInt32(0);
            }

            return res;
        }
    }
}
