using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Msg
{
    public class RecvChatNotifyMessage : PacketResponse
    {
        public RecvChatNotifyMessage()
            : base((ushort) MsgPacketId.recv_chat_notify_message, ServerType.Msg)
        {
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
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
