using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a npc
    /// </summary>
    public class HelpCommand : ServerChatCommand
    {
        public HelpCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            responses.Add(ChatResponse.CommandError(client, "Available Commands:"));
            Dictionary<string, ChatCommand> commands = Server.Chat.CommandHandler.GetCommands();
            foreach (string key in commands.Keys)
            {
                ChatCommand chatCommand = commands[key];
                if (chatCommand.HelpText == null)
                {
                    continue;
                }

                responses.Add(ChatResponse.CommandError(client, "----------"));
                responses.Add(ChatResponse.CommandError(client, $"{key}"));
                responses.Add(ChatResponse.CommandError(client, chatCommand.HelpText));
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "h";
    }
}
