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
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(MonsterSpawn));

        private readonly object AgroLock = new object();
        private readonly object TargetLock = new object();
        private readonly object AgroListLock = new object();
        private readonly object GotoLock = new object();

        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int CatalogId { get; set; }
        public int ModelId { get; set; }
        public int TextureType { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public Map Map { get; set; }
        public bool Active { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }

        public short Radius { get; set; }

        //private int CurrentHp { get; set; }
        private int GotoDistance;

        //public int MaxHp { get; set; }
        public bool CombatMode { get; set; }
        public int AttackSkillId { get; set; }
        public int RespawnTime { get; set; }
        public int CurrentCoordIndex { get; set; }
        public int MonsterWalkVelocity { get; }
        public int MonsterRunVelocity { get; }
        public bool SpawnActive { get; set; }
        public bool TaskActive { get; set; }
        private bool MonsterAgro;
        public bool MonsterVisible { get; set; }
        private Character CurrentTarget;
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public List<MonsterCoord> monsterCoords;
        public bool defaultCoords { get; set; }
        public Dictionary<uint, int> MonsterAgroList { get; set; }
        public BaseStat Hp;
        public BaseStat Mp;
        public BaseStat Od;
        public Loot loot;

        public MonsterSpawn()
        {
            Hp = new BaseStat(300, 300);
            RespawnTime = 10000;
            GotoDistance = 10;
            SpawnActive = false;
            TaskActive = false;
            MonsterAgro = false;
            defaultCoords = true;
            Created = DateTime.Now;
            Updated = DateTime.Now;
            monsterCoords = new List<MonsterCoord>();
            MonsterAgroList = new Dictionary<uint, int>();
            MonsterWalkVelocity = 175;
            MonsterRunVelocity = 300;
            MonsterVisible = false;
            loot = new Loot(this.Level, this.Id);
        }

        public void MonsterMove(NecServer server, NecClient client, int monsterVelocity, byte pose, byte animation,
            MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[CurrentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y,
                monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.X, this.Y, this.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.InstanceId); //Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.Router.Send(client, (ushort) AreaPacketId.recv_0x8D92, res,
                ServerType.Area); //recv_0xE8B9  recv_0x1FC1 
        }

        public void MonsterMove(NecServer server, int monsterVelocity, byte pose, byte animation,
            MonsterCoord monsterCoord = null)
        {
            if (monsterCoord == null)
                monsterCoord = monsterCoords[CurrentCoordIndex];
            Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y,
                monsterCoord.destination.Z);
            Vector3 monsterPos = new Vector3(this.X, this.Y, this.Z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / monsterVelocity;
            float xTick = moveTo.X / travelTime;
            float yTick = moveTo.Y / travelTime;
            float zTick = moveTo.Z / travelTime;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.InstanceId); //Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterMove(NecServer server, byte pose, byte animation, MonsterTick moveTo, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.InstanceId); //Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(moveTo.xTick); //X per tick
            res.WriteFloat(moveTo.yTick); //Y Per tick
            res.WriteFloat((float) 1); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterStop(NecServer server, NecClient client, byte pose, byte animation, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.InstanceId); //Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(0.0F); //X per tick
            res.WriteFloat(0.0F); //Y Per tick
            res.WriteFloat(0.0F); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.Router.Send(client, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void MonsterStop(NecServer server, byte pose, byte animation, float travelTime)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(this.InstanceId); //Monster ID
            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteFloat(0.0F); //X per tick
            res.WriteFloat(0.0F); //Y Per tick
            res.WriteFloat(0.0F); //verticalMovementSpeedMultiplier

            res.WriteFloat((float) 1 / travelTime); //movementMultiplier
            res.WriteFloat((float) travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.Router.Send(Map, (ushort) AreaPacketId.recv_0x8D92, res, ServerType.Area);
        }

        public void
            MonsterOrient(
                NecServer server) // Need to change this to a recv_ with a time attribute, monsters shouldn't turn instantly
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(this.InstanceId);

            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteByte(this.Heading);
            res.WriteByte(1);
            server.Router.Send(Map, (ushort) AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }

        public void
            MonsterOrient(NecServer server,
                NecClient client) // Need to change this to a recv_ with a time attribute, monsters shouldn't turn instantly
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32(this.InstanceId);

            res.WriteFloat(this.X);
            res.WriteFloat(this.Y);
            res.WriteFloat(this.Z);
            res.WriteByte(this.Heading);
            res.WriteByte(1);
            server.Router.Send(client, (ushort) AreaPacketId.recv_0x6B6A, res, ServerType.Area);
        }

        public void SendBattlePoseStartNotify(NecServer server)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(InstanceId);
            server.Router.Send(Map, (ushort) AreaPacketId.recv_battle_attack_pose_start_notify, res, ServerType.Area);
        }

        public void SendBattlePoseEndNotify(NecServer server)
        {
            IBuffer res = BufferProvider.Provide();
            server.Router.Send(Map, (ushort) AreaPacketId.recv_battle_attack_pose_end_notify, res, ServerType.Area);
        }

        public void MonsterHate(NecServer server, bool hateOn, uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(InstanceId);
            res.WriteUInt32(instanceId);
            if (hateOn)
            {
                server.Router.Send(Map, (ushort) AreaPacketId.recv_monster_hate_on, res, ServerType.Area);
            }
            else
            {
                server.Router.Send(Map, (ushort) AreaPacketId.recv_monster_hate_off, res, ServerType.Area);
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
        public void UpdateHP(int modifier, NecServer server = null, bool verifyAgro = false, uint instanceId = 0)
        {
            Hp.Modify(modifier);
            if (verifyAgro)
            {
                if (server == null)
                {
                    Logger.Error($"NecServer is null!");
                    return;
                }

                if (!GetAgroCharacter(instanceId))
                {
                    MonsterAgroList.Add(instanceId, modifier);
                    Character character = (Character) server.Instances.GetInstance((uint) instanceId);
                    SetCurrentTarget(character);
                    SetAgro(true);
                    MonsterHate(server, true, instanceId);
                    SendBattlePoseStartNotify(server);
                    if (Id == 4)
                        SetGotoDistance(1000);
                    else
                        SetGotoDistance(200);
                }
            }
        }

        public void SetAgro(bool agroOn)
        {
            lock (AgroLock)
            {
                MonsterAgro = agroOn;
            }
        }

        public bool GetAgro()
        {
            bool agro = false;
            lock (AgroLock)
            {
                agro = MonsterAgro;
            }

            return agro;
        }

        public void SetCurrentTarget(Character character)
        {
            lock (TargetLock)
            {
                CurrentTarget = character;
            }
        }

        public Character GetCurrentTarget()
        {
            Character character = null;
            lock (TargetLock)
            {
                character = CurrentTarget;
            }

            return character;
        }

        public void AddAgroList(uint instanceId, int damage)
        {
            lock (AgroListLock)
            {
                MonsterAgroList.Add(instanceId, damage);
            }
        }

        public List<uint> GetAgroInstanceList()
        {
            List<uint> agroInstanceList = new List<uint>();
            lock (AgroListLock)
            {
                foreach (uint instanceId in MonsterAgroList.Keys)
                {
                    agroInstanceList.Add(instanceId);
                }
            }

            return agroInstanceList;
        }

        public uint GetAgroHigh()
        {
            uint instancedId;
            lock (AgroListLock)
            {
                instancedId = MonsterAgroList.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;
            }

            return instancedId;
        }

        public bool GetAgroCharacter(uint instanceId)
        {
            bool agro;
            lock (AgroListLock)
            {
                agro = MonsterAgroList.ContainsKey(instanceId);
            }

            return agro;
        }

        public void ClearAgroList()
        {
            lock (AgroListLock)
            {
                MonsterAgroList.Clear();
            }
        }

        public void SetGotoDistance(int modifier)
        {
            lock (GotoLock)
            {
                GotoDistance = modifier;
            }
        }

        public int GetGotoDistance()
        {
            int gotoDistance;
            lock (GotoLock)
            {
                gotoDistance = GotoDistance;
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
        public int Id;
        public uint MonsterId { get; set; }
        public uint MapId { get; set; }
        public int CoordIdx { get; set; }
        public Vector3 destination { get; set; }
    }
}
