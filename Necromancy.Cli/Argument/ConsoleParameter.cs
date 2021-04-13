using System.Collections.Generic;

namespace Necromancy.Cli.Argument
{
    public class ConsoleParameter
    {
        public ConsoleParameter(string key)
        {
            this.key = key;
            arguments = new List<string>();
            switches = new List<string>();
            argumentMap = new Dictionary<string, string>();
            switchMap = new Dictionary<string, string>();
        }

        public string key { get; }
        public List<string> arguments { get; }
        public List<string> switches { get; }
        public Dictionary<string, string> switchMap { get; }
        public Dictionary<string, string> argumentMap { get; }

        public void Clear()
        {
            arguments.Clear();
            switches.Clear();
            argumentMap.Clear();
            switchMap.Clear();
        }
    }
}
