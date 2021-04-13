using System;
using Necromancy.Cli.Argument;
using Necromancy.Server.Data;

namespace Necromancy.Cli.Command.Commands
{
    public class PackCommand : ConsoleCommand
    {
        public override string key => "pack";

        /* Please save a copy of your client's data directory before using.
         * Currently need to unpack the client files for the type you want to modify.
         * Example:  unpack "C:\Wizardy Online\data\script.hed" "C:\UnpackedFiles"
         * Once changes are made use "pack source_dir target_dir type"
         * Example: pack "C:\UnpackedFiles" "C:\PackedFiles" script
         * If target .hed is not in the root of the out directory and typethe relative path after "pack source_dir target_dir type path"
         * Example: pack "C:\UnpackedFiles" "C:\PackedFiles" chara 00\01
         * When complete, delete .hed and type, script, directory in client, script.hed and script directory
         * Copy .hed and directory from PackedFiles to client data directory. Example: "C:\Program Files (x86)\Steam\steamapps\common\Wizardry Online\data"
         * Run the client
         */
        public override string description =>
            $"Packs Data. Ex.:{Environment.NewLine}pack \"C:/input\" \"C:/output\" \"script\"";

        public override CommandResultType Handle(ConsoleParameter parameter)
        {
            FpmfArchiveIo archiveIo2 = new FpmfArchiveIo();

            if (parameter.arguments.Count == 3)
            {
                FpmfArchiveIo hedFile = new FpmfArchiveIo();
                hedFile.Pack(parameter.arguments[0], parameter.arguments[1], parameter.arguments[2]);
                return CommandResultType.Completed;
            }

            if (parameter.arguments.Count == 4)
            {
                FpmfArchiveIo hedFile = new FpmfArchiveIo();
                hedFile.Pack(parameter.arguments[0], parameter.arguments[1], parameter.arguments[2], parameter.arguments[3]);
                return CommandResultType.Completed;
            }


            return CommandResultType.Continue;
        }
    }
}
