using System;
using System.Collections.Generic;
using System.Text;
using Necromancy.Cli.Argument;

namespace Necromancy.Cli.Command.Commands
{
    public class ConvertCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.arguments.Count == 3)
            {
                string[] itemInfo = System.IO.File.ReadAllLines(parameter.arguments[0]);
                string[] items = System.IO.File.ReadAllLines(parameter.arguments[1]);
                System.IO.StreamWriter outFile = new System.IO.StreamWriter(parameter.arguments[2]);
                foreach (string infoline in itemInfo)
                {
                    if (infoline.StartsWith("#"))
                        continue;
                    string infoId = infoline.Substring(0, infoline.IndexOf(","));
                    foreach (string itemline in items)
                    {
                        if (itemline.StartsWith("#") || itemline.StartsWith(",") || itemline.StartsWith("N/A"))
                            continue;
                        string itemId = itemline.Substring(0, infoline.IndexOf(","));
                        if (infoId == itemId)
                        {
                            string infoName = infoline.Substring(infoline.IndexOf(",")+1);
                            string [] itemValues = itemline.Split(",", StringSplitOptions.None);
                            itemValues[5] = infoName;
                            List<string> listItems = new List<string>(itemValues);
                            listItems.RemoveAt(4);
                            listItems.RemoveAt(2);
                            string outLine = string.Join(",", listItems.ToArray());
                            outFile.WriteLine(outLine);
                            break;
                        }
                    }
                }
                outFile.Close();
                return CommandResultType.Completed;
            }


            return CommandResultType.Completed;
        }


        public override string key => "convert";
        public override string description => $"Find English iteminfo in Japanese ItemInfo and add to new file substituting name.{Environment.NewLine}";
    }
}
