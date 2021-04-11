using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area.SendCmdExec
{
    public class SendCmdExecHandler : ClientHandlerDeserializer<SendCmdExecRequest>
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCmdExecHandler));

        public SendCmdExecHandler(NecServer server) : base(server, new SendCmdExecDeserializer())
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_cmd_exec;

        public override void HandleRequest(NecClient client, SendCmdExecRequest request)
        {
            string command = request.CommandString();
            for (int i = 0; i < command.Length; i++)
            {
                char c = command[i];
                if (c < 32 || c > 126)
                {
                    // outside of printable ascii range, cut off bad characters for safety
                    if (i <= 0)
                    {
                        // first character already bad, exit
                        _Logger.Info(client, "Command will not be executed due to bad character");
                        return;
                    }

                    command = command.Substring(0, i - 1);
                    break;
                }
            }

            server.chat.commandHandler.HandleCommand(client, command);
            // IBuffer res = BufferProvider.Provide();
            // res.WriteInt32(0);
            // Router.Send(client, (ushort) AreaPacketId.recv_cmd_exec_r, res, ServerType.Area);
        }
    }
}
