using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat
{
    public abstract class ChatHandler : IChatHandler
    {
        public abstract void Handle(NecClient client, ChatMessage message, ChatResponse response,
            List<ChatResponse> responses);
    }
}
