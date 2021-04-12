using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Changes the map
    /// </summary>
    public class SendMapChangeForce : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendMapChangeForce));

        public SendMapChangeForce(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "map";
        public override string helpText => "usage: `/map [mapId]` - Changes the map";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0].Length == 0)
            {
                _Logger.Debug("Re-entering current map");
                command[0] = $"{client.character.mapId}";
            }

            if (!int.TryParse(command[0], out int mapId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!server.maps.TryGet(mapId, out Map map))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MapId: {mapId}"));
                return;
            }

            map.EnterForce(client);
        }
    }
}
