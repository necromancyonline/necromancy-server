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
            Dictionary<string, ChatCommand> commands = server.chat.commandHandler.GetCommands();
            foreach (string key in commands.Keys)
            {
                ChatCommand chatCommand = commands[key];
                if (chatCommand.helpText == null)
                {
                    continue;
                }

                responses.Add(ChatResponse.CommandError(client, "----------"));
                responses.Add(ChatResponse.CommandError(client, $"{key}"));
                responses.Add(ChatResponse.CommandError(client, chatCommand.helpText));
            }
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "h";
    }
}
