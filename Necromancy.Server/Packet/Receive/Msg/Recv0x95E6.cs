using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class Recv0X95E6 : PacketResponse
    {
        public Recv0X95E6()
            : base((ushort)MsgPacketId.recv_0x95E6, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteCString(""); //max size 0x5B
            res.WriteCString(""); //max size 0x3D
            res.WriteInt16(0);
            return res;
        }
    }
}
