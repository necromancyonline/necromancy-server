using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class Recv0X8D74 : PacketResponse
    {
        public Recv0X8D74()
            : base((ushort)MsgPacketId.recv_0x8D74, ServerType.Msg)
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
