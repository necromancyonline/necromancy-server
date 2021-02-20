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
                $"AccountId: {client.Account.Id} SoulId: {client.Soul.Id} CharacterId:{client.Character.Id} InstanceId: {client.Character.InstanceId} State: {client.Character.State}"));
            responses.Add(ChatResponse.CommandError(client,
                $"MapId: {client.Character.MapId} X: {client.Character.X} Y:{client.Character.Y} Z:{client.Character.Z}  H:{client.Character.Heading}"));
        }

        public override AccountStateType AccountState => AccountStateType.User;
        public override string Key => "status";
        public override string HelpText => "usage: `/status` - Display current values";
    }
}
