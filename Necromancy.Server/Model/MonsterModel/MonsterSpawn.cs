using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(MonsterSpawn));

        private readonly object _agroLock = new object();
        private readonly object _targetLock = new object();
        private readonly object _agroListLock = new object();
        private readonly object _gotoLock = new object();

        public uint instanceId { get; set; }
        public int id { get; set; }
        public int monsterId { get; set; }
        public int catalogId { get; set; }
        public int modelId { get; set; }
        public int textureType { get; set; }
        public byte level { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int mapId { get; set; }
        public Map map { get; set; }
        public bool active { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public byte heading { get; set; }
        public short size { get; set; }

        public short radius { get; set; }

        //private int CurrentHp { get; set; }
        private int _gotoDistance;

        //public int MaxHp { get; set; }
        public bool combatMode { get; set; }
        public int attackSkillId { get; set; }
        public int respawnTime { get; set; }
        public int currentCoordIndex { get; set; }
        public int monsterWalkVelocity { get; }
        public int monsterRunVelocity { get; }
        public bool spawnActive { get; set; }
        public bool taskActive { get; set; }
        private bool _monsterAgro;
        public bool monsterVisible { get; set; }
        private Character _currentTarget;
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public List<MonsterCoord> monsterCoords;
        public bool defaultCoords { get; set; }
        public Dictionary<uint, int> monsterAgroList { get; set; }
        public BaseStat hp;
        public BaseStat mp;
        public BaseStat od;
        public Loot loot;

        public MonsterSpawn()
        {
            hp = new BaseStat(300, 300);
            respawnTime = 10000;
            _gotoDistance = 10;
            spawnActive = false;
            taskActive = false;
            _monsterAgro = false;
            defaultCoords = true;
            created = DateTime.Now;
            updated = DateTime.Now;
            monsterCoords = new List<MonsterCoord>();
            monsterAgroList = new Dictionary<uint, int>();
            monsterWalkVelocity = 175;
            monsterRunVelocity = 300;
            monsterVisible = false;
            loot = new Loot(this.level, this.id);
        }

        public void MonsterMove(NecServer server, NecClient client, int monsterVelocity, byte pose, byte animation,
            MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[currentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y,
                monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.x, this.y, this.z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId); //Monster ID
            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(client, (ushort) AreaPacketId.recv_0x8D92, res,
                ServerType.Area); //recv_0xE8B9  recv_0x1FC1
        }

        public void MonsterMove(NecServer server, int monsterVelocity, byte pose, byte animation,
            MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[currentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y,
                monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.x, this.y, this.z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;
            float xTick = moveTo.X / travelTime;
            float yTick = moveTo.Y / travelTime;
            float zTick = moveTo.Z / travelTime;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId); //Monster ID
            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterMove(NecServer server, byte pose, byte animation, MonsterTick moveTo, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId); //Monster ID
            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteFloat(moveTo.xTick); //X per tick
            res.WriteFloat(moveTo.yTick); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterStop(NecServer server, NecClient client, byte pose, byte animation, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId); //Monster ID
            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteFloat(0.0F); //X per tick
            res.WriteFloat(0.0F); //Y Per tick
            res.WriteFloat(0.0F); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(client, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterStop(NecServer server, byte pose, byte animation, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId); //Monster ID
            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteFloat(0.0F); //X per tick
            res.WriteFloat(0.0F); //Y Per tick
            res.WriteFloat(0.0F); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void
            MonsterOrient(
                NecServer server) // Need to change this to a recv_ with a time attribute, monsters shouldn't turn instantly
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(this.instanceId);

            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteByte(this.heading);
            res.WriteByte(1);
            server.router.Send(map, (ushort) AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }

        public void
            MonsterOrient(NecServer server,
                NecClient client) // Need to change this to a recv_ with a time attribute, monsters shouldn't turn instantly
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(this.instanceId);

            res.WriteFloat(this.x);
            res.WriteFloat(this.y);
            res.WriteFloat(this.z);
            res.WriteByte(this.heading);
            res.WriteByte(1);
            server.router.Send(client, (ushort) AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }

        public void SendBattlePoseStartNotify(NecServer server)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(instanceId);
            server.router.Send(map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
        }

        public void SendBattlePoseEndNotify(NecServer server)
        {
            IBuffer res = BufferProvider.Provide();
            server.router.Send(map, (ushort) AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
        }

        public void MonsterHate(NecServer server, bool hateOn, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.instanceId);
            res.WriteUInt32(instanceId);
            if (hateOn)
            {
                server.router.Send(map, (ushort) AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
            }
            else
            {
                server.router.Send(map, (ushort) AreaPacketId.recv_monster_hate_off, res, ServerType.Area);
            }
        }

        /*public void SetHP(int modifier)
        {
            lock (HpLock)
            {
                CurrentHp = modifier;
            }
        }
        public int GetHP()
        {
            int hp;
            lock (HpLock)
            {
                hp = CurrentHp;
            }
            return hp;
        }
        */
        public void UpdateHp(int modifier, NecServer server = null, bool verifyAgro = false, uint instanceId = 0)
        {
            hp.Modify(modifier);
            if (verifyAgro)
            {
                if (server == null)
                {
                    _Logger.Error($"NecServer is null!");
                    return;
                }

                if (!GetAgroCharacter(instanceId))
                {
                    monsterAgroList.Add(instanceId, modifier);
                    Character character = (Character) server.instances.GetInstance((uint) instanceId);
                    SetCurrentTarget(character);
                    SetAgro(true);
                    MonsterHate(server, true, instanceId);
                    SendBattlePoseStartNotify(server);
                    if (id == 4)
                        SetGotoDistance(1000);
                    else
                        SetGotoDistance(200);
                }
            }
        }

        public void SetAgro(bool agroOn)
        {
            lock (_agroLock)
            {
                _monsterAgro = agroOn;
            }
        }

        public bool GetAgro()
        {
            bool agro = false;
            lock (_agroLock)
            {
                agro = _monsterAgro;
            }

            return agro;
        }

        public void SetCurrentTarget(Character character)
        {
            lock (_targetLock)
            {
                _currentTarget = character;
            }
        }

        public Character GetCurrentTarget()
        {
            Character character = null;
            lock (_targetLock)
            {
                character = _currentTarget;
            }

            return character;
        }

        public void AddAgroList(uint instanceId, int damage)
        {
            lock (_agroListLock)
            {
                monsterAgroList.Add(instanceId, damage);
            }
        }

        public List<uint> GetAgroInstanceList()
        {
            List<uint> agroInstanceList = new List<uint>();
            lock (_agroListLock)
            {
                foreach (uint instanceId in monsterAgroList.Keys)
                {
                    agroInstanceList.Add(instanceId);
                }
            }

            return agroInstanceList;
        }

        public uint GetAgroHigh()
        {
            uint instancedId;
            lock (_agroListLock)
            {
                instancedId = monsterAgroList.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            }

            return instancedId;
        }

        public bool GetAgroCharacter(uint instanceId)
        {
            bool agro;
            lock (_agroListLock)
            {
                agro = monsterAgroList.ContainsKey(instanceId);
            }

            return agro;
        }

        public void ClearAgroList()
        {
            lock (_agroListLock)
            {
                monsterAgroList.Clear();
            }
        }

        public void SetGotoDistance(int modifier)
        {
            lock (_gotoLock)
            {
                _gotoDistance = modifier;
            }
        }

        public int GetGotoDistance()
        {
            int gotoDistance;
            lock (_gotoLock)
            {
                gotoDistance = _gotoDistance;
            }

            return gotoDistance;
        }
    }

    public class MonsterTick
    {
        public float xTick;
        public float yTick;
        public float zTick;
    }

    public class MonsterCoord
    {
        public int id;
        public uint monsterId { get; set; }
        public uint mapId { get; set; }
        public int coordIdx { get; set; }
        public Vector3 destination { get; set; }
    }
}
