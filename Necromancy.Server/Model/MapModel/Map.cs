using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Model.MapModel;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class Map

    {
        public const int NEW_CHARACTER_MAP_ID = 1001002; //2006000
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(Map));

        private readonly NecServer _server;

        private readonly object _trapLock = new object();


        public Map(MapData mapData, NecServer server)
        {
            _server = server;
            clientLookup = new ClientLookup();
            this.npcSpawns = new Dictionary<uint, NpcSpawn>();
            this.monsterSpawns = new Dictionary<uint, MonsterSpawn>();
            this.gimmickSpawns = new Dictionary<uint, Gimmick>();
            this.mapTransitions = new Dictionary<uint, MapTransition>();
            this.gGateSpawns = new Dictionary<uint, GGateSpawn>();
            deadBodies = new Dictionary<uint, DeadBody>();


            id = mapData.id;
            x = mapData.x;
            y = mapData.y;
            z = mapData.z;
            country = mapData.country;
            area = mapData.area;
            place = mapData.place;
            orientation = (byte)(mapData.orientation / 2); // Client uses 180 degree orientation
            traps = new Dictionary<uint, TrapStack>();

            List<MapTransition> mapTransitions = server.database.SelectMapTransitionsByMapId(mapData.id);
            foreach (MapTransition mapTran in mapTransitions)
            {
                if (mapTran.id == 0) //Only one special transition
                {
                    double angle = mapTran.MaplinkHeading / 255.0;
                    mapTran.LeftPos.X = (float)((mapTran.ReferencePos.X + mapTran.MaplinkWidth / 2) * Math.Cos(angle));
                    mapTran.LeftPos.Y = (float)(mapTran.ReferencePos.Y * Math.Cos(angle));
                    mapTran.RightPos.X = (float)((mapTran.ReferencePos.X - mapTran.MaplinkWidth / 2) * Math.Cos(angle));
                    mapTran.RightPos.Y = mapTran.ReferencePos.Y + mapTran.MaplinkWidth / 2;
                }
                else if (mapTran.InvertedTransition != true) //map is x dominant
                {
                    mapTran.LeftPos.X = mapTran.ReferencePos.X + mapTran.MaplinkWidth / 2;
                    mapTran.LeftPos.Y = mapTran.ReferencePos.Y;
                    mapTran.RightPos.X = mapTran.ReferencePos.X - mapTran.MaplinkWidth / 2;
                    mapTran.RightPos.Y = mapTran.ReferencePos.Y;
                }
                else if (mapTran.InvertedTransition) //map is y dominant
                {
                    mapTran.LeftPos.X = mapTran.ReferencePos.X;
                    mapTran.LeftPos.Y = mapTran.ReferencePos.Y + mapTran.MaplinkWidth / 2;
                    mapTran.RightPos.X = mapTran.ReferencePos.X;
                    mapTran.RightPos.Y = mapTran.ReferencePos.Y - mapTran.MaplinkWidth / 2;
                }

                server.instances.AssignInstance(mapTran);
                this.mapTransitions.Add(mapTran.instanceId, mapTran);
                _Logger.Debug($"Loaded Map transition {mapTran.id} on map {fullName}");
            }

            //Assign Unique Instance ID to each NPC per map. Add to dictionary stored with the Map object
            List<NpcSpawn> npcSpawns = server.database.SelectNpcSpawnsByMapId(mapData.id);
            foreach (NpcSpawn npcSpawn in npcSpawns)
            {
                server.instances.AssignInstance(npcSpawn);
                this.npcSpawns.Add(npcSpawn.instanceId, npcSpawn);
            }

            //Assign Unique Instance ID to each Gimmick per map. Add to dictionary stored with the Map object
            List<Gimmick> gimmickSpawns = server.database.SelectGimmicksByMapId(mapData.id);
            foreach (Gimmick gimmickSpawn in gimmickSpawns)
            {
                server.instances.AssignInstance(gimmickSpawn);
                this.gimmickSpawns.Add(gimmickSpawn.instanceId, gimmickSpawn);
            }

            List<GGateSpawn> gGateSpawns = server.database.SelectGGateSpawnsByMapId(mapData.id);
            foreach (GGateSpawn gGateSpawn in gGateSpawns)
            {
                server.instances.AssignInstance(gGateSpawn);
                this.gGateSpawns.Add(gGateSpawn.instanceId, gGateSpawn);
            }

            //To-Do   | for each deadBody in Deadbodies {RecvDataNotifyCharabodyData}

            List<MonsterSpawn> monsterSpawns = server.database.SelectMonsterSpawnsByMapId(mapData.id);
            foreach (MonsterSpawn monsterSpawn in monsterSpawns)
            {
                server.instances.AssignInstance(monsterSpawn);
                if (!_server.settingRepository.modelCommon.TryGetValue(monsterSpawn.modelId,
                    out ModelCommonSetting modelSetting))
                {
                    _Logger.Error($"Error getting ModelCommonSetting for ModelId {monsterSpawn.modelId}");
                    continue;
                }

                if (!_server.settingRepository.monster.TryGetValue(monsterSpawn.monsterId,
                    out MonsterSetting monsterSetting))
                {
                    _Logger.Error($"Error getting MonsterSetting for MonsterId {monsterSpawn.monsterId}");
                    continue;
                }

                monsterSpawn.modelId = modelSetting.id;
                //monsterSpawn.Size = (short) (modelSetting.Height / 2);   //commenting out to use size setting from database.
                monsterSpawn.radius = (short)modelSetting.radius;
                monsterSpawn.Hp.SetMax(300);
                monsterSpawn.Hp.SetCurrent(300);
                monsterSpawn.attackSkillId = monsterSetting.attackSkillId;
                //monsterSpawn.Level = (byte) monsterSetting.Level;
                monsterSpawn.combatMode = monsterSetting.combatMode;
                monsterSpawn.catalogId = monsterSetting.catalogId;
                monsterSpawn.textureType = monsterSetting.textureType;
                monsterSpawn.map = this;
                this.monsterSpawns.Add(monsterSpawn.instanceId, monsterSpawn);

                List<MonsterCoord> coords = server.database.SelectMonsterCoordsByMonsterId(monsterSpawn.id);
                if (coords.Count > 0)
                {
                    monsterSpawn.defaultCoords = false;
                    monsterSpawn.MonsterCoords.Clear();
                    foreach (MonsterCoord monsterCoord in coords)
                        //Console.WriteLine($"added coord {monsterCoord} to monster {monsterSpawn.InstanceId}");
                        monsterSpawn.MonsterCoords.Add(monsterCoord);
                }
                else
                {
                    //home coordinate set to monster X,Y,Z from database
                    Vector3 homeVector3 = new Vector3(monsterSpawn.x, monsterSpawn.y, monsterSpawn.z);
                    MonsterCoord homeCoord = new MonsterCoord();
                    homeCoord.Id = monsterSpawn.id;
                    homeCoord.monsterId = (uint)monsterSpawn.monsterId;
                    homeCoord.mapId = (uint)monsterSpawn.mapId;
                    homeCoord.coordIdx = 0;
                    homeCoord.destination = homeVector3;
                    monsterSpawn.MonsterCoords.Add(homeCoord);

                    //default path part 2
                    Vector3 defaultVector3 = new Vector3(monsterSpawn.x, monsterSpawn.y + Util.GetRandomNumber(50, 150),
                        monsterSpawn.z);
                    MonsterCoord defaultCoord = new MonsterCoord();
                    defaultCoord.Id = monsterSpawn.id;
                    defaultCoord.monsterId = (uint)monsterSpawn.monsterId;
                    defaultCoord.mapId = (uint)monsterSpawn.mapId;
                    defaultCoord.coordIdx = 1;
                    defaultCoord.destination = defaultVector3;

                    monsterSpawn.MonsterCoords.Add(defaultCoord);

                    //default path part 3
                    Vector3 defaultVector32 = new Vector3(monsterSpawn.x + Util.GetRandomNumber(50, 150),
                        monsterSpawn.y + Util.GetRandomNumber(50, 150), monsterSpawn.z);
                    MonsterCoord defaultCoord2 = new MonsterCoord();
                    defaultCoord2.Id = monsterSpawn.id;
                    defaultCoord2.monsterId = (uint)monsterSpawn.monsterId;
                    defaultCoord2.mapId = (uint)monsterSpawn.mapId;
                    defaultCoord2.coordIdx = 2; //64 is currently the Idx of monsterHome on send_map_get_info.cs
                    defaultCoord2.destination = defaultVector32;

                    monsterSpawn.MonsterCoords.Add(defaultCoord2);
                }
            }
        }

        public int id { get; set; }
        public float x { get; }
        public float y { get; }
        public float z { get; }
        public string country { get; set; }
        public string area { get; set; }
        public string place { get; set; }
        public byte orientation { get; }
        public string fullName => $"{country}/{area}/{place}";
        public ClientLookup clientLookup { get; }

        public Dictionary<uint, NpcSpawn> npcSpawns { get; }

        // public Dictionary<int, TrapTransition> Trap { get; }
        public Dictionary<uint, MonsterSpawn> monsterSpawns { get; }
        public Dictionary<uint, Gimmick> gimmickSpawns { get; }
        public Dictionary<uint, MapTransition> mapTransitions { get; }
        public Dictionary<uint, TrapStack> traps { get; }
        public Dictionary<uint, DeadBody> deadBodies { get; }
        public Dictionary<uint, GGateSpawn> gGateSpawns { get; }

        public void EnterForce(NecClient client, MapPosition mapPosition = null)
        {
            client.character.mapChange = true;
            Enter(client, mapPosition);
            _server.router.Send(new RecvMapChangeForce(this, mapPosition, _server.setting, client), client);

            // currently required to prevent disconnect by force changing
            _server.router.Send(new RecvMapChangeSyncOk(), client);
        }

        public void Enter(NecClient client, MapPosition mapPosition = null)
        {
            if (client.map != null) client.map.Leave(client);
            client.map = this;

            _Logger.Info(client, $"Entering Map: {id}:{fullName}");
            // If position is passed in use it and set character position, if null then use map default coords
            // If this isn't set here, the wrong coords are in character until send_movement_info updates it.
            if (mapPosition != null)
            {
                client.character.x = mapPosition.x;
                client.character.y = mapPosition.y;
                client.character.z = mapPosition.z;
                client.character.heading = mapPosition.heading;
            }
            //set character coords to default map entry coords If arriving form another map.
            else if (client.character.mapId != id)
            {
                client.character.x = x;
                client.character.y = y;
                client.character.z = z;
                client.character.heading = orientation;
            }

            client.character.mapId = id;
            client.character.mapChange = false;
            clientLookup.Add(client);
            _Logger.Debug($"Client Lookup count is now : {clientLookup.GetAll().Count}  for map  {id} ");
            _Logger.Debug($"Character State for character {client.character.name} is {client.character.state}");
            //Send your character data to the other living or dead players on the map.

            //on successful map entry, update the client database position
            if (!_server.database.UpdateCharacter(client.character)) _Logger.Error("Could not update the database with current known player position");
            if (!_server.database.UpdateSoul(client.soul)) _Logger.Error("Could not update the database with soul details ");

            //ToDo  move all this rendering logic to Send_Map_Entry.   We dont need a copy of this logic on every map instance.
            RecvDataNotifyCharaData myCharacterData = new RecvDataNotifyCharaData(client.character, client.soul.name);

            //dead
            //you are dead here.  only getting soul form characters. sorry bro.
            if (client.character.state.HasFlag(CharacterState.SoulForm))
                foreach (NecClient otherClient in clientLookup.GetAll())
                {
                    if (otherClient == client) continue;
                    if (otherClient.character.state.HasFlag(CharacterState.SoulForm)) _server.router.Send(myCharacterData, otherClient);
                }
            else //Bro, you alive! You gon see living characters!
                foreach (NecClient otherClient in clientLookup.GetAll())
                {
                    if (otherClient == client) continue;
                    if (otherClient.character.state.HasFlag(CharacterState.SoulForm)) continue;
                    _server.router.Send(myCharacterData, otherClient);
                }

            if (client.union != null)
            {
                RecvDataNotifyUnionData myUnionData = new RecvDataNotifyUnionData(client.character, client.union.name);
                _server.router.Send(this, myUnionData, client);
            }

            Task.Delay(TimeSpan.FromSeconds(10)).ContinueWith
            (t1 =>
                {
                    foreach (MonsterSpawn monsterSpawn in monsterSpawns.Values)
                        if (monsterSpawn.active)
                        {
                            monsterSpawn.spawnActive = true;
                            if (!monsterSpawn.taskActive)
                            {
                                MonsterTask monsterTask = new MonsterTask(_server, monsterSpawn);
                                if (monsterSpawn.defaultCoords)
                                    monsterTask.MonsterHome = monsterSpawn.MonsterCoords[0];
                                else
                                    monsterTask.MonsterHome = monsterSpawn.MonsterCoords.Find(x => x.coordIdx == 64);
                                monsterTask.Start();
                            }
                            else
                            {
                                if (monsterSpawn.monsterVisible)
                                {
                                    _Logger.Debug($"MonsterTask already running for [{monsterSpawn.name}]");
                                    RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                                    _server.router.Send(monsterData, client);
                                    if (!monsterSpawn.GetAgro())
                                        monsterSpawn.MonsterMove(_server, client, monsterSpawn.monsterWalkVelocity, 2,
                                            0);
                                }
                            }
                        }
                }
            );
        }

        public void Leave(NecClient client)
        {
            _Logger.Info(client, $"Leaving Map: {id}:{fullName}");
            clientLookup.Remove(client);
            if (!_server.database.UpdateCharacter(client.character)) _Logger.Error("Could not update the database with last known player position");
            if (!_server.database.UpdateSoul(client.soul)) _Logger.Error("Could not update the database with soul details ");


            client.map = null;

            RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(client.character.instanceId);
            _server.router.Send(this, objectDisappearData, client);
            if (clientLookup.GetAll().Count == 0)
                foreach (MonsterSpawn monsterSpawn in monsterSpawns.Values)
                    monsterSpawn.spawnActive = false;

            _Logger.Debug($"Client Lookup count is now : {clientLookup.GetAll().Count}  for map  {id} ");
        }

        public bool MonsterInRange(Vector3 position, int range)
        {
            foreach (MonsterSpawn monster in monsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                if (Vector3.Distance(position, monsterPos) <= range) return true;
            }

            return false;
        }

        public MonsterSpawn GetMonsterByInstanceId(uint instanceId)
        {
            foreach (MonsterSpawn monster in monsterSpawns.Values)
                if (monster.instanceId == instanceId)
                    return monster;

            return null;
        }

        public List<MonsterSpawn> GetMonstersRange(Vector3 position, int range)
        {
            List<MonsterSpawn> monsters = new List<MonsterSpawn>();

            foreach (MonsterSpawn monster in monsterSpawns.Values)
            {
                Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                if (Vector3.Distance(position, monsterPos) <= range) monsters.Add(monster);
            }

            return monsters;
        }

        public List<Character> GetCharactersRange(Vector3 position, int range)
        {
            List<Character> characters = new List<Character>();

            foreach (NecClient client in clientLookup.GetAll())
            {
                Character character = client.character;
                Vector3 characterPos = new Vector3(character.x, character.y, character.z);
                if (Vector3.Distance(position, characterPos) <= range) characters.Add(character);
            }

            return characters;
        }

        public List<TrapStack> GetTraps()
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (_trapLock)
            {
                foreach (TrapStack trap in this.traps.Values) traps.Add(trap);
            }

            return traps;
        }

        public List<TrapStack> GetTrapsCharacter(uint characterInstanceId)
        {
            List<TrapStack> traps = new List<TrapStack>();
            lock (_trapLock)
            {
                foreach (TrapStack trap in this.traps.Values)
                    if (trap.TrapTask.ownerInstanceId == characterInstanceId)
                        traps.Add(trap);
            }

            return traps;
        }

        public bool GetTrapsCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            bool inRange = false;
            lock (_trapLock)
            {
                foreach (TrapStack trap in traps.Values)
                    if (trap.TrapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap.TrapTask.trapPos, position);
                        if (distance < range)
                            return true;
                    }
            }

            return inRange;
        }

        public TrapStack GetTrapCharacterRange(uint characterInstanceId, int range, Vector3 position)
        {
            lock (_trapLock)
            {
                foreach (TrapStack trap in traps.Values)
                    if (trap.TrapTask.ownerInstanceId == characterInstanceId)
                    {
                        double distance = Vector3.Distance(trap.TrapTask.trapPos, position);
                        if (distance < range)
                            return trap;
                    }
            }

            return null;
        }

        public void AddTrap(uint instanceId, TrapStack trap)
        {
            lock (_trapLock)
            {
                traps.Add(instanceId, trap);
            }
        }

        public void RemoveTrap(uint instanceId)
        {
            lock (_trapLock)
            {
                traps.Remove(instanceId);
            }
        }

        public MonsterSpawn MonsterInRange(uint instanceId)
        {
            foreach (MonsterSpawn monster in monsterSpawns.Values)
                if (monster.instanceId == instanceId)
                    return monster;

            return null;
        }
    }
}
