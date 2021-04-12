using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Commands to find out who's on a map.
    /// </summary>
    public class PlayersCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(PlayersCommand));

        public PlayersCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.User;
        public override string key => "players";
        public override string helpText => "usage: `/players [map|world|{characterName}]`";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command.Length < 1)
            {
                responses.Add(ChatResponse.CommandError(client, "To few arguments"));
                return;
            }

            switch (command[0])
            {
                case "map":
                {
                    foreach (NecClient theirClient in client.map.clientLookup.GetAll())
                        responses.Add(ChatResponse.CommandInfo(client,
                            $"{theirClient.character.name} {theirClient.soul.name} is on Map {theirClient.character.mapId} with InstanceID {theirClient.character.instanceId}"));

                    break;
                }
                case "world":
                {
                    foreach (NecClient theirClient in server.clients.GetAll())
                        if (theirClient.map != null)
                            responses.Add(ChatResponse.CommandInfo(client,
                                $"{theirClient.character.name} {theirClient.soul.name} is on Map {theirClient.character.mapId} with InstanceID {theirClient.character.instanceId}"));

                    break;
                }

                default:
                    foreach (NecClient otherClient in server.clients.GetAll())
                    {
                        Character character = otherClient.character;
                        if (character == null) continue;

                        if (character.name.Equals(command[0], StringComparison.InvariantCultureIgnoreCase))
                        {
                            string mapName = "None";
                            Map map = client.map;
                            if (map != null) mapName = $"{map.id} ({map.place})";

                            responses.Add(ChatResponse.CommandInfo(client,
                                $"CharacterName: {character.name} SoulId:{character.soulId} Map:{mapName} InstanceId: {character.instanceId}"));
                            return;
                        }
                    }

                    responses.Add(ChatResponse.CommandError(client, $"Character: '{command[0]}' not found"));
                    break;
            }
        }
    }
}
