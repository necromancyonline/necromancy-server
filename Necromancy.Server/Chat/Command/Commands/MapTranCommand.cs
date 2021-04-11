using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// MapTranCommand related commands.
    /// </summary>
    public class MapTranCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(MapTranCommand));

        public MapTranCommand(NecServer server) : base(server)
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
                responses.Add(ChatResponse.CommandError(client,
                    $"Please provide a value.  Destination Map from Map.csv, or a table ID for updating"));
                return;
            }

            if (!int.TryParse(command[2], out int y))
            {
                responses.Add(ChatResponse.CommandError(client, $"Good Job!"));
            }

            MapTransition myMapTransition = new MapTransition();
            IBuffer res = BufferProvider.Provide();

            switch (command[0])
            {
                case "spawn"
                    : //spawns a new object on the map at your position.  makes a sign above it.  and jumps over it
                    myMapTransition = new MapTransition();
                    server.instances.AssignInstance(myMapTransition);

                    IBuffer res1 = BufferProvider.Provide(); // it's the aura portal for map
                    res1.WriteUInt32(myMapTransition.instanceId); // Unique ID
                    res1.WriteFloat(client.character.x); //x
                    res1.WriteFloat(client.character.y); //y
                    res1.WriteFloat(client.character.z + 2); //z
                    res1.WriteByte(client.character.heading); // offset
                    res1.WriteFloat(myMapTransition.maplinkOffset); // Height
                    res1.WriteFloat(myMapTransition.maplinkWidth); // Width
                    res1.WriteInt32(Util.GetRandomNumber(0,
                        6)); // Aura color 0=blue 1=gold 2=white 3=red 4=purple 5=black  0 to 5, crash above 5
                    router.Send(client.map, (ushort) AreaPacketId.recv_data_notify_maplink, res1, ServerType.Area);


                    myMapTransition.mapId = client.character.mapId;
                    myMapTransition.transitionMapId = x;
                    myMapTransition.referencePos.X = client.character.x;
                    myMapTransition.referencePos.Y = client.character.y;
                    myMapTransition.referencePos.Z = client.character.z;
                    myMapTransition.maplinkHeading = client.character.heading;
                    myMapTransition.maplinkColor =
                        0; //  Aura color 0=blue (safe, town) 1=gold (locked) 2=white (open) 3=red (occupied) 4=purple (special) 5=black (Boss Exit)  0 to 5, crash above 5
                    myMapTransition.maplinkOffset = 200;
                    myMapTransition.maplinkWidth = 1000;
                    myMapTransition.refDistance = 400;
                    myMapTransition.leftPos.X = client.character.x + 50;
                    myMapTransition.leftPos.Y = client.character.y + 50;
                    myMapTransition.leftPos.Z = client.character.z + 10;
                    myMapTransition.rightPos.X = client.character.x - 50;
                    myMapTransition.rightPos.Y = client.character.y - 50;
                    myMapTransition.rightPos.Z = client.character.z - 10;
                    myMapTransition.invertedTransition = false;
                    server.settingRepository.map.TryGetValue(x, out MapSetting targetMap);
                    myMapTransition.toPos.x = targetMap.x;
                    myMapTransition.toPos.y = targetMap.y;
                    myMapTransition.toPos.z = targetMap.z;
                    myMapTransition.toPos.heading = (byte) targetMap.orientation;
                    myMapTransition.state = true;
                    myMapTransition.created = DateTime.Now;
                    myMapTransition.updated = DateTime.Now;

                    myMapTransition.maplinkHeading = (byte) (client.character.heading + 90);
                    myMapTransition.maplinkHeading = (byte) (myMapTransition.maplinkHeading % 180);
                    if (myMapTransition.maplinkHeading < 0)
                    {
                        myMapTransition.maplinkHeading += 180;
                    }

                    responses.Add(ChatResponse.CommandError(client,
                        $"Spawned MapTransition {myMapTransition.instanceId}"));

                    if (command[2] == "add"
                    ) //if you want to send your MapTransition straight to the DB.  type Add at the end of the spawn command.
                    {
                        if (!server.database.InsertMapTransition(myMapTransition))
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                "myMapTransition could not be saved to database"));
                            return;
                        }
                        else
                        {
                            responses.Add(ChatResponse.CommandError(client,
                                $"Added MapTransition {myMapTransition.id} to the database"));
                        }
                    }

                    //so you dont map transition immediatly
                    client.character.mapChange = true;
                    Task.Delay(TimeSpan.FromSeconds(5)).ContinueWith
                    (t1 =>
                        {
                            myMapTransition.Start(server, client.map);
                            client.character.mapChange = false;
                        }
                    );


                    client.map.mapTransitions.Add(myMapTransition.instanceId, myMapTransition);


                    break;

                case "add": //Adds a new entry to the database
                    myMapTransition = server.instances.GetInstance((uint) x) as MapTransition;
                    myMapTransition.updated = DateTime.Now;
                    if (!server.database.InsertMapTransition(myMapTransition))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Added MapTransition {myMapTransition.id} to the database"));
                    }

                    break;
                case "update": //Updates an existing entry in the database

                    myMapTransition = server.instances.GetInstance((uint) x) as MapTransition;
                    //myMapTransition = Server.Database.SelectMapTransitionsById(x);
                    myMapTransition.updated = DateTime.Now;
                    if (!server.database.UpdateMapTransition(myMapTransition))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated MapTransition {myMapTransition.id} in the database"));
                    }

                    break;
                case "setdestination"
                    : //Updates the to Postion of a given maptrans row ID to the players current position on the destination map.

                    //myMapTransition = Server.Instances.GetInstance((uint)x) as MapTransition;
                    myMapTransition = server.database.SelectMapTransitionsById(x);
                    myMapTransition.toPos.x = client.character.x;
                    myMapTransition.toPos.y = client.character.y;
                    myMapTransition.toPos.z = client.character.z;
                    myMapTransition.toPos.heading = client.character.heading;
                    myMapTransition.transitionMapId = client.character.mapId;

                    myMapTransition.updated = DateTime.Now;
                    if (!server.database.UpdateMapTransition(myMapTransition))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated MapTransition {myMapTransition.id} in the database"));
                    }

                    break;
                case "setleft": //Updates the left position of the maptransition

                    //myMapTransition = Server.Instances.GetInstance((uint)x) as MapTransition;
                    myMapTransition = server.database.SelectMapTransitionsById(x);
                    myMapTransition.leftPos.X = client.character.x;
                    myMapTransition.leftPos.Y = client.character.y;
                    myMapTransition.leftPos.Z = client.character.z;

                    myMapTransition.updated = DateTime.Now;
                    if (!server.database.UpdateMapTransition(myMapTransition))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated MapTransition {myMapTransition.id} in the database"));
                    }

                    break;
                case "setright": //Updates the right position of the maptransition

                    //myMapTransition = Server.Instances.GetInstance((uint)x) as MapTransition;
                    myMapTransition = server.database.SelectMapTransitionsById(x);
                    myMapTransition.rightPos.X = client.character.x;
                    myMapTransition.rightPos.Y = client.character.y;
                    myMapTransition.rightPos.Z = client.character.z;

                    myMapTransition.updated = DateTime.Now;
                    if (!server.database.UpdateMapTransition(myMapTransition))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be saved to database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Updated MapTransition {myMapTransition.id} in the database"));
                    }

                    break;
                case "stop"
                    : //stops the maptrans task so you can modify the maptran.  actually this didnt work, so i just set your transition state to 'true' so it doesnt transition you

                    myMapTransition = server.instances.GetInstance((uint) x) as MapTransition;
                    //myMapTransition = Server.Database.SelectMapTransitionsById(x);
                    myMapTransition.Stop();
                    client.character.mapChange = true;

                    myMapTransition.updated = DateTime.Now;
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"Stoped {myMapTransition.instanceId}.  Safe to edit"));
                }
                    break;
                case "start": //starts the maptrans task so you can test the maptran

                    myMapTransition = server.instances.GetInstance((uint) x) as MapTransition;
                    //myMapTransition = Server.Database.SelectMapTransitionsById(x);
                    myMapTransition.Start(server, client.map);
                    client.character.mapChange = false;

                    myMapTransition.updated = DateTime.Now;
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"Started {myMapTransition.instanceId}.  Safe to Warp"));
                }
                    break;

                case "remove": //removes a MapTransition from the database
                    myMapTransition = server.instances.GetInstance((uint) x) as MapTransition;
                    if (!server.database.DeleteMapTransition(myMapTransition.id))
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            "myMapTransition could not be deleted from database"));
                        return;
                    }
                    else
                    {
                        responses.Add(ChatResponse.CommandError(client,
                            $"Removed MapTransition {myMapTransition.id} from the database"));
                    }

                    res.WriteUInt32(myMapTransition.instanceId);
                    router.Send(client.map, (ushort) AreaPacketId.recv_object_disappear_notify, res, ServerType.Area);
                    break;

                default: //you don't know what you're doing do you?
                    _Logger.Error($"There is no recv of type : {command[0]} ");
                {
                    responses.Add(ChatResponse.CommandError(client,
                        $"{command[0]} is not a valid MapTransition command."));
                }
                    break;
            }
        }

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "maptran";

        public override string helpText =>
            "usage: `/maptran [argument] [RowId] [parameter]` - spawns or updates a map transition";
    }
}


