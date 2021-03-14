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
            Server.Database.UpdateCharacter(client.Character);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "charsave";
    }
}
