using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command
{
    public abstract class ConsoleCommand : IConsoleCommand
    {
        public abstract string key { get; }
        public abstract string description { get; }
        public abstract CommandResultType Handle(ConsoleParameter parameter);

        public virtual void Shutdown()
        {
        }
    }
}
