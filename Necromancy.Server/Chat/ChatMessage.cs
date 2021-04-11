namespace Necromancy.Server.Chat
{
    public class ChatMessage
    {
        public ChatMessage(ChatMessageType messageType, string recipientSoulName, string message)
        {
            this.messageType = messageType;
            this.recipientSoulName = recipientSoulName;
            this.message = message;
        }

        public ChatMessageType messageType { get; }
        public string recipientSoulName { get; }
        public string message { get; }
    }
}
