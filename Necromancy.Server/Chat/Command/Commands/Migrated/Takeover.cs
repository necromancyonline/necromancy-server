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
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Takeover));

        public Takeover(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (client.Character.takeover == true)
            {
                client.Character.takeover = false;
            }
            else if (client.Character.takeover == false)
            {
                client.Character.takeover = true;
            }

            if (command[0] == "cancel" || command[0] == "c")
            {
                client.Character.takeover = false;
            }

            IInstance instance = Server.Instances.GetInstance(client.Character.eventSelectReadyCode);

            if (command[0] == "save" || command[0] == "s")
            {
                client.Character.takeover = false;

                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug($"NPCId: {npcSpawn.Id} is being updated in the database");
                        npcSpawn.Heading = (byte) (client.Character.Heading);
                        npcSpawn.X = (client.Character.X);
                        npcSpawn.Y = (client.Character.Y);
                        npcSpawn.Z = (client.Character.Z);
                        npcSpawn.Updated = DateTime.Now;
                        if (!Server.Database.UpdateNpcSpawn(npcSpawn))
                        {
                            Logger.Error("Could not update the database");
                            return;
                        }

                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug($"MonsterId: {monsterSpawn.Id} is being updated in the database");
                        monsterSpawn.Heading = (byte) (client.Character.Heading);
                        monsterSpawn.X = (client.Character.X);
                        monsterSpawn.Y = (client.Character.Y);
                        monsterSpawn.Z = (client.Character.Z);
                        monsterSpawn.Updated = DateTime.Now;
                        if (!Server.Database.UpdateMonsterSpawn(monsterSpawn))
                        {
                            Logger.Error("Could not update the database");
                            return;
                        }

                        break;
                    case Character character:
                        Logger.Debug($"CharacterId: {character.Id} is being updated in the database");
                        character.Heading = (byte) (client.Character.Heading);
                        character.X = (client.Character.X);
                        character.Y = (client.Character.Y);
                        character.Z = (client.Character.Z);
                        character.MapId = client.Character.MapId;
                        //character.Updated = DateTime.Now;
                        if (!Server.Database.UpdateCharacter(character))
                        {
                            Logger.Error("Could not update the database");
                            return;
                        }

                        break;
                    case Gimmick gimmick:
                        Logger.Debug($"MonsterId: {gimmick.Id} is being updated in the database");
                        gimmick.Heading = (byte) (client.Character.Heading);
                        gimmick.X = (client.Character.X);
                        gimmick.Y = (client.Character.Y);
                        gimmick.Z = (client.Character.Z);
                        gimmick.Updated = DateTime.Now;
                        if (!Server.Database.UpdateGimmick(gimmick))
                        {
                            Logger.Error("Could not update the database");
                            return;
                        }

                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                        break;
                }
            }
            else if (uint.TryParse(command[0], out uint specifiedInstanceId))
            {
                IInstance specifiedInstance = Server.Instances.GetInstance(specifiedInstanceId);

                switch (specifiedInstance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug($"NPCId: {npcSpawn.Id} is now under your movement control. /takeover to Cancel");
                        client.Character.eventSelectReadyCode = specifiedInstance.InstanceId;

                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug(
                            $"MonsterId: {monsterSpawn.InstanceId} is now under your movement control. /takeover to Cancel");
                        client.Character.eventSelectReadyCode = specifiedInstance.InstanceId;
                        break;
                    case Character character:
                        Logger.Debug(
                            $"CharacterId: {character.InstanceId} is now under your movement control. /takeover to Cancel");
                        client.Character.eventSelectReadyCode = specifiedInstance.InstanceId;
                        break;
                    case Skill skill:
                        Logger.Debug(
                            $"CharacterId: {skill.InstanceId} is now under your movement control. /takeover to Cancel");
                        client.Character.eventSelectReadyCode = specifiedInstance.InstanceId;
                        break;
                    case Gimmick gimmick:
                        Logger.Debug(
                            $"CharacterId: {gimmick.InstanceId} is now under your movement control. /takeover to Cancel");
                        client.Character.eventSelectReadyCode = specifiedInstance.InstanceId;
                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                        break;
                }
            }
            else
            {
                switch (instance)
                {
                    case NpcSpawn npcSpawn:
                        Logger.Debug(
                            $"NPCId: {npcSpawn.InstanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case MonsterSpawn monsterSpawn:
                        Logger.Debug(
                            $"MonsterId: {monsterSpawn.InstanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case Character character:
                        Logger.Debug(
                            $"CharacterId: {character.InstanceId} is now under your movement control. /takeover to Cancel");

                        break;
                    case Skill skill:
                        Logger.Debug(
                            $"CharacterId: {skill.InstanceId} is now under your movement control. /takeover to Cancel");
                        break;
                    case Gimmick gimmick:
                        Logger.Debug(
                            $"CharacterId: {gimmick.InstanceId} is now under your movement control. /takeover to Cancel");
                        break;
                    default:
                        Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                        break;
                }
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "takeover";

        public override string HelpText =>
            "usage: `/takeover ` - Takes over the last object targeted \n `/takeover cancel` - cancels the takeover \n `/takeover save` - saves the takeover to database ";
    }
}
