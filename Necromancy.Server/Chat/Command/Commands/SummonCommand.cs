using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    ///     Commands to find out who's on a ma.
    /// </summary>
    public class SummonCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SummonCommand));

        public SummonCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "summon";

        public override string helpText =>
            "usage: `/summon [Character.Name] [Soul.Name]` - moves a character location to you";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == "")
            {
                responses.Add(ChatResponse.CommandError(client,
                    "Hi There!  Type /Summon Character.Name Soul.Name to summon a character."));
                responses.Add(ChatResponse.CommandError(client,
                    "If they are offline, it will update their DB position to yours"));
                responses.Add(ChatResponse.CommandError(client,
                    "If they are online, it will warp them to your position."));
                return;
            }

            if (command[1] == "")
            {
                responses.Add(ChatResponse.CommandError(client, "Sooo close.  you forgot the Soul.Name didn't you?"));
                return;
            }

            bool soulFound = false;

            foreach (NecClient otherClient in server.clients.GetAll())
            {
                Character theirCharacter = otherClient.character;
                if (theirCharacter == null) continue;

                _Logger.Debug($"Comparing {theirCharacter.name} to {command[0]}");
                if (theirCharacter.name == command[0])
                {
                    Soul theirSoul = server.database.SelectSoulById(theirCharacter.soulId);
                    responses.Add(ChatResponse.CommandError(client,
                        $"{theirCharacter.name} {theirSoul.name} is on Map {theirCharacter.mapId} with InstanceID {theirCharacter.instanceId}"));
                    if (theirSoul.name == command[1])
                    {
                        soulFound = true;
                        if (server.clients.GetBySoulName(theirSoul.name) != null)
                        {
                            NecClient theirClient = server.clients.GetBySoulName(theirSoul.name);
                            responses.Add(ChatResponse.CommandError(client,
                                $"{theirSoul.name} is online, Moving them to you"));
                            if (theirClient.character.mapId == client.character.mapId)
                            {
                                IBuffer res = BufferProvider.Provide();

                                res.WriteUInt32(theirClient.character.instanceId);
                                res.WriteFloat(client.character.x);
                                res.WriteFloat(client.character.y);
                                res.WriteFloat(client.character.z);
                                res.WriteByte(client.character.heading);
                                res.WriteByte(client.character.movementAnim);
                                router.Send(theirClient.map, (ushort)AreaPacketId.recv_object_point_move_notify, res,
                                    ServerType.Area);
                            }
                            else
                            {
                                int x = (int)client.character.x;
                                int y = (int)client.character.y;
                                int z = (int)client.character.z;
                                int orietation = client.character.heading;
                                MapPosition mapPos = new MapPosition(x, y, z, (byte)orietation);
                                client.map.EnterForce(theirClient, mapPos);
                            }
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                "updaing position to your current position"));
                            theirCharacter.x = client.character.x;
                            theirCharacter.y = client.character.y;
                            theirCharacter.z = client.character.z;
                            theirCharacter.mapId = client.character.mapId;

                            if (!server.database.UpdateCharacter(theirCharacter)) _Logger.Error("Could not update the database with last known player position");
                        }
                    }
                }
            }

            if (soulFound == false)
            {
                _Logger.Error($"There is no command switch or player name matching : {command[0]} ");
                responses.Add(ChatResponse.CommandError(client, $"{command[0]} is not a valid Character name."));
            }
        }
    }
}
