using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvBaseLogin : PacketResponse
    {
        public RecvBaseLogin()
            : base((ushort) MsgPacketId.recv_base_login_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            for (int i = 0; i < 8; i++)
            {
                res.WriteByte(0);
                res.WriteFixedString("", 0x31);
                res.WriteByte(0);
                res.WriteByte(0); //bool
            }
            res.WriteInt32(0);
            res.WriteByte(0); //bool
            res.WriteByte(0);
            return res;
        }
    }
}
