using System.IO;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class HedCommand : ConsoleCommand
    {
        public static readonly ILogger Logger = LogProvider.Logger(typeof(HedCommand));

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.Arguments.Count < 1)
            {
                Logger.Error("to few args");
                return CommandResultType.Error;
            }

            string command = parameter.Arguments[0].ToLower();

            if (command == "unpack")
            {
                if (parameter.Arguments.Count < 3)
                {
                    Logger.Error("to few args");
                    return CommandResultType.Error;
                }

                byte[] hedKey = ParseKey(parameter);
                string source = parameter.Arguments[1];
                string destination = parameter.Arguments[2];

                FileAttributes attr = File.GetAttributes(source);
                if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string[] hedFiles = Directory.GetFiles(source, "*.hed", SearchOption.AllDirectories);
                    foreach (var hedFile in hedFiles)
                    {
                        FpmfArchive archive = FpmfArchiveIo.Instance.Open(hedFile, hedKey);
                        FpmfArchiveIo.Instance.Save(archive, destination);
                    }
                }
                else
                {
                    FpmfArchive archive = FpmfArchiveIo.Instance.Open(source, FpmfArchiveIo.HedKeyJp);
                    FpmfArchiveIo.Instance.Save(archive, destination);
                }

                return CommandResultType.Completed;
            }

            if (command == "pack")
            {
                // TODO pack needs work
                if (parameter.Arguments.Count < 4)
                {
                    Logger.Error("to few args");
                    return CommandResultType.Error;
                }

                byte[] hedKey = ParseKey(parameter);
                if (parameter.Arguments.Count == 4)
                {
                    FpmfArchiveIo hedFile = new FpmfArchiveIo();
                    hedFile.Pack(parameter.Arguments[1], parameter.Arguments[2], parameter.Arguments[3], hedKey);
                    return CommandResultType.Completed;
                }
                else if (parameter.Arguments.Count == 5)
                {
                    FpmfArchiveIo hedFile = new FpmfArchiveIo();
                    hedFile.Pack(parameter.Arguments[1], parameter.Arguments[2], parameter.Arguments[3], hedKey,
                        parameter.Arguments[4]);
                    return CommandResultType.Completed;
                }
            }

            Logger.Error("Command not valid");
            return CommandResultType.Error;
        }

        private byte[] ParseKey(ConsoleParameter parameter)
        {
            byte[] hedKey = FpmfArchiveIo.HedKeyJp;
            string name = "KeyJp";
            if (parameter.Switches.Contains("-keyUsBeta"))
            {
                hedKey = FpmfArchiveIo.HedKeyUsBeta;
                name = "UsBeta";
            }
            else if (parameter.Switches.Contains("-keyUsSteam"))
            {
                hedKey = FpmfArchiveIo.HedKeyUsSteam;
                name = "UsSteam";
            }
            else if (parameter.Switches.Contains("-keyUsSunset"))
            {
                hedKey = FpmfArchiveIo.HedKeyUsSunset;
                name = "UsSunset";
            }
            else if (parameter.Switches.Contains("-keyJp"))
            {
                hedKey = FpmfArchiveIo.HedKeyJp;
                name = "KeyJp";
            }

            Logger.Info($"Using: {name} Key: {hedKey[0]:X8} {hedKey[1]:X8}");
            return hedKey;
        }

        public override string Key => "hed";

        public override string Description => $@"packs/unpacks hed game data files
`hed [pack|unpack] [source] [destination] (-keyJp|-keyUsBeta|-keyUsSteam|-keyUsSunset)`
Unpack:
`hed unpack ""C:/Games/Wizardry Online/data/settings.hed"" ""C:/output""` - unpack single .hed file
`hed unpack ""C:/Games/Wizardry Online/data"" ""C:/output""`              - unpack all .hed files in sub folders
Pack:
`hed pack ""C:/Games/Wizardry Online/data"" ""C:/output""`
";
    }
}
