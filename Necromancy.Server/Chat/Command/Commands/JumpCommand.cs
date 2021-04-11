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

            client.character.z += x;

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(client.character.instanceId);
            res.WriteFloat(client.character.x);
            res.WriteFloat(client.character.y);
            res.WriteFloat(client.character.z);
            res.WriteByte(client.character.heading);
            res.WriteByte(client.character.movementAnim);

            router.Send(client.map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "jump";
        public override string helpText => "usage: `/jump [# of units]` - Moves character x units upward.";
    }
}
