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
    ///     GGateSpawn related commands.
    /// </summary>
    public class GGateCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(GGateCommand));

        public GGateCommand(NecServer server) : base(server)
        {
            int[] gGateModelIds =
            {
                1800000, /*	Stone slab of guardian statue	*/
                1801000, /*	Bulletin board	*/
                1802000, /*	Sign	*/
                1803000, /*	Stone board	*/
                1804000, /*	Guardians Gate	*/
                1805000, /*	Warp device	*/
                1806000, /*	Puddle	*/
                1807000, /*	machine	*/
                1808000, /*	Junk mountain	*/
                1809000, /*	switch	*/
                1810000, /*	Statue	*/
                1811000, /*	Horse statue	*/
                1812000, /*	Agate balance	*/
                1813000, /*	Dagger scale	*/
                1814000, /*	Apple balance	*/
                1815000, /*	torch	*/
                1816000, /*	Royal shop sign	*/
                1817000, /*	Witch pot	*/
                1818000, /*	toilet	*/
                1819000, /*	Abandoned tree	*/
                1820000, /*	Pedestal with fire	*/
                1900000, /*	For transparency	*/
                1900001 /*	For transparency	*/
            };
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "ggate";

        public override string helpText =>
            "usage: `/GGateSpawn [argument] [number] [parameter]` - does something GGateSpawn related";

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

            GGateSpawn myGGateSpawn = new GGateSpawn();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn"
                    : //spawns a new object on the map at your position.  makes a sign above it.  and jumps over it
                    myGGateSpawn = new GGateSpawn();
                    server.instances.AssignInstance(myGGateSpawn);
                    myGGateSpawn.modelId = x;
                    myGGateSpawn.x = client.character.x;
                    myGGateSpawn.y = client.character.y;
                    myGGateSpawn.z = client.character.z;
                    myGGateSpawn.heading = client.character.heading;
                    myGGateSpawn.mapId = client.character.mapId;
                    myGGateSpawn.name = "";
                    myGGateSpawn.title = "";
                    myGGateSpawn.size = 100;
                    myGGateSpawn.active = 0;
                    myGGateSpawn.glow = 2;
                    myGGateSpawn.interaction = 1;

                    //Add a sign above them so you know their ID.
                    res = BufferProvider.Provide();
                    res.WriteUInt32(myGGateSpawn
                        .instanceId); // Unique Object ID.  Crash if already in use (dont use your character ID)
                    res.WriteInt32(myGGateSpawn.serialId); // Serial ID for Interaction? from npc.csv????
                    res.WriteByte(myGGateSpawn.interaction); // 0 = Text, 1 = F to examine  , 2 or above nothing
                    res.WriteCString($"You spawned a GGateSpawn model : {myGGateSpawn.modelId}"); //"0x5B" //Name
                    res.WriteCString(
                        $"The Instance ID of your GGateSpawn is: {myGGateSpawn.instanceId}"); //"0x5B" //Title
                    res.WriteFloat(client.character.x); //X Pos
                    res.WriteFloat(client.character.y); //Y Pos
                    res.WriteFloat(client.character.z); //Z Pos
                    res.WriteByte(client.character.heading); //view offset
                    res.WriteInt32(myGGateSpawn
                        .modelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                    res.WriteInt16(myGGateSpawn.size); //  size of the object
                    res.WriteInt32(myGGateSpawn
                        .active); // 0 = collision, 1 = no collision  (active/Inactive?) //rename this to state...... >.>
                    res.WriteInt32(myGGateSpawn
                        .glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx
                    router.Send(client.map, (ushort)AreaPacketId.recv_data_notify_ggate_stone_data, res,
                        ServerType.Area);

                    //Jump over your GGateSpawn
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.character.instanceId);
                    res.WriteFloat(client.character.x);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z + 500);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(client.character.movementAnim);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    responses.Add(ChatResponse.CommandError(client, $"Spawned GGateSpawn {myGGateSpawn.modelId} with instance id {myGGateSpawn.instanceId}"));

                    if (command[2] == "add"
                    ) //if you want to send your GGateSpawn straight to the DB.  type Add at the end of the spawn command.
                    {
                        if (!server.database.InsertGGateSpawn(myGGateSpawn))
                            responses.Add(ChatResponse.CommandError(client,
                                "myGGateSpawn could not be saved to database"));
                        else
                            responses.Add(ChatResponse.CommandError(client,
                                $"Added GGateSpawn {myGGateSpawn.id} to the database"));
                    }

                    break;
                case "move": //move a GGateSpawn to your current position and heading
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.x = client.character.x;
                    myGGateSpawn.y = client.character.y;
                    myGGateSpawn.z = client.character.z;
                    myGGateSpawn.heading = client.character.heading;
                    res.WriteUInt32(myGGateSpawn.instanceId);
                    res.WriteFloat(client.character.x);
                    res.WriteFloat(client.character.y);
                    res.WriteFloat(client.character.z);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    //Jump away from GGateSpawn
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
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.heading = client.character.heading;
                    res.WriteUInt32(myGGateSpawn.instanceId);
                    res.WriteFloat(myGGateSpawn.x);
                    res.WriteFloat(myGGateSpawn.y);
                    res.WriteFloat(myGGateSpawn.z);
                    res.WriteByte(client.character.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "rotate": //rotates a GGateSpawn to a specified heading
                    int newHeading = y;
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.heading = (byte)newHeading;
                    myGGateSpawn.heading = (byte)y;
                    res.WriteUInt32(myGGateSpawn.instanceId);
                    res.WriteFloat(myGGateSpawn.x);
                    res.WriteFloat(myGGateSpawn.y);
                    res.WriteFloat(myGGateSpawn.z);
                    res.WriteByte((byte)y);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    _Logger.Debug($"GGateSpawn {myGGateSpawn.instanceId} has been rotated to {y}*2 degrees.");
                    break;
                case "height": //adjusts the height of GGateSpawn by current value +- Y
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.z = myGGateSpawn.z + y;
                    res.WriteUInt32(myGGateSpawn.instanceId);
                    res.WriteFloat(myGGateSpawn.x);
                    res.WriteFloat(myGGateSpawn.y);
                    res.WriteFloat(myGGateSpawn.z);
                    res.WriteByte(myGGateSpawn.heading);
                    res.WriteByte(0xA);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    _Logger.Debug($"GGateSpawn {myGGateSpawn.instanceId} has been adjusted by a height of {y}.");
                    break;
                case "add": //Adds a new entry to the database
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.updated = DateTime.Now;
                    if (!server.database.InsertGGateSpawn(myGGateSpawn))
                        responses.Add(ChatResponse.CommandError(client, "myGGateSpawn could not be saved to database"));
                    else
                        responses.Add(ChatResponse.CommandError(client,
                            $"Added GGateSpawn {myGGateSpawn.id} to the database"));

                    break;
                case "update": //Updates an existing entry in the database
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    myGGateSpawn.updated = DateTime.Now;
                    if (!server.database.UpdateGGateSpawn(myGGateSpawn))
                        responses.Add(ChatResponse.CommandError(client, "myGGateSpawn could not be saved to database"));
                    else
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated GGateSpawn {myGGateSpawn.id} in the database"));

                    break;
                case "remove": //removes a GGateSpawn from the database
                    myGGateSpawn = server.instances.GetInstance((uint)x) as GGateSpawn;
                    if (!server.database.DeleteGGateSpawn(myGGateSpawn.id))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myGGateSpawn could not be deleted from database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Removed GGateSpawn {myGGateSpawn.id} from the database"));
                    }

                    res.WriteUInt32(myGGateSpawn.instanceId);
                    router.Send(client.map, (ushort)AreaPacketId.recv_object_disappear_notify, res, ServerType.Area);
                    break;
                case "state": //updates GGate State.
                    res.WriteUInt32(myGGateSpawn.instanceId);
                    res.WriteInt32(y);
                    router.Send(client.map, (ushort)AreaPacketId.recv_npc_ggate_state_update_notify, res,
                        ServerType.Area);
                    break;
                default: //you don't know what you're doing do you?
                    _Logger.Error($"There is no recv of type : {command[0]} ");
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"{command[0]} is not a valid GGateSpawn command."));
                    }
                    break;
            }
        }
    }
}
