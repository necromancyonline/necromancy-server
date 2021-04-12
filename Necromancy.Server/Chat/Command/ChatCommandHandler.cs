using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command
{
    public class ChatCommandHandler : ChatHandler
    {
        public const char ChatCommandStart = '/';
        public const char ChatCommandSeparator = ' ';

        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ChatCommandHandler));

        private readonly Dictionary<string, ChatCommand> _commands;
        private readonly NecServer _server;

        public ChatCommandHandler(NecServer server)
        {
            _server = server;
            _commands = new Dictionary<string, ChatCommand>();
        }

        public void AddCommand(ChatCommand command)
        {
            _commands.Add(command.keyToLowerInvariant, command);
        }

        public Dictionary<string, ChatCommand> GetCommands()
        {
            return new Dictionary<string, ChatCommand>(_commands);
        }

        public void HandleCommand(NecClient client, string command)
        {
            if (client == null) return;

            ChatMessage message = new ChatMessage(ChatMessageType.ChatCommand, client.character.name, command);
            List<ChatResponse> responses = new List<ChatResponse>();
            Handle(client, message, new ChatResponse(), responses);
            foreach (ChatResponse response in responses) _server.router.Send(response);
        }

        public override void Handle(NecClient client, ChatMessage message, ChatResponse response,
            List<ChatResponse> responses)
        {
            if (client == null) return;

            if (message.message == null || message.message.Length <= 1) return;

            if (!message.message.StartsWith(ChatCommandStart)) return;

            string commandMessage = message.message.Substring(1);
            string[] command = commandMessage.Split(ChatCommandSeparator);
            if (command.Length <= 0)
            {
                _Logger.Error(client, $"Command '{message.message}' is invalid");
                return;
            }

            string commandKey = command[0].ToLowerInvariant();
            if (!_commands.ContainsKey(commandKey))
            {
                _Logger.Error(client, $"Command '{commandKey}' does not exist");
                responses.Add(ChatResponse.CommandError(client, $"Command does not exist: {commandKey}"));
                return;
            }

            ChatCommand chatCommand = _commands[commandKey];
            if (client.account.state < chatCommand.accountState)
            {
                _Logger.Error(client,
                    $"Not entitled to execute command '{chatCommand.key}' (State < Required: {client.account.state} < {chatCommand.accountState})");
                return;
            }

            int cmdLength = command.Length - 1;
            string[] subCommand;
            if (cmdLength > 0)
            {
                subCommand = new string[cmdLength];
                Array.Copy(command, 1, subCommand, 0, cmdLength);
            }
            else
            {
                subCommand = new string[0];
            }

            chatCommand.Execute(subCommand, client, message, responses);
        }
    }
}
