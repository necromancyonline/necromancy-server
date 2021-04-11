using System;
using System.IO;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class UnpackCommand : ConsoleCommand
    {
        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.arguments.Count == 2)
            {
                FpmfArchiveIo archiveIo = new FpmfArchiveIo();
                FileAttributes attr = File.GetAttributes(parameter.arguments[0]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string[] hedFiles =
                        Directory.GetFiles(parameter.arguments[0], "*.hed", SearchOption.AllDirectories);
                    for (int i = 0; i < hedFiles.Length; i++)
                    {
                        FpmfArchive archive = archiveIo.Open(hedFiles[i]);
                        archiveIo.Save(archive, parameter.arguments[1]);
                    }
                }
                else
                {
                    FpmfArchive archive = archiveIo.Open(parameter.arguments[0]);
                    archiveIo.Save(archive, parameter.arguments[1]);
                }

                return CommandResultType.Completed;
            }

            if (parameter.arguments.Count == 1)
            {
                FpmfArchiveIo archiveIo = new FpmfArchiveIo();
                archiveIo.OpenWoItm(parameter.arguments[0]);
                return CommandResultType.Completed;
            }

            if (parameter.arguments.Count == 3)
            {
                FpmfArchiveIo archiveIo = new FpmfArchiveIo();
                string[] hedFiles;
                FileAttributes attr = File.GetAttributes(parameter.arguments[1]);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    hedFiles = Directory.GetFiles(parameter.arguments[1], "*.hed", SearchOption.AllDirectories);
                }
                else
                {
                    hedFiles = new string[1];
                    hedFiles[0] = parameter.arguments[1];
                }

                for (int i = 0; i < hedFiles.Length; i++)
                {
                    if (parameter.arguments[0] == "header")
                    {
                        archiveIo.Header(hedFiles[i], parameter.arguments[2]);
                    }
                }

                return CommandResultType.Completed;
            }

            return CommandResultType.Continue;
        }

        public override string key => "unpack";

        public override string description =>
            $"Unpacks Data. Ex.:{Environment.NewLine}unpack \"C:/Games/Wizardry Online/data/settings.hed\" \"C:/output\"";
    }
}
