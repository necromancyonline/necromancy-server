using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvSoulAuthenticatePasswd : PacketResponse
    {
        public RecvSoulAuthenticatePasswd()
            : base((ushort)MsgPacketId.recv_soul_authenticate_passwd_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); //bool
            res.WriteCString("");
            return res;
        }
    }
}
