using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvSoulSetPasswd : PacketResponse
    {
        public RecvSoulSetPasswd()
            : base((ushort) MsgPacketId.recv_soul_set_passwd_r, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(0); //Bool
            res.WriteCString("");
            return res;
        }
    }
}
