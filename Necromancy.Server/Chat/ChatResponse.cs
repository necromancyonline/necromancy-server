using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public class ChatResponse
    {
        public ChatResponse()
        {
            recipients = new List<NecClient>();
            deliver = true;
            errorType = ChatErrorType.Success;
            messageType = ChatMessageType.Area;
        }

        public ChatResponse(NecClient sender, string message, ChatMessageType messageType,
            string recipientSoulName = null) : this()
        {
            this.message = message;
            characterId = sender.character.id;
            characterInstanceId = sender.character.instanceId;
            characterName = sender.character.name;
            soulName = sender.soul.name;
            deliver = true;
            errorType = ChatErrorType.Success;
            this.messageType = messageType;
            this.recipientSoulName = recipientSoulName;
        }

        public List<NecClient> recipients { get; }
        public bool deliver { get; set; }
        public ChatErrorType errorType { get; set; }
        public ChatMessageType messageType { get; set; }
        public string soulName { get; set; }
        public string characterName { get; set; }
        public string message { get; set; }
        public int characterId { get; set; }
        public uint characterInstanceId { get; set; }
        public string recipientSoulName { get; set; }

        public static ChatResponse CommandError(NecClient client, string message)
        {
            return new ChatResponse
            {
                deliver = true,
                soulName = "System",
                characterName = "",
                errorType = ChatErrorType.GenericUnknownStatement,
                message = message,
                messageType = ChatMessageType.TextCommandLog,
                recipients = { client }
            };
        }

        public static ChatResponse CommandInfo(NecClient client, string message)
        {
            return new ChatResponse
            {
                deliver = true,
                soulName = "System",
                characterName = "",
                errorType = ChatErrorType.Success,
                message = message,
                messageType = ChatMessageType.TextCommandLog,
                recipients = { client }
            };
        }
    }
}
