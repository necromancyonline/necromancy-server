using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class StripCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.arguments.Count == 2)
            {
                string line;
                System.IO.StreamReader fileIn = new System.IO.StreamReader(parameter.arguments[0]);
                System.IO.StreamWriter fileOut = new System.IO.StreamWriter(parameter.arguments[1]);
                while ((line = fileIn.ReadLine()) != null)
                {
                    if (!line.StartsWith(",") && !line.StartsWith("#") && line.Length != 0)
                    {
                        fileOut.WriteLine(line);
                    }

                }

                fileIn.Close();
                fileOut.Close();
                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string key => "strip";

        public override string description =>
            $"Strips Lines From Data. Ex.:{Environment.NewLine}strip \"C:/Games/Wizardry Online/data/settings.csv\" \"C:/Games/Wizardry Online/data/settings_filtered.csv\"";
    }
}
