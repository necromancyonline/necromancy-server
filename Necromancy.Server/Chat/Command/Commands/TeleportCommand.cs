using System.Collections.Generic;
using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Moves character location to x, y, z.
    /// </summary>
    public class TeleportCommand : ServerChatCommand
    {
        public TeleportCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "tp";
        public override string helpText => "usage: `/tp x, y, z` - Moves character to location x, y, z.";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!float.TryParse(command[0], out float x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!float.TryParse(command[1], out float y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!float.TryParse(command[2], out float z))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[2]}"));
                return;
            }

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);
            res.WriteFloat(x);
            res.WriteFloat(y);
            res.WriteFloat(z);
            res.WriteByte(client.character.heading);
            res.WriteByte(client.character.movementAnim);

            router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
        }
    }
}
