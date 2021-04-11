using Necromancy.Server.Chat;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area.SendChatPostMessage
{
    public class SendChatPostMessageHandler : ClientHandlerDeserializer<ChatMessage>
    {
        public SendChatPostMessageHandler(NecServer server) : base(server, new SendChatPostMessageDeserializer())
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_chat_post_message;

        public override void HandleRequest(NecClient client, ChatMessage request)
        {
            server.chat.Handle(client, request);
        }
    }
}
