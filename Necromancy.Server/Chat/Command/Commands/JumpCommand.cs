using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Moves character x units upward.
    /// </summary>
    public class JumpCommand : ServerChatCommand
    {
        public JumpCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!float.TryParse(command[0], out float x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            client.Character.Z += x;

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.Character.InstanceId);
            res.WriteFloat(client.Character.X);
            res.WriteFloat(client.Character.Y);
            res.WriteFloat(client.Character.Z);
            res.WriteByte(client.Character.Heading);
            res.WriteByte(client.Character.movementAnim);

            Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "jump";
        public override string HelpText => "usage: `/jump [# of units]` - Moves character x units upward.";
    }
}
