using System;
using System.IO;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class StripCommand : ConsoleCommand
    {
        public override string key => "strip";

        public override string description =>
            $"Strips Lines From Data. Ex.:{Environment.NewLine}strip \"C:/Games/Wizardry Online/data/settings.csv\" \"C:/Games/Wizardry Online/data/settings_filtered.csv\"";

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.arguments.Count == 2)
            {
                string line;
                StreamReader fileIn = new StreamReader(parameter.arguments[0]);
                StreamWriter fileOut = new StreamWriter(parameter.arguments[1]);
                while ((line = fileIn.ReadLine()) != null)
                    if (!line.StartsWith(",") && !line.StartsWith("#") && line.Length != 0)
                        fileOut.WriteLine(line);

                fileIn.Close();
                fileOut.Close();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }
    }
}
