using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Prints current status to console
    /// </summary>
    public class StatusCommand : ServerChatCommand
    {
        public StatusCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            responses.Add(ChatResponse.CommandError(client, "-----Status-----"));
            responses.Add(ChatResponse.CommandError(client,
                $"AccountId: {client.account.id} SoulId: {client.soul.id} CharacterId:{client.character.id} InstanceId: {client.character.instanceId} State: {client.character.state}"));
            responses.Add(ChatResponse.CommandError(client,
                $"MapId: {client.character.mapId} X: {client.character.x} Y:{client.character.y} Z:{client.character.z}  H:{client.character.heading}"));
        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "status";
        public override string helpText => "usage: `/status` - Display current values";
    }
}
