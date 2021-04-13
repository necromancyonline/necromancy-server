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
    ///     Does ScriptCommand stuff
    /// </summary>
    public class ScriptCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ScriptCommand));

        public ScriptCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "script";

        public override string helpText =>
            "usage: `/script start tutorial\tutorial_soul` - executes the script at the given path";

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
                    router.Send(client, (ushort)AreaPacketId.recv_event_start, res21, ServerType.Area);

                    IBuffer res22 = BufferProvider.Provide();

                    res22.WriteCString(command[1]); // lable
                    res22.WriteUInt32(client.character.instanceId); //newjp  ObjectId
                    router.Send(client, (ushort)AreaPacketId.recv_event_script_play, res22, ServerType.Area);

                    break;

                default:
                    _Logger.Error($"There is no script of type : {command[1]} ");

                    break;
            }
        }
    }
}
