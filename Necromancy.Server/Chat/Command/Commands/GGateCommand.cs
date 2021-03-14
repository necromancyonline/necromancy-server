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
    /// GGateSpawn related commands.
    /// </summary>
    public class GGateCommand : ServerChatCommand
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(GGateCommand));

        public GGateCommand(NecServer server) : base(server)
        {
            int[] GGateModelIds = new int[]
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
                1900001, /*	For transparency	*/
            };
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

            GGateSpawn myGGateSpawn = new GGateSpawn();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn"
                    : //spawns a new object on the map at your position.  makes a sign above it.  and jumps over it
                    myGGateSpawn = new GGateSpawn();
                    Server.Instances.AssignInstance(myGGateSpawn);
                    myGGateSpawn.ModelId = x;
                    myGGateSpawn.X = client.Character.X;
                    myGGateSpawn.Y = client.Character.Y;
                    myGGateSpawn.Z = client.Character.Z;
                    myGGateSpawn.Heading = client.Character.Heading;
                    myGGateSpawn.MapId = client.Character.MapId;
                    myGGateSpawn.Name = "";
                    myGGateSpawn.Title = "";
                    myGGateSpawn.Size = 100;
                    myGGateSpawn.Active = 0;
                    myGGateSpawn.Glow = 2;
                    myGGateSpawn.Interaction = 1;

                    //Add a sign above them so you know their ID.
                    res = BufferProvider.Provide();
                    res.WriteUInt32(myGGateSpawn
                        .InstanceId); // Unique Object ID.  Crash if already in use (dont use your character ID)
                    res.WriteInt32(myGGateSpawn.SerialId); // Serial ID for Interaction? from npc.csv????
                    res.WriteByte(myGGateSpawn.Interaction); // 0 = Text, 1 = F to examine  , 2 or above nothing
                    res.WriteCString($"You spawned a GGateSpawn model : {myGGateSpawn.ModelId}"); //"0x5B" //Name
                    res.WriteCString(
                        $"The Instance ID of your GGateSpawn is: {myGGateSpawn.InstanceId}"); //"0x5B" //Title
                    res.WriteFloat(client.Character.X); //X Pos
                    res.WriteFloat(client.Character.Y); //Y Pos
                    res.WriteFloat(client.Character.Z); //Z Pos
                    res.WriteByte(client.Character.Heading); //view offset
                    res.WriteInt32(myGGateSpawn
                        .ModelId); // Optional Model ID. Warp Statues. Gaurds, Pedistals, Etc., to see models refer to the model_common.csv
                    res.WriteInt16(myGGateSpawn.Size); //  size of the object
                    res.WriteInt32(myGGateSpawn
                        .Active); // 0 = collision, 1 = no collision  (active/Inactive?) //rename this to state...... >.>
                    res.WriteInt32(myGGateSpawn
                        .Glow); //0= no effect color appear, //Red = 0bxx1x   | Gold = obxxx1   |blue = 0bx1xx
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_data_notify_ggate_stone_data, res,
                        ServerType.Area);

                    //Jump over your GGateSpawn
                    res = BufferProvider.Provide();
                    res.WriteUInt32(client.Character.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z + 500);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(client.Character.movementAnim);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    responses.Add(ChatResponse.CommandError(client, $"Spawned GGateSpawn {myGGateSpawn.ModelId} with instance id {myGGateSpawn.InstanceId}"));

                    if (command[2] == "add"
                    ) //if you want to send your GGateSpawn straight to the DB.  type Add at the end of the spawn command. 
                    {
                        if (!Server.Database.InsertGGateSpawn(myGGateSpawn))
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                "myGGateSpawn could not be saved to database"));
                            return;
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                $"Added GGateSpawn {myGGateSpawn.Id} to the database"));
                        }
                    }

                    break;
                case "move": //move a GGateSpawn to your current position and heading
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.X = client.Character.X;
                    myGGateSpawn.Y = client.Character.Y;
                    myGGateSpawn.Z = client.Character.Z;
                    myGGateSpawn.Heading = client.Character.Heading;
                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    res.WriteFloat(client.Character.X);
                    res.WriteFloat(client.Character.Y);
                    res.WriteFloat(client.Character.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);

                    //Jump away from GGateSpawn
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
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.Heading = client.Character.Heading;
                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    res.WriteFloat(myGGateSpawn.X);
                    res.WriteFloat(myGGateSpawn.Y);
                    res.WriteFloat(myGGateSpawn.Z);
                    res.WriteByte(client.Character.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    break;
                case "rotate": //rotates a GGateSpawn to a specified heading
                    int newHeading = y;
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.Heading = (byte) newHeading;
                    myGGateSpawn.Heading = (byte) y;
                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    res.WriteFloat(myGGateSpawn.X);
                    res.WriteFloat(myGGateSpawn.Y);
                    res.WriteFloat(myGGateSpawn.Z);
                    res.WriteByte((byte) y);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    Logger.Debug($"GGateSpawn {myGGateSpawn.InstanceId} has been rotated to {y}*2 degrees.");
                    break;
                case "height": //adjusts the height of GGateSpawn by current value +- Y
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.Z = myGGateSpawn.Z + y;
                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    res.WriteFloat(myGGateSpawn.X);
                    res.WriteFloat(myGGateSpawn.Y);
                    res.WriteFloat(myGGateSpawn.Z);
                    res.WriteByte(myGGateSpawn.Heading);
                    res.WriteByte(0xA);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_point_move_notify, res, ServerType.Area);
                    Logger.Debug($"GGateSpawn {myGGateSpawn.InstanceId} has been adjusted by a height of {y}.");
                    break;
                case "add": //Adds a new entry to the database
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.Updated = DateTime.Now;
                    if (!Server.Database.InsertGGateSpawn(myGGateSpawn))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGGateSpawn could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Added GGateSpawn {myGGateSpawn.Id} to the database"));
                    }

                    break;
                case "update": //Updates an existing entry in the database
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    myGGateSpawn.Updated = DateTime.Now;
                    if (!Server.Database.UpdateGGateSpawn(myGGateSpawn))
                    {
                        responses.Add(ChatResponse.CommandError(client, "myGGateSpawn could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated GGateSpawn {myGGateSpawn.Id} in the database"));
                    }

                    break;
                case "remove": //removes a GGateSpawn from the database
                    myGGateSpawn = Server.Instances.GetInstance((uint) x) as GGateSpawn;
                    if (!Server.Database.DeleteGGateSpawn(myGGateSpawn.Id))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myGGateSpawn could not be deleted from database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Removed GGateSpawn {myGGateSpawn.Id} from the database"));
                    }

                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_object_disappear_notify, res, ServerType.Area);
                    break;
                case "state": //updates GGate State.
                    res.WriteUInt32(myGGateSpawn.InstanceId);
                    res.WriteInt32(y);
                    Router.Send(client.Map, (ushort) AreaPacketId.recv_npc_ggate_state_update_notify, res,
                        ServerType.Area);
                    break;
                default: //you don't know what you're doing do you?
                    Logger.Error($"There is no recv of type : {command[0]} ");
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"{command[0]} is not a valid GGateSpawn command."));
                }
                    break;
            }
        }

        public override AccountStateType AccountState => AccountStateType.Admin;
        public override string Key => "ggate";

        public override string HelpText =>
            "usage: `/GGateSpawn [argument] [number] [parameter]` - does something GGateSpawn related";
    }
}
