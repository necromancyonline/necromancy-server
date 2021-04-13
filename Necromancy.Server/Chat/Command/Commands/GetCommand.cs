using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Shows instance IDs for the type entered
    /// </summary>
    public class GetCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(GetCommand));

        public GetCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "get";
        public override string helpText => "usage: `/get [type]` - Displays instnace ID for all objects of 'type' on current map";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == null)
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid argument: {command[0]}"));
                return;
            }

            switch (command[0])
            {
                case "mob":
                    responses.Add(ChatResponse.CommandError(client, "Mob instance ids for current map:"));
                    foreach (KeyValuePair<uint, MonsterSpawn> monster in client.map.monsterSpawns)
                    {
                        MonsterSpawn mobHolder = server.instances.GetInstance(monster.Key) as MonsterSpawn;
                        if (mobHolder != null)
                            responses.Add(ChatResponse.CommandError(client, $"{mobHolder.name} has instance id {mobHolder.instanceId}"));
                    }

                    break;
                case "npc":
                    responses.Add(ChatResponse.CommandError(client, "Npc instance ids for current map:"));
                    foreach (KeyValuePair<uint, NpcSpawn> npc in client.map.npcSpawns)
                    {
                        NpcSpawn npcHolder = server.instances.GetInstance(npc.Key) as NpcSpawn;
                        if (npcHolder != null)
                            responses.Add(ChatResponse.CommandError(client, $"{npcHolder.name} has instance id {npcHolder.instanceId}"));
                    }

                    break;
                case "maptran":
                    responses.Add(ChatResponse.CommandError(client, "Map transition instance ids for current map:"));
                    foreach (KeyValuePair<uint, MapTransition> mapTran in client.map.mapTransitions)
                    {
                        MapTransition mapTranHolder = server.instances.GetInstance(mapTran.Key) as MapTransition;
                        responses.Add(ChatResponse.CommandError(client, $"Map transition {mapTranHolder.id} has instance id {mapTranHolder.instanceId}"));
                    }

                    break;
                case "gimmick":
                    responses.Add(ChatResponse.CommandError(client, "Gimmick instance ids for current map:"));
                    foreach (KeyValuePair<uint, Gimmick> gimmick in client.map.gimmickSpawns)
                    {
                        Gimmick gimmickHolder = server.instances.GetInstance(gimmick.Key) as Gimmick;
                        responses.Add(ChatResponse.CommandError(client, $"Gimmick {gimmickHolder.id} has instance id {gimmickHolder.instanceId}"));
                    }

                    break;
                case "ggate":
                    responses.Add(ChatResponse.CommandError(client, "Ggate instance ids for current map:"));
                    foreach (KeyValuePair<uint, GGateSpawn> ggate in client.map.gGateSpawns)
                    {
                        GGateSpawn ggateHolder = server.instances.GetInstance(ggate.Key) as GGateSpawn;
                        responses.Add(ChatResponse.CommandError(client, $"Ggate {ggateHolder.id} has instance id {ggateHolder.instanceId}"));
                    }

                    break;
                default:
                    _Logger.Error($"Unable to searc for: {command[0]} ");
                    break;
            }
        }
    }
}
