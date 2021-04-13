using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class Recv0X9A9B : PacketResponse
    {
        public Recv0X9A9B()
            : base((ushort)MsgPacketId.recv_0x9A9B, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString("");
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x31);
            res.WriteFixedString("", 0x25);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteFixedString("", 0x301);
            return res;
        }
    }
}
