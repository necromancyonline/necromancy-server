using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendCharacterSave : ServerChatCommand
    {
        public SendCharacterSave(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            server.database.UpdateCharacter(client.character);
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "charsave";
    }
}
