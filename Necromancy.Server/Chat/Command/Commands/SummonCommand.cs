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
    /// Commands to find out who's on a ma.
    /// </summary>
    public class SummonCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(SummonCommand));

        public SummonCommand(NecServer server) : base(server)
        {
        }

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            if (command[0] == "")
            {
                responses.Add(ChatResponse.CommandError(client,
                    $"Hi There!  Type /Summon Character.Name Soul.Name to summon a character."));
                responses.Add(ChatResponse.CommandError(client,
                    $"If they are offline, it will update their DB position to yours"));
                responses.Add(ChatResponse.CommandError(client,
                    $"If they are online, it will warp them to your position."));
                return;
            }

            if (command[1] == "")
            {
                responses.Add(ChatResponse.CommandError(client, $"Sooo close.  you forgot the Soul.Name didn't you?"));
                return;
            }

            bool soulFound = false;

            foreach (NecClient otherClient in Server.Clients.GetAll())
            {
                Character theirCharacter = otherClient.Character;
                if (theirCharacter == null)
                {
                    continue;
                }

                Logger.Debug($"Comparing {theirCharacter.Name} to {command[0]}");
                if (theirCharacter.Name == command[0])
                {
                    Soul theirSoul = Server.Database.SelectSoulById(theirCharacter.SoulId);
                    responses.Add(ChatResponse.CommandError(client,
                        $"{theirCharacter.Name} {theirSoul.Name} is on Map {theirCharacter.MapId} with InstanceID {theirCharacter.InstanceId}"));
                    if (theirSoul.Name == command[1])
                    {
                        soulFound = true;
                        if (Server.Clients.GetBySoulName(theirSoul.Name) != null)
                        {
                            NecClient theirClient = Server.Clients.GetBySoulName(theirSoul.Name);
                            responses.Add(ChatResponse.CommandError(client,
                                $"{theirSoul.Name} is online, Moving them to you"));
                            if (theirClient.Character.MapId == client.Character.MapId)
                            {
                                IBuffer res = BufferProvider.Provide();

                                res.WriteUInt32(theirClient.Character.InstanceId);
                                res.WriteFloat(client.Character.X);
                                res.WriteFloat(client.Character.Y);
                                res.WriteFloat(client.Character.Z);
                                res.WriteByte(client.Character.Heading);
                                res.WriteByte(client.Character.movementAnim);
                                Router.Send(theirClient.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res,
                                    ServerType.Area);
                            }
                            else
                            {
                                int X = (int) client.Character.X;
                                int Y = (int) client.Character.Y;
                                int Z = (int) client.Character.Z;
                                int Orietation = client.Character.Heading;
                                MapPosition mapPos = new MapPosition(X, Y, Z, (byte) Orietation);
                                client.Map.EnterForce(theirClient, mapPos);
                            }
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                $"updaing position to your current position"));
                            theirCharacter.X = client.Character.X;
                            theirCharacter.Y = client.Character.Y;
                            theirCharacter.Z = client.Character.Z;
                            theirCharacter.MapId = client.Character.MapId;

                            if (!Server.Database.UpdateCharacter(theirCharacter))
                            {
                                Logger.Error("Could not update the database with last known player position");
                            }
                        }
                    }
                }
            }

            if (soulFound == false)
            {
                Logger.Error($"There is no command switch or player name matching : {command[0]} ");
                responses.Add(ChatResponse.CommandError(client, $"{command[0]} is not a valid Character name."));
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "summon";

        public override string HelpText =>
            "usage: `/summon [Character.Name] [Soul.Name]` - moves a character location to you";
    }
}
