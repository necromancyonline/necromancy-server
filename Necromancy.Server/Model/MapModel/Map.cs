using System;
using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class Map

    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(Map));

        private readonly object TrapLock = new object();

        public const int NewCharacterMapId = 1001002; //2006000

        private readonly NecServer _server;
        public int Id { get; set; }
        public float X { get; }
        public float Y { get; }
        public float Z { get; }
        public string Country { get; set; }
        public string Area { get; set; }
        public string Place { get; set; }
        public byte Orientation { get; }
        public string FullName => $"{Country}/{Area}/{Place}";
        public ClientLookup ClientLookup { get; }

        public Dictionary<uint, NpcSpawn> NpcSpawns { get; }

        // public Dictionary<int, TrapTransition> Trap { get; }
        public Dictionary<uint, MonsterSpawn> MonsterSpawns { get; }
        public Dictionary<uint, Gimmick> GimmickSpawns { get; }
        public Dictionary<uint, MapTransition> MapTransitions { get; }
        public Dictionary<uint, TrapStack> Traps { get; }
        public Dictionary<uint, DeadBody> DeadBodies { get; }
        public Dictionary<uint, GGateSpawn> GGateSpawns { get; }


        public Map(MapData mapData, NecServer server)
        {
            _server = server;
            ClientLookup = new ClientLookup();
            NpcSpawns = new Dictionary<uint, NpcSpawn>();
            MonsterSpawns = new Dictionary<uint, MonsterSpawn>();
            GimmickSpawns = new Dictionary<uint, Gimmick>();
            MapTransitions = new Dictionary<uint, MapTransition>();
            GGateSpawns = new Dictionary<uint, GGateSpawn>();
            DeadBodies = new Dictionary<uint, DeadBody>();


            Id = mapData.Id;
            X = mapData.X;
            Y = mapData.Y;
            Z = mapData.Z;
            Country = mapData.Country;
            Area = mapData.Area;
            Place = mapData.Place;
            Orientation = (byte) (mapData.Orientation / 2); // Client uses 180 degree orientation
            Traps = new Dictionary<uint, TrapStack>();

            List<MapTransition> mapTransitions = server.Database.SelectMapTransitionsByMapId(mapData.Id);
            foreach (MapTransition mapTran in mapTransitions)
            {
                
                if(mapTran.Id == 0) //Only one special transition
                {
                    double angle = mapTran.MaplinkHeading / 255.0;
                    mapTran.LeftPos.X = (float)((mapTran.ReferencePos.X + mapTran.MaplinkWidth / 2) * Math.Cos(angle));
                    mapTran.LeftPos.Y = (float)((mapTran.ReferencePos.Y) * Math.Cos(angle));
                    mapTran.RightPos.X = (float)((mapTran.ReferencePos.X - mapTran.MaplinkWidth / 2) * Math.Cos(angle));
                    mapTran.RightPos.Y = (float)(mapTran.ReferencePos.Y + mapTran.MaplinkWidth / 2);
                }
                else if (mapTran.InvertedTransition != true) //map is x dominant
                {
                    mapTran.LeftPos.X = (float)(mapTran.ReferencePos.X + mapTran.MaplinkWidth / 2);
                    mapTran.LeftPos.Y = (float)(mapTran.ReferencePos.Y);
                    mapTran.RightPos.X = (float)(mapTran.ReferencePos.X - mapTran.MaplinkWidth / 2);
                    mapTran.RightPos.Y = (float)(mapTran.ReferencePos.Y);
                }
                else if(mapTran.InvertedTransition != false) //map is y dominant
                {
                    mapTran.LeftPos.X = (float)(mapTran.ReferencePos.X);
                    mapTran.LeftPos.Y = (float)(mapTran.ReferencePos.Y + mapTran.MaplinkWidth / 2);
                    mapTran.RightPos.X = (float)(mapTran.ReferencePos.X);
                    mapTran.RightPos.Y = (float)(mapTran.ReferencePos.Y - mapTran.MaplinkWidth / 2);
                }

                server.Instances.AssignInstance(mapTran);
                MapTransitions.Add(mapTran.InstanceId, mapTran);
                Logger.Debug($"Loaded Map transition {mapTran.Id} on map {this.FullName}");
            }

            //Assign Unique Instance ID to each NPC per map. Add to dictionary stored with the Map object
            List<NpcSpawn> npcSpawns = server.Database.SelectNpcSpawnsByMapId(mapData.Id);
            foreach (NpcSpawn npcSpawn in npcSpawns)
            {
                server.Instances.AssignInstance(npcSpawn);
                NpcSpawns.Add(npcSpawn.InstanceId, npcSpawn);
            }

            //Assign Unique Instance ID to each Gimmick per map. Add to dictionary stored with the Map object
            List<Gimmick> gimmickSpawns = server.Database.SelectGimmicksByMapId(mapData.Id);
            foreach (Gimmick gimmickSpawn in gimmickSpawns)
            {
                server.Instances.AssignInstance(gimmickSpawn);
                GimmickSpawns.Add(gimmickSpawn.InstanceId, gimmickSpawn);
            }

            List<GGateSpawn> gGateSpawns = server.Database.SelectGGateSpawnsByMapId(mapData.Id);
            foreach (GGateSpawn gGateSpawn in gGateSpawns)
            {
                server.Instances.AssignInstance(gGateSpawn);
                GGateSpawns.Add(gGateSpawn.InstanceId, gGateSpawn);
            }

            //To-Do   | for each deadBody in Deadbodies {RecvDataNotifyCharabodyData} 

            List<MonsterSpawn> monsterSpawns = server.Database.SelectMonsterSpawnsByMapId(mapData.Id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                server.Instances.AssignInstance(monsterSpawn);
                if (!_server.SettingRepository.ModelCommon.TryGetValue(monsterSpawn.ModelId,
                    out ModelCommonSetting modelSetting))
                {
                    Logger.Error($"Error getting ModelCommonSetting for ModelId {monsterSpawn.ModelId}");
                    continue;
                }

                if (!_server.SettingRepository.Monster.TryGetValue(monsterSpawn.MonsterId,
                    out MonsterSetting monsterSetting))
                {
                    Logger.Error($"Error getting MonsterSetting for MonsterId {monsterSpawn.MonsterId}");
                    continue;
                }

                monsterSpawn.ModelId = modelSetting.Id;
                //monsterSpawn.Size = (short) (modelSetting.Height / 2);   //commenting out to use size setting from database.
                monsterSpawn.Radius = (short)modelSetting.Radius;
                monsterSpawn.Hp.setMax(300);
                monsterSpawn.Hp.setCurrent(300);
                monsterSpawn.AttackSkillId = monsterSetting.AttackSkillId;
                //monsterSpawn.Level = (byte) monsterSetting.Level;
                monsterSpawn.CombatMode = monsterSetting.CombatMode;
                monsterSpawn.CatalogId = monsterSetting.CatalogId;
                monsterSpawn.TextureType = monsterSetting.TextureType;
                monsterSpawn.Map = this;
                MonsterSpawns.Add(monsterSpawn.InstanceId, monsterSpawn);

                List<MonsterCoord> coords = server.Database.SelectMonsterCoordsByMonsterId(monsterSpawn.Id);
                if (coords.Count > 0)
                {
                    monsterSpawn.defaultCoords = false;
                    monsterSpawn.monsterCoords.Clear();
                    foreach (MonsterCoord monsterCoord in coords)
                    {
                        //Console.WriteLine($"added coord {monsterCoord} to monster {monsterSpawn.InstanceId}");
                        monsterSpawn.monsterCoords.Add(monsterCoord);
                    }
                }
                else
                {
                    //home coordinate set to monster X,Y,Z from database
                    Vector3 homeVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y, monsterSpawn.Z);
                    MonsterCoord homeCoord = new MonsterCoord();
                    homeCoord.Id = monsterSpawn.Id;
                    homeCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    homeCoord.MapId = (uint)monsterSpawn.MapId;
                    homeCoord.CoordIdx = 0;
                    homeCoord.destination = homeVector3;
                    monsterSpawn.monsterCoords.Add(homeCoord);

                    //default path part 2
                    Vector3 defaultVector3 = new Vector3(monsterSpawn.X, monsterSpawn.Y + Util.GetRandomNumber(50, 150),
                        monsterSpawn.Z);
                    MonsterCoord defaultCoord = new MonsterCoord();
                    defaultCoord.Id = monsterSpawn.Id;
                    defaultCoord.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord.CoordIdx = 1;
                    defaultCoord.destination = defaultVector3;

                    monsterSpawn.monsterCoords.Add(defaultCoord);

                    //default path part 3
                    Vector3 defaultVector32 = new Vector3(monsterSpawn.X + Util.GetRandomNumber(50, 150),
                        monsterSpawn.Y + Util.GetRandomNumber(50, 150), monsterSpawn.Z);
                    MonsterCoord defaultCoord2 = new MonsterCoord();
                    defaultCoord2.Id = monsterSpawn.Id;
                    defaultCoord2.MonsterId = (uint)monsterSpawn.MonsterId;
                    defaultCoord2.MapId = (uint)monsterSpawn.MapId;
                    defaultCoord2.CoordIdx = 2; //64 is currently the Idx of monsterHome on send_map_get_info.cs
                    defaultCoord2.destination = defaultVector32;

                    monsterSpawn.monsterCoords.Add(defaultCoord2);
                }
            }
        }

        public void EnterForce(NecClient client, MapPosition mapPosition = null)
        {
            client.Character.mapChange = true;
            Enter(client, mapPosition);
            _server.Router.Send(new RecvMapChangeForce(this, mapPosition, _server.Setting, client), client);

            // currently required to prevent disconnect by force changing
            _server.Router.Send(new RecvMapChangeSyncOk(), client);
        }

        public void Enter(NecClient client, MapPosition mapPosition = null)
        {
            if (client.Map != null)
            {
                client.Map.Leave(client);
            }
            client.Map = this;

            Logger.Info(client, $"Entering Map: {Id}:{FullName}");
            // If position is passed in use it and set character position, if null then use map default coords
            // If this isn't set here, the wrong coords are in character until send_movement_info updates it. 
            if (mapPosition != null)
            {
                client.Character.X = mapPosition.X;
                client.Character.Y = mapPosition.Y;
                client.Character.Z = mapPosition.Z;
                client.Character.Heading = mapPosition.Heading;
            }
            //set character coords to default map entry coords If arriving form another map.
            else if (client.Character.MapId != Id)
            {
                client.Character.X = this.X;
                client.Character.Y = this.Y;
                client.Character.Z = this.Z;
                client.Character.Heading = this.Orientation;
            }

            client.Character.MapId = Id;
            client.Character.mapChange = false;
            ClientLookup.Add(client);
            Logger.Debug($"Client Lookup count is now : {ClientLookup.GetAll().Count}  for map  {this.Id} ");
            Logger.Debug($"Character State for character {client.Character.Name} is {client.Character.State}");
            RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            _server.Router.Send(this, myCharacterData, client);
            if (client.Union != null)
            {
                RecvDataNotifyUnionData myUnionData = new RecvDataNotifyUnionData(client.Character, client.Union.Name);
                _server.Router.Send(this, myUnionData, client);
            }

            foreach (MonsterSpawn monsterSpawn in this.MonsterSpawns.Values)
            {
                if (monsterSpawn.Active == true)
                {
                    monsterSpawn.SpawnActive = true;
                    if (!monsterSpawn.TaskActive)
                    {
                        MonsterTask monsterTask = new MonsterTask(_server, monsterSpawn);
                        if (monsterSpawn.defaultCoords)
                            monsterTask.monsterHome = monsterSpawn.monsterCoords[0];
                        else
                            monsterTask.monsterHome = monsterSpawn.monsterCoords.Find(x => x.CoordIdx == 64);
                        monsterTask.Start();
                    }
                    else
                    {
                        if (monsterSpawn.MonsterVisible)
                        {
                            Logger.Debug($"MonsterTask already running for [{monsterSpawn.Name}]");
                            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                            _server.Router.Send(monsterData, client);
                            if (!monsterSpawn.GetAgro())
                            {
                                monsterSpawn.MonsterMove(_server, client, monsterSpawn.MonsterWalkVelocity, (byte) 2,
                                    (byte) 0);
                            }
                        }
                    }
                }
            }

            //on successful map entry, update the client database position
            if (!_server.Database.UpdateCharacter(client.Character))
            {
                Logger.Error("Could not update the database with current known player position");
            }
            if (!_server.Database.UpdateSoul(client.Soul))
            {
                Logger.Error("Could not update the database with soul details ");
            }
        }

        public void Leave(NecClient client)
        {
            Logger.Info(client, $"Leaving Map: {Id}:{FullName}");
            ClientLookup.Remove(client);
            if (!_server.Database.UpdateCharacter(client.Character))
            {
                Logger.Error("Could not update the database with last known player position");
            }
            if (!_server.Database.UpdateSoul(client.Soul))
            {
                Logger.Error("Could not update the database with soul details ");
            }
 

            client.Map = null;

            RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(client.Character.InstanceId);
            _server.Router.Send(this, objectDisappearData, client);
            if (ClientLookup.GetAll().Count == 0)
            {
                foreach (MonsterSpawn monsterSpawn in this.MonsterSpawns.Values)
                {
                    monsterSpawn.SpawnActive = false;
                }
            }

            Logger.Debug($"Client Lookup count is now : {ClientLookup.GetAll().Count}  for map  {this.Id} ");
        }

        public bool MonsterInRange(Vector3 position, int range)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                if (Vector3.Distance(position, monsterPos) <= range)
                {
                    return true;
                }
            }

            return false;
        }

        public MonsterSpawn GetMonsterByInstanceId(uint instanceId)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                if (monster.InstanceId == instanceId)
                {
                    return monster;
                }
            }

            return null;
        }

        public List<MonsterSpawn> GetMonstersRange(Vector3 position, int range)
        {
            List<MonsterSpawn> monsters = new List<MonsterSpawn>();

            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                if (Vector3.Distance(position, monsterPos) <= range)
                {
                    monsters.Add(monster);
                }
            }

            return monsters;
        }

        public List<Character> GetCharactersRange(Vector3 position, int range)
        {
            List<Character> characters = new List<Character>();

            foreach (NecClient client in ClientLookup.GetAll())
            {
                Character character = client.Character;
                Vector3 characterPos = new Vector3(character.X, character.Y, character.Z);
                if (Vector3.Distance(position, characterPos) <= range)
                {
                    characters.Add(character);
                }
            }

            return characters;
        }

        public List<TrapStack> GetTraps()
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    traps.Add(trap);
                }
            }

            return traps;
        }

        public List<TrapStack> GetTrapsCharacter(uint characterInstanceId)
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        traps.Add(trap);
                    }
                }
            }

            return traps;
        }

        public bool GetTrapsCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            bool inRange = false;
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap._trapTask.TrapPos, position);
                        if (distance < range)
                            return true;
                    }
                }
            }

            return inRange;
        }

        public TrapStack GetTrapCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            lock (TrapLock)
            {
                foreach (TrapStack trap in Traps.Values)
                {
                    if (trap._trapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap._trapTask.TrapPos, position);
                        if (distance < range)
                            return trap;
                    }
                }
            }

            return null;
        }

        public void AddTrap(uint instanceId, TrapStack trap)
        {
            lock (TrapLock)
            {
                Traps.Add(instanceId, trap);
            }
        }

        public void RemoveTrap(uint instanceId)
        {
            lock (TrapLock)
            {
                Traps.Remove(instanceId);
            }
        }

        public MonsterSpawn MonsterInRange(uint instanceId)
        {
            foreach (MonsterSpawn monster in MonsterSpawns.Values)
            {
                if (monster.InstanceId == instanceId)
                {
                    return monster;
                }
            }

            return null;
        }
    }
}
