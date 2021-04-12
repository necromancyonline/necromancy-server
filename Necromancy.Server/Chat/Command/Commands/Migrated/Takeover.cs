using System;
using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    //failsafe to end events when frozen
    public class Takeover : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(Takeover));

        public Takeover(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "takeover";

        public override string helpText =>
            "usage: `/takeover ` - Takes over the last object targeted \n `/takeover cancel` - cancels the takeover \n `/takeover save` - saves the takeover to database ";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.character.takeover)
                client.character.takeover = false;
            else if (client.character.takeover == false) client.character.takeover = true;

            if (command[0] == "cancel" || command[0] == "c") client.character.takeover = false;

            IInstance instance = server.instances.GetInstance(client.character.eventSelectReadyCode);

            if (command[0] == "save" || command[0] == "s")
            {
                client.character.takeover = false;

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        _Logger.Debug($"NPCId: {npcSpawn.id} is being updated in the database");
                        npcSpawn.heading = client.character.heading;
                        npcSpawn.x = client.character.x;
                        npcSpawn.y = client.character.y;
                        npcSpawn.z = client.character.z;
                        npcSpawn.updated = DateTime.Now;
                        if (!server.database.UpdateNpcSpawn(npcSpawn)) _Logger.Error("Could not update the database");

                        break;
                    case MonsterSpawn monsterSpawn:
                        _Logger.Debug($"MonsterId: {monsterSpawn.id} is being updated in the database");
                        monsterSpawn.heading = client.character.heading;
                        monsterSpawn.x = client.character.x;
                        monsterSpawn.y = client.character.y;
                        monsterSpawn.z = client.character.z;
                        monsterSpawn.updated = DateTime.Now;
                        if (!server.database.UpdateMonsterSpawn(monsterSpawn)) _Logger.Error("Could not update the database");

                        break;
                    case Character character:
                        _Logger.Debug($"CharacterId: {character.id} is being updated in the database");
                        character.heading = client.character.heading;
                        character.x = client.character.x;
                        character.y = client.character.y;
                        character.z = client.character.z;
                        character.mapId = client.character.mapId;
                        //character.Updated = DateTime.Now;
                        if (!server.database.UpdateCharacter(character)) _Logger.Error("Could not update the database");

                        break;
                    case Gimmick gimmick:
                        _Logger.Debug($"MonsterId: {gimmick.id} is being updated in the database");
                        gimmick.heading = client.character.heading;
                        gimmick.x = client.character.x;
                        gimmick.y = client.character.y;
                        gimmick.z = client.character.z;
                        gimmick.updated = DateTime.Now;
                        if (!server.database.UpdateGimmick(gimmick)) _Logger.Error("Could not update the database");

                        break;
                    default:
                        _Logger.Error($"Instance with InstanceId: {instance.instanceId} does not exist");
                        break;
                }
            }
            else if (uint.TryParse(command[0], out uint specifiedInstanceId))
            {
                IInstance specifiedInstance = server.instances.GetInstance(specifiedInstanceId);

                switch (specifiedInstance)
                {
                    case NpcSpawn npcSpawn:
                        _Logger.Debug($"NPCId: {npcSpawn.id} is now under your movement control. /takeover to Cancel");
                        client.character.eventSelectReadyCode = specifiedInstance.instanceId;

                        break;
                    case MonsterSpawn monsterSpawn:
                        _Logger.Debug(
                            $"MonsterId: {monsterSpawn.instanceId} is now under your movement control. /takeover to Cancel");
                        client.character.eventSelectReadyCode = specifiedInstance.instanceId;
                        break;
                    case Character character:
                        _Logger.Debug(
                            $"CharacterId: {character.instanceId} is now under your movement control. /takeover to Cancel");
                        client.character.eventSelectReadyCode = specifiedInstance.instanceId;
                        break;
                    case Skill skill:
                        _Logger.Debug(
                            $"CharacterId: {skill.instanceId} is now under your movement control. /takeover to Cancel");
                        client.character.eventSelectReadyCode = specifiedInstance.instanceId;
                        break;
                    case Gimmick gimmick:
                        _Logger.Debug(
                            $"CharacterId: {gimmick.instanceId} is now under your movement control. /takeover to Cancel");
                        client.character.eventSelectReadyCode = specifiedInstance.instanceId;
                        break;
                    default:
                        _Logger.Error($"Instance with InstanceId: {instance.instanceId} does not exist");
                        break;
                }
            }
            else
            {
                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        _Logger.Debug(
                            $"NPCId: {npcSpawn.instanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case MonsterSpawn monsterSpawn:
                        _Logger.Debug(
                            $"MonsterId: {monsterSpawn.instanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case Character character:
                        _Logger.Debug(
                            $"CharacterId: {character.instanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case Skill skill:
                        _Logger.Debug(
                            $"CharacterId: {skill.instanceId} is now under your movement control. /takeover to Cancel");
                        break;
                    case Gimmick gimmick:
                        _Logger.Debug(
                            $"CharacterId: {gimmick.instanceId} is now under your movement control. /takeover to Cancel");
                        break;
                    default:
                        _Logger.Error($"Instance with InstanceId: {instance.instanceId} does not exist");
                        break;
                }
            }
        }
    }
}
