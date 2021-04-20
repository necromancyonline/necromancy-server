using System;
using System.Collections.Generic;
using System.Text;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class HelpCommand : ConsoleCommand
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(HelpCommand));

        private readonly Dictionary<string, IConsoleCommand> _commands;

        public HelpCommand(Dictionary<string, IConsoleCommand> commands)
        {
            _commands = commands;
        }

        public override string key => "help";
        public override string description => "Displays this text";

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.arguments.Count >= 1)
            {
                string subKey = parameter.arguments[0];
                if (!_commands.ContainsKey(subKey))
                {
                    _Logger.Error(
                        $"Command: 'help {subKey}' not available. Type 'help' for a list of available commands.");
                    return CommandResultType.Continue;
                }

                IConsoleCommand consoleCommandHelp = _commands[subKey];
                _Logger.Info(ShowHelp(consoleCommandHelp));
                return CommandResultType.Continue;
            }

            ShowHelp();
            return CommandResultType.Continue;
        }

        private void ShowHelp()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append("Available Commands:");
            foreach (string key in _commands.Keys)
            {
                sb.Append(Environment.NewLine);
                sb.Append("----------");
                IConsoleCommand command = _commands[key];
                sb.Append(ShowHelp(command));
            }

            _Logger.Info(sb.ToString());
        }

        private string ShowHelp(IConsoleCommand command)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Environment.NewLine);
            sb.Append(command.key);
            sb.Append(Environment.NewLine);
            sb.Append($"- {command.description}");
            return sb.ToString();
        }
    }
}