/*   MAP REFERENCE INFORMATION
 *
 *   #MapID,exterior,minimap,Region,Map,SubMap,X,Y,Z,direction,BGM-ID / Sword,During extraction,,Same area judgment,Switching order,Image ID,Number of map divisions
1001001,1001001,1001001,Kingdom of Dimento,Illfalo Port,Hero's Square,1608,-3958,-19,0,2,0,1,1,1,1001001,0
1001002,1001002,1001002,Kingdom of Dimento,Illfalo Port,Deep Sea Port,23162,-219,-7,260,2,0,1,1,2,1001002,0
1001003,1001003,1001003,Kingdom of Dimento,Illfalo Port,Shady Path,3719,-5395,-4,0,41,0,1,1,3,1001003,0
1001004,1001004,1001004,Kingdom of Dimento,Illfalo Port,Bustling Market,1,1,1,0,2,0,1,1,4,1001004,0
1001005,1001005,1001005,Kingdom of Dimento,Illfalo Port,Avenue Where the Stalwart and the Wise Talk of Dreams,1,1,1,0,2,0,1,0,1,1001001,0
1001006,1001006,1001006,Kingdom of Dimento,Illfalo Port,Sunset Hub,76,2300,0,0,2,0,1,0,1,1001001,0
1001007,1001007,1001007,Kingdom of Dimento,Illfalo Port,Twilight Alley ,-5516,-3896,2,352,41,0,1,1,5,1001007,0
1001008,1001008,1001008,Kingdom of Dimento,Illfalo Port,Street Where the Holy Knight Pledged His Royalty,1,1,1,0,2,0,1,0,1,1001001,0
1001009,1001009,1001009,Kingdom of Dimento,Illfalo Ministry of Arcanum,Discovery Hall,-410,-859,68,180,9,0,1,1,6,1001009,0
1001091,1001091,1001091,,,,0,0,0,0,0,0,0,0,1,,0
1001901,1001901,1001901,Create Character,,,-163,4845,323,136,0,0,0,0,1,,0
1001902,1001902,1001002,Kingdom of Dimento,Illfalo Port,Deep Sea Port,23021,-224,0,260,2,0,1,0,1,1001002,0
1001010,1001010,1001010,Union Room,Union Room,Connection Square,0,200,0,0,49,0,0,0,1,,1
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
2001001,2001001,2001001,Kingdom of Dimento,Old Sewers,Basement 1,5029,439,119,180,29,0,0,101,1,2001001,3
2001002,2001002,2001002,Kingdom of Dimento,Old Sewers,Path to Basement 2,-14,-2785,0,0,29,0,0,101,2,2001001,1
2001003,2001003,2001003,Kingdom of Dimento,Old Sewers,Basement 2,1369,-1602,0,90,33,0,0,101,3,2001001,3
2001004,2001004,2001004,Kingdom of Dimento,Old Sewers,Path to Basement 3,0,0,30,0,33,0,0,101,4,2001001,1
2001005,2001005,2001005,Kingdom of Dimento,Old Sewers,Basement 3,2198,-2442,392,180,35,0,0,101,5,2001001,4
2001006,2001006,2001006,Kingdom of Dimento,Descension Ruins,The Corruption Square,0,0,30,0,45,0,0,103,1,2001006,5
2001007,2001007,2001007,Kingdom of Dimento,Descension Ruins,Blood-Soaked Hall,1600,0,30,90,45,0,0,103,2,2001007,1
,,,,,,,,,,,,,,,,
2001011,2001011,2001011,Kingdom of Dimento,Old Sewers,The Hall of Twisted Life,-14,73,7,180,13,0,0,0,1,2001001,1
2001012,2001012,2001012,Entering a sealed chamber . . .,Final Battle,Conclusion Hall,790,3,0,90,16,0,0,0,1,2001001,1
2001013,2001013,2001013,Entering a sealed chamber . . .,Hidden Final Battle,Survivor's Hall,790,3,0,90,16,0,0,0,1,2001001,1
2001014,2001014,2001014,Guardian's Astral Discipline Area,Trial of Fantasy,Amateur Trials,2700,1500,30,180,1,0,0,501,1,2002015,1
,,,,,,,,,,,,,,,,
2001021,2001021,2001021,Kingdom of Dimento,Illfalo Training Grounds,Movement Training,-178,6395,2004,90,11,0,0,0,1,2001001,1
2001022,2001022,2001022,Kingdom of Dimento,Illfalo Training Grounds,Battle Training,-4809,3155,2,180,11,0,0,0,1,2001001,1
2001023,2001023,2001023,Kingdom of Dimento,Illfalo Training Grounds,Avenue Where the Stalwart and the Wise Talk of Dreams,0,0,30,180,11,0,0,0,1,2001001,1
2001024,2001024,2001024,Kingdom of Dimento,Illfalo Training Grounds,Discovery Hall,0,0,30,90,11,0,0,0,1,2001001,1
2001025,2001025,2001025,Kingdom of Dimento,Illfalo Training Grounds,Trial Area,0,0,30,270,11,0,0,0,1,2001001,1
,,,,,,,,,,,,,,,,
2001031,2001031,2001031,Kingdom of Dimento,Execution Area,Basement 1,0,0,30,0,11,0,0,0,1,2001001,1
,,,,,,,,,,,,,,,,
2001051,2001051,2001051,Kingdom of Dimento,Azarm Trial Grounds,Basement 1,0,0,30,270,44,0,0,104,1,2001001,1
2001052,2001052,2001052,Kingdom of Dimento,Azarm Trial Grounds,Basement 2,0,0,30,270,44,0,0,104,2,2001001,1
2001053,2001053,2001053,Kingdom of Dimento,Azarm Trial Grounds,Basement 3,0,0,30,0,44,0,0,104,3,2001001,1
,,,,,,,,,,,,,,,,
2001101,2001101,2001101,Kingdom of Dimento,Caligrase Sewers,Adventurer's Entrance ,5029,439,119,180,28,0,0,0,1,2001101,2
2001103,2001103,2001103,Kingdom of Dimento,Deltis Keep,Planned Treasury Site,1369,-1602,0,90,29,0,0,0,1,2001103,2
2001105,2001105,2001105,Kingdom of Dimento,Aria Reservoir,Reservoir,2200,-2635,384,180,33,0,0,102,1,2001105,3
2001106,2001106,2001106,Kingdom of Dimento,Aria Reservoir,Core Area,0,0,30,0,33,0,0,102,2,2001105,3
,,,,,,,,,,,,,,,,
2001151,2001151,2001151,Kingdom of Dimento,Azarm Trial Grounds,Negligent Architect's Pride,0,0,30,270,44,0,0,0,1,2001151,1
,,,,,,,,,,,,,,,,
2001901,2001901,2001901,Title,,,0,0,0,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
2001991,2001991,2001991,Kingdom of Dimento,Abyss Labyrinth,Basement 1,790,3,0,90,11,0,0,0,1,2001001,1
,,,,,,,,,,,,,,,,
2001091,2001091,2001091,Test Map Alpha,,,-1600,-3200,30,0,0,0,0,0,1,,1
2001092,2001092,2001092,Test Map A,,,0,0,30,0,0,0,0,0,1,,1
2001093,2001093,2001093,Test Map B,,,1600,3200,30,0,0,0,0,0,1,,1
2001094,2001094,2001094,Test Map C,,,0,-200,-400,0,0,0,0,0,1,,1
2001095,2001095,2001095,Test Map D,,,0,0,30,0,0,0,0,0,1,,1
2001096,2001096,2001096,Test Map E,,,0,0,30,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
2002001,2002001,2002001,Kingdom of Dimento,Sangent Ruins,Traveler's Decline Gate,8427,9918,178,180,12,0,0,201,1,2002001,3
2002002,2002002,2002002,Kingdom of Dimento,Sangent Ruins,Ruined Circuit,445,-5131,1,0,12,0,0,201,2,2002001,3
2002003,2002003,2002003,Kingdom of Dimento,Sangent Ruins,Forgotten Path,442,6327,6,180,12,0,0,201,3,2002001,3
2002004,2002004,2002004,Kingdom of Dimento,Sangent Ruins,Lake Temple,-900,6679,-414,180,30,0,0,201,4,2002001,1
2002005,2002005,2002005,Kingdom of Dimento,Sangent Ruins,Canal Ruins,21266,-18815,-2,90,30,0,0,201,5,2002001,1
2002006,2002006,2002006,Kingdom of Dimento,Sangent Ruins,Dead Aqueduct,2375,-3250,-2,180,32,0,0,201,6,2002001,1
2002007,2002007,2002007,Kingdom of Dimento,Sangent Ruins,Silent Temple,-1133,12485,657,180,32,0,0,201,7,2002001,1
,,,,,,,,,,,,,,,,
2002011,2002011,2002011,Kingdom of Dimento,Sangent Ruins,Fighting Ring,7971,8308,2,180,14,0,0,0,1,2002001,1
2002012,2002012,2002012,Kingdom of Dimento,Final Battle,Conclusion Hall,-400,500,10,0,16,0,0,0,1,2002001,1
2002013,2002013,2002013,Altar,,,-400,500,10,0,11,0,0,0,1,2002001,1
2002014,2002014,2002014,Entering a sealed chamber . . .,Hidden Final Battle,Survivor's Hall,-400,500,10,0,16,0,0,0,1,2002001,1
2002015,2002015,2002015,Guardian's Astral Discipline Area,Trial of Fantasy,Beginner Trials,3600,-6600,20,0,1,0,0,501,1,2002015,1
2002016,2002016,2002016,Entering a sealed chamber . . .,Final Battle,The Realm of Illusion,-400,500,10,0,16,0,0,0,1,2002001,1
,,,,,,,,,,,,,,,,
2002091,2002091,2002091,Ruins Footstep Investigation Map,,,0,0,30,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
2002101,2002101,2002101,Kingdom of Dimento,Golden Dragon Ruins,Bol Naya Street,8427,9918,178,180,30,0,0,0,1,2002101,2
2002102,2002102,2002102,Kingdom of Dimento,Golden Dragon Ruins,Path of a Thousand Cuts,445,-5131,1,0,32,0,0,202,1,2002102,1
2002103,2002103,2002103,Kingdom of Dimento,Golden Dragon Ruins,Fir Elin's Chamber,442,6327,6,180,32,0,0,202,2,2002102,1
2002104,2002104,2002104,Kingdom of Dimento,Roswald Deep Fort,Rusted Great Gate,-900,6679,-414,180,46,0,0,203,1,2002104,1
2002105,2002105,2002105,Kingdom of Dimento,Roswald Deep Fort,Isolated Hall,-5465,-6000,0,270,46,0,0,203,2,2002104,1
2002106,2002106,2002106,Kingdom of Dimento,Roswald Deep Fort,Severed Corridor,23249,-25039,2,0,46,0,0,203,3,2002104,1
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
2003001,2003001,2003001,Kingdom of Dimento,Papylium Hanging Gardens,Basement 1,2785,-10168,1263,0,34,0,0,301,1,2003001,1
2003002,2003002,2003002,Kingdom of Dimento,Papylium Hanging Gardens,Basement 2,-3990,-10360,996,0,37,0,0,301,2,2003001,1
2003003,2003003,2003003,Kingdom of Dimento,Papylium Hanging Gardens,Basement 3,-4393,-2282,800,0,37,0,0,301,3,2003001,1
2003004,2003004,2003004,Kingdom of Dimento,Papylium Hanging Gardens,Basement 4,400,-7430,-55,0,34,0,0,301,4,2003001,1
2003005,2003005,2003005,Kingdom of Dimento,Papylium Hanging Gardens,Basement 5,400,-10340,1800,0,34,0,0,301,5,2003001,1
2003006,2003006,2003006,Kingdom of Dimento,Papylium Hanging Gardens,Basement 6,400,-7290,-55,0,37,0,0,301,6,2003001,1
2003007,2003007,2003007,Kingdom of Dimento,Papylium Hanging Gardens,Basement 7,0,-8600,-55,0,37,0,0,301,7,2003001,1
,,,,,,,,,,,,,,,,
2003011,2003011,2003011,Entering a sealed chamber . . .,Final Battle,Conclusion Hall,-1195,-4513,-176,0,16,0,0,0,1,2003001,1
2003012,2003012,2003012,Entering a sealed chamber . . .,Final Battle,Conclusion Hall,0,-200,-50,0,16,0,0,0,1,2003001,1
2003013,2003013,2003013,Entering a sealed chamber . . .,Hidden Final Battle,Survivor's Hall,0,-200,-50,0,16,0,0,0,1,2003001,1
2003014,2003014,2003014,Addition (Unused),,,1600,-1300,-30,0,16,0,0,0,1,2002015,1
,,,,,,,,,,,,,,,,
2003051,2003051,2003051,DUM SPIRO SPERO,,,0,-2400,30,0,11,0,0,0,1,2003051,1
2003052,2003052,2003052,DUM SPIRO SPERO,Flux Tower,Dawn,1450,-2400,30,0,39,0,0,0,1,2003051,1
2003053,2003053,2003053,DUM SPIRO SPERO,Flux Tower,Cry of Crazybones,0,-2400,30,0,16,0,0,0,1,2003051,1
2003054,2003054,2003052,DUM SPIRO SPERO,Flux Tower,Trickling Haze,1600,9750,30,0,39,0,0,0,1,2003051,1
2003055,2003055,2003052,DUM SPIRO SPERO,Flux Tower,T.B.B,-16000,50,30,0,39,0,0,0,1,2003051,1
2003056,2003056,2003052,DUM SPIRO SPERO,Flux Tower,DECAYED CROW,-25450,-1600,30,0,39,0,0,0,1,2003051,1
2003057,2003057,2003052,DUM SPIRO SPERO,Flux Tower, Nesting-,-34250,6400,30,0,39,0,0,0,1,2003051,1
,,,,,,,,,,,,,,,,
2003091,2003091,2003091,Ancient Labyrinth Test Map A,,,-2076,-948,1,0,0,0,0,0,1,,1
2003092,2003092,2003092,Ancient Labyrinth Test Map B,,,-2076,-948,1,0,11,0,0,0,1,2003001,1
,,,,,,,,,,,,,,,,
2003101,2003101,2003101,Kingdom of Dimento,Chikor Castle Site,Citadel Interior,2785,-10168,1263,0,34,0,0,0,1,2003001,4
2003102,2003102,2003102,Kingdom of Dimento,Temple of Oblivion,Shrine of the Soul,-3990,-10360,996,0,37,0,0,302,1,2003102,3
2003103,2003103,2003103,Kingdom of Dimento,Temple of Oblivion,Palace of Mentor,5914,1391,-3,270,37,0,0,302,2,2003102,3
2003104,2003104,2003104,Kingdom of Dimento,Ruined Chamber,Rotting Dungeon,400,-7430,-55,0,50,0,0,303,1,2003104,5
2003105,2003105,2003105,Kingdom of Dimento,Ruined Chamber,Vulgar Spread,400,-10340,1800,0,50,0,0,303,2,2003104,5
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
2004001,2004001,2004001,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 1,-15000,3500,50,0,35,0,0,401,1,2004001,1
2004002,2004002,2004002,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 2,7800,10440,15,0,35,0,0,401,2,2004001,1
2004003,2004003,2004003,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 3,6000,-9300,15,0,35,0,0,401,3,2004001,1
2004004,2004004,2004004,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 4,-6800,8440,815,0,35,0,0,401,4,2004001,1
2004005,2004005,2004005,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 5,-1000,6400,15,0,35,0,0,401,5,2004001,1
2004006,2004006,2004006,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 6,-7600,12000,50,0,35,0,0,401,6,2004001,1
2004007,2004007,2004007,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 8,-6800,-2400,1650,0,35,0,0,401,7,2004001,1
2004008,2004008,2004008,Kingdom of Dimento,The Labyrinth of Apocrypha,Basement 9,-4200,0,50,0,35,0,0,401,8,2004001,1
,,,,,,,,,,,,,,,,
2004011,2004011,2004011,Entering a sealed chamber . . .,Final Battle,Conclusion Hall,-7600,12000,50,0,14,0,0,0,1,2004001,1
2004012,2004012,2004012,Entering a sealed chamber . . .,Final Battle,Conclusion Hall,0,-200,1,0,16,0,0,0,1,2004001,1
2004013,2004013,2004012,Kingdom of Dimento,Depths of the Underground Dragoon Ruins,Deep Trembles,0,-200,1,0,13,0,0,0,1,2004001,1
2004014,2004014,2004014,Entering a sealed chamber . . .,Hidden Final Battle,Survivor's Hall,0,-200,1,0,13,0,0,0,1,2004001,1
2004015,2004015,2004015,Guardian's Astral Discipline Area,Trial of Fantasy,Deep Trembles,1600,-3600,30,0,1,0,0,501,1,2002015,1
,,,,,The Realm of Illusion,,,,,,,,,,,
2004091,2004091,2004091,Ancient Labyrinth Depths Test Map,,,-2076,-948,1,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
2004101,2004101,2004101,Kingdom of Dimento,Underground Dragoon Ruins,Warped Heaven,-5204,-6125,-4,0,47,0,0,402,1,2004001,1
2004102,2004102,2004102,Kingdom of Dimento,Underground Dragoon Ruins,Flawed Palace of Fear,-4603,-578,-797,90,47,0,0,402,2,2004001,1
2004103,2004103,2004103,Kingdom of Dimento,Dark Roundtable,Teased Diversion,6000,-8688,0,360,35,0,0,403,1,2004103,4
2004104,2004104,2004104,Kingdom of Dimento,Dark Roundtable,Wicked Congress,12691,-6605,-793,90,35,0,0,403,2,2004103,4
2004105,2004105,2004105,Kingdom of Dimento,Dark Roundtable,Arrogant Ogre's Rest,-1000,6400,15,0,35,0,0,403,3,2004103,1
2004106,2004106,2004106,Kingdom of Dimento,Facility 13,Test Site,-1559,-1384,1,180,52,0,0,404,1,2004106,1
2004107,2004107,2004107,Kingdom of Dimento,Facility 13,Control Area,-6800,-2400,1650,0,52,0,0,404,2,2004106,1
,,,,,,,,,,,,,,,,
2006000,2006000,2006000,House Map,,,0,0,30,270,11,0,0,,,2001151,1
,,,,,,,,,,,,,,,2001151,
,,,,,,,,,,,,,,,,
2007000,2007000,2007000,Cave Map,,,0,0,30,270,11,0,0,,,2001151,1
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
2000001,2000001,2000001,Test Map 001,,,1,1,1,0,0,0,0,0,1,,1
2000002,2000002,2000002,Gimmick Test Map,,,1,1,1,0,0,0,0,0,1,,1
2000003,2000003,2000003,Character Test Map,,,1,1,1,0,0,0,0,0,1,,1
2000004,2000004,2000004,NPC Test Map,,,1,1,1,0,0,0,0,0,1,,1
2000005,2000005,2000005,NPC Test Map 2,,,1,1,1,0,2,0,0,0,1,,1
2999999,2999999,2999999,For Viewer,,,0,0,0,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
3001001,3001001,3001001,Character Select Screen,,,0,0,0,0,0,0,0,0,1,,1
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,
9001102,9001102,9001102,Mock Start Dungeon B2,,,1,1,1,0,0,0,0,,,,1
9001103,9001103,9001103,Mock Start Dungeon B3,,,1,1,1,0,0,0,0,,,,1
9001104,9001104,9001104,Mock Start Dungeon B4,,,1,1,1,0,0,0,0,,,,1
9001105,9001105,9001105,Mock Start Dungeon B5,,,1,1,1,0,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9001201,9001201,9001201,Mock Tutorial Basics,,,1800,-27000,26,0,0,0,0,,,,1
9001202,9001202,9001202,Mock Tutorial Fighting,,,1800,-27200,26,0,0,0,0,,,,1
9001203,9001203,9001203,Mock Tutorial Group Basics,,,1800,-28200,26,0,0,0,0,,,,1
9001204,9001204,9001204,Mock Tutorial Group Fighting,,,200,-36200,226,0,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9002101,9002101,9002101,Mock Level 2 Dungeon B1,,,1,1,1,0,0,0,0,,,,1
9002102,9002102,9002102,Mock Level 2 Dungeon B2,,,1,1,1,0,0,0,0,,,,1
9002103,9002103,9002103,Mock Level 2 Dungeon B3,,,1,1,1,0,0,0,0,,,,1
9002104,9002104,9002104,Mock Level 2 Dungeon B4,,,1,1,1,0,0,0,0,,,,1
9002105,9002105,9002105,Mock Level 2 Dungeon B5,,,1,1,1,0,0,0,0,,,,1
9002106,9002106,9002106,Mock Level 2 Dungeon B6,,,1,1,1,0,0,0,0,,,,1
9002107,9002107,9002107,Mock Level 2 Dungeon B7,,,1,1,1,0,0,0,0,,,,1
9002108,9002108,9002108,Mock Level 2 Dungeon B8,,,1,1,1,0,0,0,0,,,,1
9002109,9002109,9002109,Mock Level 2 Dungeon B9,,,1,1,1,0,0,0,0,,,,1
9002110,9002110,9002110,Mock Level 2 Dungeon B10,,,1,1,1,0,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9005101,9005101,9005101,Mock Level 5 Dungeon B1,,,1,1,1,0,0,0,0,,,,1
9005102,9005102,9005102,Mock Level 5 Dungeon B2,,,1,1,1,0,0,0,0,,,,1
9005103,9005103,9005103,Mock Level 5 Dungeon B3,,,1,1,1,0,0,0,0,,,,1
9005104,9005104,9005104,Mock Level 5 Dungeon B4,,,1,1,1,0,0,0,0,,,,1
9005105,9005105,9005105,Mock Level 5 Dungeon B5,,,1,1,1,0,0,0,0,,,,1
9005106,9005106,9005106,Mock Level 5 Dungeon B6,,,1,1,1,0,0,0,0,,,,1
9005107,9005107,9005107,Mock Level 5 Dungeon B7,,,1,1,1,0,0,0,0,,,,1
9005108,9005108,9005108,Mock Level 5 Dungeon B8,,,1,1,1,0,0,0,0,,,,1
9005109,9005109,9005109,Mock Level 5 Dungeon B9,,,1,1,1,0,0,0,0,,,,1
9005110,9005110,9005110,Mock Level 5 Dungeon B10,,,1,1,1,0,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9006101,9006101,9006101,Mock Start Dungeon Returns B1,,,-1800,-44600,0,0,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9007101,9007101,9007101,Mock Leading Dungeon,,,6600,-24000,0,180,0,0,0,,,,1
,,,,,,,,,,,,,,,,
9009001,9009001,2001151,Kingdom of Dimento,Azarm Trial Grounds,Negligent Architect's Pride,0,0,30,270,11,0,0,0,1,2001151,1
9009002,9009002,9009002,Cave Map Mock B1F_A,,,0,0,30,270,11,0,0,,,2001151,1
9009003,9009003,9009003,Cave Map Mock B1F_B,,,0,0,30,270,11,0,0,,,2001151,1
9009004,9009004,9009004,Cave Map Mock Stairwell,,,0,0,30,270,11,0,0,,,2001151,1
9009005,9009005,9009005,Cave Map Mock B2F_A,,,0,0,30,270,11,0,0,,,2001151,1
9009006,9009006,9009006,Cave Map Mock B2F_B,,,0,0,30,270,11,0,0,,,2001151,1
9009007,9009007,9009007,Cave Map Mock B3F_A,,,0,0,30,270,11,0,0,,,2001151,1
9009008,9009008,9009008,Cave Map Mock B3F_B,,,0,0,30,270,11,0,0,,,2001151,1
9009009,9009009,9009009,Cave Map Mock B4F_A,,,0,0,30,270,11,0,0,,,2001151,1
9009010,9009010,9009010,Cave Map Mock B4F_B,,,0,0,30,270,11,0,0,,,2001151,1
9009011,9009011,9009011,Cave Map Mock,,,0,0,30,270,11,0,0,,,2001151,1


    */
