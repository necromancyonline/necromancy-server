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
    ///     Gimmick related commands.
    /// </summary>
    public class GimmickCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(GimmickCommand));

        public GimmickCommand(NecServer server) : base(server)
        {
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "gimmick";

        public override string helpText =>
            "usage: `/gimmick [argument] [number] [parameter]` - does something gimmick related";

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
                responses.Add(ChatResponse.CommandError(client, "Please provide a value.  model Id or instance Id"));
                return;
            }

            if (!int.TryParse(command[2], out int y)) responses.Add(ChatResponse.CommandError(client, "Good Job!"));

            Gimmick myGimmick = new Gimmick();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn"
                    : //spawns a new object on the map at your position.  makes a sign above it.  and jumps over it
                    myGimmick = new Gimmick();
                    server.instances.AssignInstance(myGimmick);
                    myGimmick.modelId = x;
                    myGimmick.x = client.character.x;
                    myGimmick.y = client.character.y;
                    myGimmick.z = client.character.z;
                    myGimmick.heading = client.character.heading;
                    myGimmick.state = 0xA;
                    myGimmick.mapId = client.character.mapId;
                    res.WriteUInt32(myGimmick.instanceId);
                    res.WriteFloat(client.character.x);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z);
                    res.WriteByte(client.character.heading);
                    res.WriteInt32(x); //Gimmick number (from gimmick.csv)
                    res.WriteInt32(0); //Gimmick State
                    router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_gimmick_data, res, ServerType.Area);
                    _Logger.Debug($"You just created a gimmick with instance ID {myGimmick.instanceId}");

                    //Add a sign above them so you know their ID.
                    GGateSpawn gGateSpawn = new GGateSpawn();
                    server.instances.AssignInstance(gGateSpawn);
                    res = BufferProvider.Provide();
                    res.WriteUInt32(gGateSpawn
                        .instanceId); // Unique Object ID.  Crash if already in use (dont use your character ID)
                    res.WriteInt32(gGateSpawn.serialId); // Serial ID for Interaction? from npc.csv????
                    res.WriteByte(0); // 0 = Text, 1 = F to examine  , 2 or above nothing
                    res.WriteCString($"You spawned a Gimmick model : {myGimmick.modelId}"); //"0x5B" //Name
                    res.WriteCString($"The Instance ID of your Gimmick is: {myGimmick.instanceId}"); //"0x5B" //Title
                    res.WriteFloat(client.character.x); //X Pos
                    res.WriteFloat(client.character.y); //Y Pos
                    res.WriteFloat(client.character.z + 200); //Z Pos
                    res.WriteByte(client.character.heading); //view offset
                    res.WriteInt32(gGateSpawn
                        .modelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                    res.WriteInt16(gGateSpawn.size); //  size of the object
                    res.WriteInt32(gGateSpawn.active); // 0 = collision, 1 = no collision  (active/Inactive?)
                    res.WriteInt32(gGateSpawn
                        .glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx
                    router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_ggate_stone_data, res,
                        ServerType.Area);

                    //Jump over your gimmick
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.character.instanceId);
                    res.WriteFloat(client.character.x);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z + 500);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(client.character.movementAnim);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    responses.Add(ChatResponse.CommandError(client, $"Spawned gimmick {myGimmick.modelId}"));

                    if (command[2] == "add"
                    ) //if you want to send your gimmick straight to the DB.  type Add at the end of the spawn command.
                    {
                        if (!server.database.InsertGimmick(myGimmick))
                            responses.Add(ChatResponse.CommandError(client,
                                "myGimmick could not be saved to database"));
                        else
                            responses.Add(ChatResponse.CommandError(client,
                                $"Added gimmick {myGimmick.id} to the database"));
                    }

                    break;
                case "move": //move a gimmick to your current position and heading
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.x = client.character.x;
                    myGimmick.y = client.character.y;
                    myGimmick.z = client.character.z;
                    myGimmick.heading = client.character.heading;
                    res.WriteUInt32(myGimmick.instanceId);
                    res.WriteFloat(client.character.x);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    //Jump away from gimmick
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.character.instanceId);
                    res.WriteFloat(client.character.x - 125);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z + 50);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(client.character.movementAnim);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "heading": //only update the heading to your current heading
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.heading = client.character.heading;
                    res.WriteUInt32(myGimmick.instanceId);
                    res.WriteFloat(myGimmick.x);
                    res.WriteFloat(myGimmick.y);
                    res.WriteFloat(myGimmick.z);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "rotate": //rotates a gimmick to a specified heading
                    int newHeading = y;
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.heading = (byte)newHeading;
                    myGimmick.heading = (byte)y;
                    res.WriteUInt32(myGimmick.instanceId);
                    res.WriteFloat(myGimmick.x);
                    res.WriteFloat(myGimmick.y);
                    res.WriteFloat(myGimmick.z);
                    res.WriteByte((byte)y);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    _Logger.Debug($"Gimmick {myGimmick.instanceId} has been rotated to {y}*2 degrees.");
                    break;
                case "height": //adjusts the height of gimmick by current value +- Y
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.z = myGimmick.z + y;
                    res.WriteUInt32(myGimmick.instanceId);
                    res.WriteFloat(myGimmick.x);
                    res.WriteFloat(myGimmick.y);
                    res.WriteFloat(myGimmick.z);
                    res.WriteByte(myGimmick.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    _Logger.Debug($"Gimmick {myGimmick.instanceId} has been adjusted by a height of {y}.");
                    break;
                case "add": //Adds a new entry to the database
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.updated = DateTime.Now;
                    if (!server.database.InsertGimmick(myGimmick))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGimmick could not be saved to database"));
                    }
                    else
                    {
                        responses.Add(
                            ChatResponse.CommandError(client, $"Added gimmick {myGimmick.id} to the database"));
                    }

                    break;
                case "update": //Updates an existing entry in the database
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    myGimmick.updated = DateTime.Now;
                    if (!server.database.UpdateGimmick(myGimmick))
                        responses.Add(ChatResponse.CommandError(client, "myGimmick could not be saved to database"));
                    else
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated gimmick {myGimmick.id} in the database"));

                    break;
                case "remove": //removes a gimmick from the database
                    myGimmick = server.instances.GetInstance((uint)x) as Gimmick;
                    if (!server.database.DeleteGimmick(myGimmick.id))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myGimmick could not be deleted from database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Removed gimmick {myGimmick.id} from the database"));
                    }

                    res.WriteUInt32(myGimmick.instanceId);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_disappear_notify, res, ServerType.Area);
                    break;
                default: //you don't know what you're doing do you?
                    _Logger.Error($"There is no recv of type : {command[0]} ");
                {
                    responses.Add(ChatResponse.CommandError(client, $"{command[0]} is not a valid gimmick command."));
                }
                    break;
            }
        }
    }
}
