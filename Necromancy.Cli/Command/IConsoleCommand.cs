using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command
{
    public interface IConsoleCommand
    {
        CommandResultType Handle(ConsoleParameter parameter);
        void Shutdown();
        string key { get; }
        string description { get; }
    }
}
