using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command
{
    public interface IConsoleCommand
    {
        string key { get; }
        string description { get; }
        CommandResultType Handle(ConsoleParameter parameter);
        void Shutdown();
    }
}
