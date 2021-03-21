using System;
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
    /// Gimmick related commands.
    /// </summary>
    public class GimmickCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(GimmickCommand));

        public GimmickCommand(NecServer server) : base(server)
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

            if (!int.TryParse(command[1], out int x))
            {
                responses.Add(ChatResponse.CommandError(client, $"Please provide a value.  model Id or instance Id"));
                return;
            }

            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Good Job!"));
            }

            Gimmick myGimmick = new Gimmick();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn"
                    : //spawns a new object on the map at your position.  makes a sign above it.  and jumps over it
                    myGimmick = new Gimmick();
                    Server.Instances.AssignInstance(myGimmick);
                    myGimmick.ModelId = x;
                    myGimmick.X = client.Character.X;
                    myGimmick.Y = client.Character.Y;
                    myGimmick.Z = client.Character.Z;
                    myGimmick.Heading = client.Character.Heading;
                    myGimmick.State = 0xA;
                    myGimmick.MapId = client.Character.MapId;
                    res.WriteUInt32(myGimmick.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteInt32(x); //Gimmick number (from gimmick.csv)
                    res.WriteInt32(0); //Gimmick State
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_gimmick_data, res, ServerType.Area);
                    Logger.Debug($"You just created a gimmick with instance ID {myGimmick.InstanceId}");

                    //Add a sign above them so you know their ID.
                    GGateSpawn gGateSpawn = new GGateSpawn();
                    Server.Instances.AssignInstance(gGateSpawn);
                    res = BufferProvider.Provide();
                    res.WriteUInt32(gGateSpawn
                        .InstanceId); // Unique Object ID.  Crash if already in use (dont use your character ID)
                    res.WriteInt32(gGateSpawn.SerialId); // Serial ID for Interaction? from npc.csv????
                    res.WriteByte(0); // 0 = Text, 1 = F to examine  , 2 or above nothing
                    res.WriteCString($"You spawned a Gimmick model : {myGimmick.ModelId}"); //"0x5B" //Name
                    res.WriteCString($"The Instance ID of your Gimmick is: {myGimmick.InstanceId}"); //"0x5B" //Title
                    res.WriteFloat(client.Character.X); //X Pos
                    res.WriteFloat(client.Character.Y); //Y Pos
                    res.WriteFloat(client.Character.Z + 200); //Z Pos
                    res.WriteByte(client.Character.Heading); //view offset
                    res.WriteInt32(gGateSpawn
                        .ModelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                    res.WriteInt16(gGateSpawn.Size); //  size of the object
                    res.WriteInt32(gGateSpawn.Active); // 0 = collision, 1 = no collision  (active/Inactive?)
                    res.WriteInt32(gGateSpawn
                        .Glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_ggate_stone_data, res,
                        ServerType.Area);

                    //Jump over your gimmick
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z + 500);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(client.Character.movementAnim);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    responses.Add(ChatResponse.CommandError(client, $"Spawned gimmick {myGimmick.ModelId}"));

                    if (command[2] == "add"
                    ) //if you want to send your gimmick straight to the DB.  type Add at the end of the spawn command. 
                    {
                        if (!Server.Database.InsertGimmick(myGimmick))
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                "myGimmick could not be saved to database"));
                            return;
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                $"Added gimmick {myGimmick.Id} to the database"));
                        }
                    }

                    break;
                case "move": //move a gimmick to your current position and heading
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.X = client.Character.X;
                    myGimmick.Y = client.Character.Y;
                    myGimmick.Z = client.Character.Z;
                    myGimmick.Heading = client.Character.Heading;
                    res.WriteUInt32(myGimmick.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    //Jump away from gimmick
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteFloat(client.Character.X - 125);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z + 50);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(client.Character.movementAnim);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "heading": //only update the heading to your current heading
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.Heading = client.Character.Heading;
                    res.WriteUInt32(myGimmick.InstanceId);
                    res.WriteFloat(myGimmick.X);
                    res.WriteFloat(myGimmick.Y);
                    res.WriteFloat(myGimmick.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "rotate": //rotates a gimmick to a specified heading
                    int newHeading = y;
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.Heading = (byte) newHeading;
                    myGimmick.Heading = (byte) y;
                    res.WriteUInt32(myGimmick.InstanceId);
                    res.WriteFloat(myGimmick.X);
                    res.WriteFloat(myGimmick.Y);
                    res.WriteFloat(myGimmick.Z);
                    res.WriteByte((byte) y);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    Logger.Debug($"Gimmick {myGimmick.InstanceId} has been rotated to {y}*2 degrees.");
                    break;
                case "height": //adjusts the height of gimmick by current value +- Y
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.Z = myGimmick.Z + y;
                    res.WriteUInt32(myGimmick.InstanceId);
                    res.WriteFloat(myGimmick.X);
                    res.WriteFloat(myGimmick.Y);
                    res.WriteFloat(myGimmick.Z);
                    res.WriteByte(myGimmick.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    Logger.Debug($"Gimmick {myGimmick.InstanceId} has been adjusted by a height of {y}.");
                    break;
                case "add": //Adds a new entry to the database
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.Updated = DateTime.Now;
                    if (!Server.Database.InsertGimmick(myGimmick))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(
                            ChatResponse.CommandError(client, $"Added gimmick {myGimmick.Id} to the database"));
                    }

                    break;
                case "update": //Updates an existing entry in the database
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    myGimmick.Updated = DateTime.Now;
                    if (!Server.Database.UpdateGimmick(myGimmick))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated gimmick {myGimmick.Id} in the database"));
                    }

                    break;
                case "remove": //removes a gimmick from the database
                    myGimmick = Server.Instances.GetInstance((uint) x) as Gimmick;
                    if (!Server.Database.DeleteGimmick(myGimmick.Id))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myGimmick could not be deleted from database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Removed gimmick {myGimmick.Id} from the database"));
                    }

                    res.WriteUInt32(myGimmick.InstanceId);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_disappear_notify, res, ServerType.Area);
                    break;
                default: //you don't know what you're doing do you?
                    Logger.Error($"There is no recv of type : {command[0]} ");
                {
                    responses.Add(ChatResponse.CommandError(client, $"{command[0]} is not a valid gimmick command."));
                }
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "gimmick";

        public override string HelpText =>
            "usage: `/gimmick [argument] [number] [parameter]` - does something gimmick related";
    }
}
