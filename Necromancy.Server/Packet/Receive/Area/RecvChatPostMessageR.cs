using Arrowgene.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChatPostMessageR : PacketResponse
    {
        private readonly ChatErrorType _chatErrorType;

        public RecvChatPostMessageR(ChatErrorType chatErrorType)
            : base((ushort)AreaPacketId.recv_chat_post_message_r, ServerType.Area)
        {
            _chatErrorType = chatErrorType;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32((int)_chatErrorType);
            return res;
        }
    }
}
