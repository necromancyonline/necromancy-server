using Arrowgene.Buffers;
using Necromancy.Server.Chat;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Receive.Area
{
    public class RecvChatNotifyMessage : PacketResponse
    {
        private readonly ChatResponse _response;

        public RecvChatNotifyMessage(ChatResponse response)
            : base((ushort)AreaPacketId.recv_chat_notify_message, ServerType.Area)
        {
            _response = response;
        }

        protected override IBuffer ToBuffer()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32((int)_response.messageType);
            res.WriteUInt32(_response.characterInstanceId);
            res.WriteFixedString(_response.soulName, 49);
            res.WriteFixedString(_response.characterName, 37);
            res.WriteInt32(1); //new
            res.WriteInt32(2); //new
            res.WriteInt32(3); //new
            res.WriteFixedString(_response.message, 769);
            return res;
        }
    }
}
