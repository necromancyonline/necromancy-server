using Arrowgene.Logging;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class ExitCommand : ConsoleCommand
    {
        private static readonly ILogger _Logger = LogProvider.Logger(typeof(ExitCommand));

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            _Logger.Info("Exiting...");
            return CommandResultType.Exit;
        }

        public override string key => "exit";
        public override string description => "Closes the program";
    }
}
