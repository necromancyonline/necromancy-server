using System.Collections.Generic;
using Necromancy.Server.Chat.Command;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecRequest
    {
        public SendCmdExecRequest(string command)
        {
            this.command = command;
            parameter = new List<string>();
        }

        public string command { get; set; }
        public List<string> parameter { get; }

        public string CommandString()
        {
            return
                $"{ChatCommandHandler.CHAT_COMMAND_START}{command} {string.Join(ChatCommandHandler.CHAT_COMMAND_SEPARATOR, parameter)}";
        }
    }
}
