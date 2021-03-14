using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Does ScriptCommand stuff
    /// </summary>
    public class ScriptCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ScriptCommand));

        public ScriptCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }

            IBuffer res36 = BufferProvider.Provide();

            switch (command[0])
            {
                case "start":
                    IBuffer res21 = BufferProvider.Provide();
                    res21.WriteInt32(1); // 0 = normal 1 = cinematic
                    res21.WriteByte(0);
                    Router.Send(client, (ushort) AreaPacketId.recv_event_start, res21, ServerType.Area);

                    IBuffer res22 = BufferProvider.Provide();

                    res22.WriteCString(command[1]); // lable
                    res22.WriteUInt32(client.Character.InstanceId); //newjp  ObjectId
                    Router.Send(client, (ushort) AreaPacketId.recv_event_script_play, res22, ServerType.Area);

                    break;

                default:
                    Logger.Error($"There is no script of type : {command[1]} ");

                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "script";

        public override string HelpText =>
            "usage: `/script start tutorial\tutorial_soul` - executes the script at the given path";
    }
}
