using System.Collections.Generic;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a npc
    /// </summary>
    public class NpcCommand : ServerChatCommand
    {
        public NpcCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (!int.TryParse(command[0], out int npcId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!Server.SettingRepository.Npc.TryGetValue(npcId, out NpcSetting npcSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid NpcId: {npcId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }

            if (!Server.SettingRepository.ModelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }

            NpcSpawn npcSpawn = new NpcSpawn();
            Server.Instances.AssignInstance(npcSpawn);
            npcSpawn.NpcId = npcSetting.Id;
            npcSpawn.Name = npcSetting.Name;
            npcSpawn.Title = npcSetting.Title;
            npcSpawn.Level = (byte) npcSetting.Level;

            npcSpawn.ModelId = modelSetting.Id;
            npcSpawn.Size = (byte) modelSetting.Height;

            npcSpawn.MapId = client.Character.MapId;
            npcSpawn.X = client.Character.X;
            npcSpawn.Y = client.Character.Y;
            npcSpawn.Z = client.Character.Z;
            npcSpawn.Heading = client.Character.Heading;

            if (!Server.Database.InsertNpcSpawn(npcSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, $"NpcSpawn could not be saved to database"));
                return;
            }

            RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
            Router.Send(client.Map, npcData);
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "npc";
        public override string HelpText => "usage: `/npc [npcId] [modelId]` - Spawns an NPC";
    }
}
