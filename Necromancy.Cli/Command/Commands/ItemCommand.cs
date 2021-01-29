using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class ItemCommand : ConsoleCommand
    {
        public static readonly ILogger Logger = LogProvider.Logger(typeof(ItemCommand));

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            if (parameter.Arguments.Count < 1)
            {
                Logger.Error("to few args");
                return CommandResultType.Completed;
            }
            
            string command = parameter.Arguments[0].ToLower();
            
            if (command == "decrypt")
            {
                if (parameter.Arguments.Count < 2)
                {
                    // error to few args
                    return CommandResultType.Completed;
                }
                
                Dictionary<uint, string> items = WoItemIo.Instance.OpenWoItm(parameter.Arguments[0],
                    (uint itemId) => { return new byte[0x10]; });
                // TODO save csv file
                return CommandResultType.Completed;
            }
            
            if (command == "encrypt")
            {
                if (parameter.Arguments.Count < 2)
                {
                    // error to few args
                    return CommandResultType.Completed;
                }

                FpmfArchiveIo archiveIO = new FpmfArchiveIo();
                Dictionary<uint, string> items = WoItemIo.Instance.OpenWoItm(parameter.Arguments[0],
                    (uint itemId) => { return new byte[0x10]; });
                // TODO save csv file
                return CommandResultType.Completed;
            }
            
            return CommandResultType.Completed;
        }

        public override string Key => "woitm";

        public override string Description => $@"encrypts/decrypts item.csv
`woitm [encrypt|decrypt] [source] [destination]`
Decrypt:
`woitm decrypt ""C:/Games/Wizardry Online/data/settings.hed"" ""C:/output""` - decrypts source to destination
Encrypt:
`woitm encrypt ""C:/Games/Wizardry Online/data"" ""C:/output""`              - encrypts source to destination
";
    }
}
