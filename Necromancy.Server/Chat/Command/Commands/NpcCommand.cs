using System.Collections.Generic;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Spawns a npc
    /// </summary>
    public class NpcCommand : ServerChatCommand
    {
        public NpcCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "npc";
        public override string helpText => "usage: `/npc [npcId] [modelId]` - Spawns an NPC";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int npcId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!server.settingRepository.npc.TryGetValue(npcId, out NpcSetting npcSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid NpcId: {npcId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!server.settingRepository.modelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }

            NpcSpawn npcSpawn = new NpcSpawn();
            server.instances.AssignInstance(npcSpawn);
            npcSpawn.npcId = npcSetting.id;
            npcSpawn.name = npcSetting.name;
            npcSpawn.title = npcSetting.title;
            npcSpawn.level = (byte)npcSetting.level;

            npcSpawn.modelId = modelSetting.id;
            npcSpawn.size = (byte)modelSetting.height;

            npcSpawn.mapId = client.character.mapId;
            npcSpawn.x = client.character.x;
            npcSpawn.y = client.character.y;
            npcSpawn.z = client.character.z;
            npcSpawn.heading = client.character.heading;

            if (!server.database.InsertNpcSpawn(npcSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, "NpcSpawn could not be saved to database"));
                return;
            }

            RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
            router.Send(client.map, npcData);
        }
    }
}
