using System.Collections.Generic;
using Necromancy.Server.Model;
using Arrowgene.Logging;
using Necromancy.Server.Logging;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Shows instance IDs for the type entered
    /// </summary>
    public class GetCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(GetCommand));
        public GetCommand(NecServer server) : base(server)
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

            switch (command[0])
            {
                case "mob":
                    responses.Add(ChatResponse.CommandError(client, "Mob instance ids for current map:"));
                    foreach (KeyValuePair<uint, MonsterSpawn> monster in client.Map.MonsterSpawns)
                    {
                        MonsterSpawn mobHolder = Server.Instances.GetInstance(monster.Key) as MonsterSpawn;
                        if (mobHolder != null)
                            responses.Add(ChatResponse.CommandError(client, $"{mobHolder.Name} has instance id {mobHolder.InstanceId}"));
                    }
                    break;
                case "npc":
                    responses.Add(ChatResponse.CommandError(client, "Npc instance ids for current map:"));
                    foreach (KeyValuePair<uint, NpcSpawn> npc in client.Map.NpcSpawns)
                    {
                        NpcSpawn npcHolder = Server.Instances.GetInstance(npc.Key) as NpcSpawn;
                        if (npcHolder != null)
                            responses.Add(ChatResponse.CommandError(client, $"{npcHolder.Name} has instance id {npcHolder.InstanceId}"));
                        
                    }
                    break;
                case "maptran":
                    responses.Add(ChatResponse.CommandError(client, "Map transition instance ids for current map:"));
                    foreach (KeyValuePair<uint, MapTransition> mapTran in client.Map.MapTransitions)
                    {
                        MapTransition mapTranHolder = Server.Instances.GetInstance(mapTran.Key) as MapTransition;
                        responses.Add(ChatResponse.CommandError(client, $"Map transition {mapTranHolder.Id} has instance id {mapTranHolder.InstanceId}"));
                    }
                    break;
                case "gimmick":
                    responses.Add(ChatResponse.CommandError(client, "Gimmick instance ids for current map:"));
                    foreach (KeyValuePair<uint, Gimmick> gimmick in client.Map.GimmickSpawns)
                    {
                        Gimmick gimmickHolder = Server.Instances.GetInstance(gimmick.Key) as Gimmick;
                        responses.Add(ChatResponse.CommandError(client, $"Gimmick {gimmickHolder.Id} has instance id {gimmickHolder.InstanceId}"));
                    }
                    break;
                case "ggate":
                    responses.Add(ChatResponse.CommandError(client, "Ggate instance ids for current map:"));
                    foreach (KeyValuePair<uint, GGateSpawn> ggate in client.Map.GGateSpawns)
                    {
                        GGateSpawn ggateHolder = Server.Instances.GetInstance(ggate.Key) as GGateSpawn;
                        responses.Add(ChatResponse.CommandError(client, $"Ggate {ggateHolder.Id} has instance id {ggateHolder.InstanceId}"));
                    }
                    break;
                default:
                        Logger.Error($"Unable to searc for: {command[0]} ");
                        break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "get";
        public override string HelpText => "usage: `/get [type]` - Displays instnace ID for all objects of 'type' on current map";
    }
}
