using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public abstract class ChatCommand
    {
        public abstract AccountStateType accountState { get; }
        public abstract string key { get; }
        public string keyToLowerInvariant => key.ToLowerInvariant();

        public virtual string helpText => null;

        public abstract void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses);
    }
}
