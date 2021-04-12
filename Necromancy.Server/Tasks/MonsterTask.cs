using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks.Core;

namespace Necromancy.Server.Tasks
{
    // Usage: create a monster and spawn it then use the following
    //MonsterThread monsterThread = new MonsterThread(Server, client, monsterSpawn);
    //Thread workerThread2 = new Thread(monsterThread.InstanceMethod);
    //workerThread2.Start();

    public class MonsterTask : PeriodicTask
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(MonsterTask));

        private static readonly int[] _skillList = {200301411, 200301412, 200301413, 200301414, 200301415, 200301416, 200301417};

        private static int[] _effectList = {301411, 301412, 301413, 301414, 301415, 301416, 301417};
        private int _agroCheckTime;
        private readonly float _agroDetectAngle;
        private readonly float _agroMoveAngle;
        private int _agroMoveTime;

        private readonly int _agroTick;
        private readonly MonsterTick _agroTickMove;
        private bool _casting;
        private int _castState;
        private Vector3 _currentDest;
        private int _currentSkill;
        private int _currentWait;
        private readonly Map _map;
        private bool _monsterWaiting;

        //private int currentDest;
        private int _moveTime;
        private readonly int _pathingTick;

        //private int monsterVelocity;
        private int _respawnTime;
        private readonly uint _skillInstanceId;
        private bool _spawnMonster;
        private int _updateTime;
        private int _waitTime;
        public MonsterCoord monsterHome;

        public MonsterTask(NecServer server, MonsterSpawn monster)
        {
            this.monster = monster;
            this.server = server;
            monsterFreeze = false;
            monsterActive = true;
            monsterMoving = false;
            _casting = false;
            _spawnMonster = true;
            monsterHome = null;
            this.monster.currentCoordIndex = 1;
            _pathingTick = 100;
            _agroTick = 200;
            _updateTime = _pathingTick;
            _agroMoveTime = 0;
            _agroTickMove = new MonsterTick();
            _waitTime = 2000;
            _currentWait = 0;
            _moveTime = _updateTime;
            _monsterWaiting = true;
            agroRange = 1000;
            _agroCheckTime = -1;
            _agroDetectAngle = (float)Math.Cos(Math.PI / 1.9);
            _agroMoveAngle = (float)Math.Cos(Math.PI / 4);
            _castState = 0;
            _respawnTime = 10000;
            _currentSkill = 0;
            _skillInstanceId = 0;
            _map = this.server.maps.Get(this.monster.mapId);
            _currentDest = new Vector3();
        }

        protected NecServer server { get; }
        public MonsterSpawn monster { get; set; }
        public bool monsterFreeze { get; set; }
        public bool monsterActive { get; set; }
        public bool monsterMoving { get; set; }
        public int agroRange { get; set; }

        public override string taskName => $"MonsterTask : {monster.instanceId} - {monster.name}";
        public override TimeSpan taskTimeSpan { get; }
        protected override bool taskRunAtStart => false;


        protected override void Execute()
        {
            monster.taskActive = true;
            while (monsterActive && monster.spawnActive)
            {
                if (_spawnMonster)
                {
                    monster.loot = new Loot(monster.level, monster.id); //reload loot table
                    Thread.Sleep(_respawnTime / 2);
                    _updateTime = _pathingTick;
                    monster.currentCoordIndex = 1;
                    _moveTime = _updateTime;
                    MonsterSpawn();
                    Thread.Sleep(2000);
                }

                //MonsterCoord nextCoord = _monster.monsterCoords.Find(x => x.CoordIdx == _monster.CurrentCoordIndex);
                //Vector3 monster = new Vector3(_monster.X, _monster.Y, _monster.Z);
                //float distance = GetDistance(nextCoord.destination, monster);
                if (monster.GetAgro())
                {
                    if (MonsterAgro())
                        continue;
                }
                else
                {
                    MonsterPath();
                }

                Thread.Sleep(_moveTime);
                if (_monsterWaiting)
                {
                    _currentWait += _updateTime;
                    if (_currentWait >= _waitTime)
                    {
                        _monsterWaiting = false;
                        _currentWait = 0;
                    }
                }
            }

            Stop();
            monster.taskActive = false;
        }

        private void MonsterPath()
        {
            MonsterCoord nextCoord = this.monster.monsterCoords.Find(x => x.coordIdx == this.monster.currentCoordIndex);
            Vector3 monster = new Vector3(this.monster.x, this.monster.y, this.monster.z);
            float distance = GetDistance(nextCoord.destination, monster);
            if (distance > this.monster.GetGotoDistance() && !monsterFreeze && !_monsterWaiting && !this.monster.GetAgro())
            {
                MonsterMove(nextCoord);
            }
            else if (monsterMoving)
            {
                //Thread.Sleep(updateTime/2); //Allow for cases where the remaining distance is less than the gotoDistance
                this.monster.MonsterStop(server, 1, 0, .1F);
                monsterMoving = false;
                if (!this.monster.GetAgro())
                {
                    _monsterWaiting = true;
                    _currentWait = 0;
                    //                        Thread.Sleep(2000);
                    if (this.monster.currentCoordIndex < this.monster.monsterCoords.Count - (this.monster.defaultCoords ? 1 : 2))
                        this.monster.currentCoordIndex++;
                    else
                        this.monster.currentCoordIndex = 0;

                    this.monster.heading = (byte)GetHeading(this.monster.monsterCoords
                        .Find(x => x.coordIdx == this.monster.currentCoordIndex).destination);
                }
            }

            if (MonsterCheck())
            {
                _Logger.Debug("MonsterCheck returned true");
                return;
            }

            if (MonsterAgroCheck())
                this.monster.SetAgro(true);
            if (this.monster.GetAgro())
            {
                monsterMoving = false;
                this.monster.MonsterStop(server, 1, 0, 0.1F);
                this.monster.MonsterHate(server, true, this.monster.GetCurrentTarget().instanceId);
                this.monster.SendBattlePoseStartNotify(server);
                _updateTime = _agroTick;
                if (this.monster.monsterId == 100201)
                    this.monster.SetGotoDistance(1000); //Caster Distance
                else
                    this.monster.SetGotoDistance(200); //Melee Distance
                //monsterVelocity = 500;
                _moveTime = _agroTick;
                _agroCheckTime = 0;

                OrientMonster();
                MonsterAgroMove();
            }
        }

        private bool MonsterAgro()
        {
            Vector3 monster = new Vector3(this.monster.x, this.monster.y, this.monster.z);
            Character currentTarget = this.monster.GetCurrentTarget();
            if (currentTarget == null)
            {
                _Logger.Error("No character target set for agroed monster");
                return true;
            }

            if (MonsterCheck())
            {
                _Logger.Debug("MonsterCheck returned true");
                return true;
            }

            float homeDistance = GetDistance(monsterHome.destination, monster);
            if (homeDistance >= agroRange * 4)
            {
                foreach (uint instanceId in this.monster.GetAgroInstanceList()) this.monster.MonsterHate(server, false, instanceId);

                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(this.monster.instanceId);
                server.router.Send(_map, objectDisappearData);
                _spawnMonster = true;
                _Logger.Debug("Too far from home");
                return true;
            }

            MonsterAgroAdjust();
            Vector3 character = new Vector3(currentTarget.x, currentTarget.y, currentTarget.z);
            float distanceChar = GetDistance(character, monster);
            if (distanceChar <= this.monster.GetGotoDistance() + 50)
            {
                if (monsterMoving)
                {
                    Thread.Sleep(_updateTime / 2);
                    monsterMoving = false;
                    this.monster.MonsterStop(server, 1, 0, 0.1F);
                    Thread.Sleep(100);
                }

                if (!_monsterWaiting)
                    switch (_castState)
                    {
                        case 0:
                            OrientMonster();
                            //Casters
                            if (this.monster.monsterId == 100201)
                            {
                                //  skillInstanceId = _server.Instances.CreateInstance<Skill>().InstanceId;
                                //  Logger.Debug($"attackId [200301411]");
                                //  //_monster.MonsterStop(Server,8, 231, 2.0F);
                                //  StartMonsterCastQueue(200301411, skillInstanceId);
                                //  PlayerDamage();
                                //  waitTime = 2000;
                                //  CastState = 1;
                            }
                            //Melee Attackers
                            else
                            {
                                int attackId = this.monster.attackSkillId * 100 + Util.GetRandomNumber(1, 4);
                                //Logger.Debug($"_monster.AttackSkillId [{_monster.AttackSkillId}]  attackId[{attackId}]");
                                MonsterAttackQueue(attackId);
                                PlayerDamage();
                                _waitTime = 5000;
                                _castState = 0;
                            }

                            _monsterWaiting = true;
                            _currentWait = 0;
                            break;
                        case 1:
                            //int skillInstanceID = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            MonsterCastQueue(200301411);
                            _monsterWaiting = true;
                            _waitTime = 1000;
                            _currentWait = 0;
                            _castState = 3;
                            break;
                        case 2:
                            _monsterWaiting = true;
                            _waitTime = 1000;
                            _currentWait = 0;
                            _castState = 3;
                            break;
                        case 3:
                            //int effectObjectInstanceId = (int)Server.Instances.CreateInstance<Skill>().InstanceId;
                            SendDataNotifyEoData(_skillInstanceId, 301411);
                            SendEoNotifyDisappearSchedule(_skillInstanceId);
                            MonsterCastMove(_skillInstanceId, 3000, 2, 2);
                            _monsterWaiting = true;
                            _waitTime = 5000;
                            _currentWait = 0;
                            _castState = 0;
                            if (_currentSkill < _skillList.Length - 1)
                                _currentSkill++;
                            else
                                _currentSkill = 0;
                            break;
                        case 4:
                            _monsterWaiting = true;
                            _waitTime = 5000;
                            _currentWait = 0;
                            _castState = 0;
                            break;
                    }
            }
            else
            {
                MonsterAgroMove();
            }

            return false;
        }

        private void StartMonsterCastQueue(int skillId, uint instanceId)
        {
            _casting = true;
            Character currentTarget = monster.GetCurrentTarget();
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(monster.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionMonsterSkillStartCast brStartCast =
                new RecvBattleReportActionMonsterSkillStartCast(currentTarget.instanceId, skillId);
            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            server.router.Send(_map, brList);
        }

        private void MonsterAttackQueue(int skillId)
        {
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(monster.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec(skillId);

            brList.Add(brStart);
            brList.Add(brAttack);
            brList.Add(brEnd);
            server.router.Send(_map, brList);
        }

        private void PlayerDamage()
        {
            int damage = Util.GetRandomNumber(8, 43);
            Character currentTarget = monster.GetCurrentTarget();
            currentTarget.hp.Modify(-damage, monster.instanceId);

            _Logger.Debug($"Monster {monster.instanceId} is attacking {currentTarget.name}");
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(currentTarget.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(currentTarget.instanceId);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(currentTarget.instanceId, damage);
            RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(currentTarget.hp.current);

            brList.Add(brStart);
            brList.Add(brHit);
            brList.Add(brHp);
            brList.Add(brEnd);
            server.router.Send(_map, brList);
            server.router.Send(_map.clientLookup.GetByCharacterInstanceId(currentTarget.instanceId),
                cHpUpdate.ToPacket());

            if (currentTarget.hp.depleted)
            {
                monster.SetAgro(false);
                monster.monsterAgroList.Remove(currentTarget.instanceId);
            }

            //PlayerDeadCheck(currentTarget);
        }

        private void MonsterCastQueue(int skillId)
        {
            _casting = true;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(monster.instanceId);
            RecvBattleReportActionMonsterSkillExec brExec = new RecvBattleReportActionMonsterSkillExec(skillId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            server.router.Send(_map, brList);
        }

        private void SendDataNotifyEoData(uint instanceId, int effectId)
        {
            Character currentTarget = monster.GetCurrentTarget();
            IBuffer res6 = BufferProvider.Provide();
            res6.WriteUInt32(instanceId);
            res6.WriteFloat(2.0F);
            //_server.Router.Send(Map, (ushort)AreaPacketId.recv_eo_base_notify_sphere, res6, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            //Logger.Debug($"Skill instance {instanceId} was just cast");
            res2.WriteUInt32(instanceId); // Unique Instance ID of Skill Cast
            res2.WriteFloat(currentTarget.x); //Effect Object X
            res2.WriteFloat(currentTarget.y); //Effect Object y
            res2.WriteFloat(currentTarget.z); //Effect Object z    (+100 just so i can see it better for now)

            //orientation related  (Note,  i believe at least 1 of these values must be above 0 for "arrows" to render"
            res2.WriteFloat(1); //Rotation Along X Axis if above 0
            res2.WriteFloat(1); //Rotation Along Y Axis if above 0
            res2.WriteFloat(1); //Rotation Along Z Axis if above 0

            res2.WriteInt32(effectId); // effect id
            res2.WriteUInt32(currentTarget
                .instanceId); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res2.WriteInt32(1); //unknown
            res2.WriteInt32(1);
            server.router.Send(_map, (ushort)AreaPacketId.recv_data_notify_eo_data, res2, ServerType.Area);
        }

        private void SendDataNotifyEoData2(uint instanceId, int effectId)
        {
            Character currentTarget = monster.GetCurrentTarget();
            IBuffer res2 = BufferProvider.Provide();
            _Logger.Debug($"Skill instance {instanceId} was just cast");
            res2.WriteUInt32(instanceId); // Unique Instance ID of Skill Cast
            res2.WriteInt32(effectId);
            res2.WriteFloat(currentTarget.x); //Effect Object X
            res2.WriteFloat(currentTarget.y); //Effect Object y
            res2.WriteFloat(currentTarget.z); //Effect Object z

            //orientation related
            res2.WriteFloat(0); //Rotation Along X Axis if above 0
            res2.WriteFloat(0); //Rotation Along Y Axis if above 0
            res2.WriteFloat(0); //Rotation Along Z Axis if above 0

            res2.WriteInt32(effectId); // effect id
            res2.WriteInt32(0); //must be set to int32 contents. int myTargetID = packet.Data.ReadInt32();
            res2.WriteInt32(0); //unknown

            res2.WriteInt32(0);
            res2.WriteInt32(0);
            res2.WriteInt32(0);
            res2.WriteInt32(0);
            server.router.Send(_map, (ushort)AreaPacketId.recv_data_notify_eo_data2, res2, ServerType.Area);
        }

        public void MonsterCastMove(uint instanceId, int castVelocity, byte pose, byte animation)
        {
            Character currentTarget = monster.GetCurrentTarget();
            Vector3 destPos = new Vector3(currentTarget.x, currentTarget.y, currentTarget.z);
            Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
            Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
            float distance = Vector3.Distance(monsterPos, destPos);
            float travelTime = distance / castVelocity;

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(instanceId); //Monster ID
            res.WriteFloat(monster.x);
            res.WriteFloat(monster.y);
            res.WriteFloat(monster.z + 75);
            res.WriteFloat(moveTo.X); //X per tick
            res.WriteFloat(moveTo.Y); //Y Per tick
            res.WriteFloat(moveTo.Z); //verticalMovementSpeedMultiplier

            res.WriteFloat(1 / travelTime); //movementMultiplier
            res.WriteFloat(travelTime); //Seconds to move

            res.WriteByte(pose); //MOVEMENT ANIM
            res.WriteByte(animation); //JUMP & FALLING ANIM
            server.router.Send(_map, (ushort)AreaPacketId.recv_0x8D92, res,
                ServerType.Area); //recv_0xE8B9  recv_0x1FC1
        }

        private void SendEoNotifyDisappearSchedule(uint instanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();

            res.WriteUInt32(instanceId);
            res.WriteFloat(3.0F);
            server.router.Send(_map, (ushort)AreaPacketId.recv_eo_notify_disappear_schedule, res, ServerType.Area);
        }

        private void SendBattleAttackPose()
        {
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteInt32(1410101);
            server.router.Send(_map, (ushort)AreaPacketId.recv_battle_attack_pose_r, res, ServerType.Area);
        }

        private void SendBattleAttackStart()
        {
            Character currentTarget = monster.GetCurrentTarget();
            IBuffer res = BufferProvider.Provide();
            res = BufferProvider.Provide();
            res.WriteUInt32(currentTarget.instanceId);
            server.router.Send(_map, (ushort)AreaPacketId.recv_battle_attack_start_r, res, ServerType.Area);
        }

        public void MonsterSpawn()
        {
            _Logger.Debug($"Monster {monster.name} instanceId [{monster.instanceId}]");
            monster.SetAgro(false);
            monsterMoving = false;
            _casting = false;
            _monsterWaiting = false;
            monster.SetGotoDistance(10);
            //monsterVelocity = 200;
            MonsterCoord spawnCoords = monster.monsterCoords.Find(x => x.coordIdx == 0);
            monster.x = spawnCoords.destination.X;
            monster.y = spawnCoords.destination.Y;
            monster.z = spawnCoords.destination.Z;
            monster.heading = (byte)GetHeading(monster.monsterCoords.Find(x => x.coordIdx == 1).destination);
            monster.hp.ToMax();
            _respawnTime = monster.respawnTime;
            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monster);
            foreach (NecClient client in _map.clientLookup.GetAll())
                if (client.character.hasDied == false)
                    server.router.Send(client, monsterData.ToPacket());
            _spawnMonster = false;
            monster.monsterVisible = true;
            monster.ClearAgroList();
            //MonsterBattlePose(false);
        }

        public bool MonsterCheck()
        {
            // Logger.Debug($"Monster HP [{_monster.GetHP()}]");
            if (monster.hp.current <= 0)
            {
                foreach (uint instanceId in monster.GetAgroInstanceList())
                {
                    monster.MonsterHate(server, false, instanceId);

                    NecClient client = server.clients.GetByCharacterInstanceId(instanceId);
                    client.character.experienceCurrent += monster.loot.experience;

                    IBuffer res = BufferProvider.Provide();
                    res.WriteUInt64(client.character.experienceCurrent);
                    res.WriteByte(0); //bool
                    server.router.Send(client, (ushort)AreaPacketId.recv_self_exp_notify, res, ServerType.Area); //This should go to the party of whomever did the most damage.  TODO

                    //To-Do,  make a variable to track union gold
                    client.character.adventureBagGold += monster.loot.gold; //Updates your Character.AdventureBagGold

                    res = BufferProvider.Provide();
                    res.WriteUInt64(client.character.adventureBagGold); // Sets your Adventure Bag Gold
                    server.router.Send(client, (ushort)AreaPacketId.recv_self_money_notify, res, ServerType.Area);
                }

                _Logger.Debug($"Monster is dead InstanceId [{monster.instanceId}]");
                //Death Animation
                List<PacketResponse> brList = new List<PacketResponse>();
                RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(monster.instanceId);
                RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
                RecvBattleReportNoactDead brDead = new RecvBattleReportNoactDead(monster.instanceId, 1);
                brList.Add(brStart);
                brList.Add(brDead);
                brList.Add(brEnd);
                server.router.Send(_map, brList);

                //Make the monster a lootable state
                IBuffer res10 = BufferProvider.Provide();
                res10.WriteUInt32(monster.instanceId);
                res10.WriteInt32(2); //Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing).
                server.router.Send(_map, (ushort)AreaPacketId.recv_monster_state_update_notify, res10,
                    ServerType.Area);

                Thread.Sleep(monster.respawnTime);
                //decompose the body
                IBuffer res7 = BufferProvider.Provide();
                res7.WriteUInt32(monster.instanceId);
                res7.WriteInt32(Util.GetRandomNumber(1, 5)); //4 here causes a cloud and the model to disappear, 5 causes a mist to happen and disappear
                res7.WriteInt32(1);
                server.router.Send(_map, (ushort)AreaPacketId.recv_charabody_notify_deadstate, res7, ServerType.Area);
                Thread.Sleep(2000);
                RecvObjectDisappearNotify objectDisappearData = new RecvObjectDisappearNotify(monster.instanceId);
                server.router.Send(_map, objectDisappearData);
                monster.monsterVisible = false;

                _spawnMonster = true;
                return true;
            }

            return false;
        }

        private void MonsterMove(MonsterCoord monsterCoord)
        {
            //ShowVectorInfo(_monster.X, _monster.Y, monsterCoord.destination.X, monsterCoord.destination.Y);
            //ShowMonsterInfo();
            if (!monsterMoving)
            {
                monsterMoving = true;
                OrientMonster();
                monster.MonsterMove(server, monster.monsterWalkVelocity, 2, 0);
            }
            else
            {
                Vector3 destPos = new Vector3(monsterCoord.destination.X, monsterCoord.destination.Y,
                    monsterCoord.destination.Z);
                Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                Vector3 moveTo = Vector3.Subtract(destPos, monsterPos);
                float distance = GetDistance(monsterPos, destPos);
                float travelTime = distance / monster.monsterWalkVelocity;
                int tickDivisor = 1000 / _updateTime;

                if (distance >= monster.monsterWalkVelocity / tickDivisor)
                {
                    monster.x = monster.x + moveTo.X / travelTime / tickDivisor;
                    monster.y = monster.y + moveTo.Y / travelTime / tickDivisor;
                    //_monster.Z = _monster.Z + (moveTo.Z / travelTime) / tickDivisor;
                }
                else
                {
                    monster.x = destPos.X;
                    monster.y = destPos.Y;
                    monster.z = destPos.Z;
                }

                //Logger.Debug($"distance [{distance}] travelTime[{travelTime}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
            }

            //            Logger.Debug($"distance [{distance}]");
            //            ShowMonsterInfo();
        }

        private void MonsterAgroMove()
        {
            float distance = 0;
            int tickDivisor = 1000 / _moveTime;
            Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);

            //ShowVectorInfo(_monster.X, _monster.Y, _monster.Z, currentDest.X, currentDest.Y, currentDest.Z);
            distance = GetDistance(monsterPos, _currentDest);
            Character currentTarget = monster.GetCurrentTarget();
            Vector3 targetPos = new Vector3(currentTarget.x, currentTarget.y, currentTarget.z);
            if (distance < monster.monsterRunVelocity / tickDivisor || _agroMoveTime >= 1000 || !monsterMoving)
            {
                _agroMoveTime = _agroTick;
                monsterMoving = true;
                if (!_casting && CheckHeading() == false)
                    OrientMonster();

                // Set destination to a position monsterGoto away from target
                float monsterGoto = monster.GetGotoDistance();
                Vector3 moveTo = Vector3.Subtract(targetPos, monsterPos);
                distance = GetDistance(monsterPos, targetPos);
                //Logger.Debug($"Target distance [{distance}] targetPos.X [{targetPos.X}] targetPos.Y [{targetPos.Y}] targetPos.Z [{targetPos.Z}]");
                float factor = (float)Math.Sqrt((monsterPos.X - targetPos.X) * (monsterPos.X - targetPos.X) +
                                                (monsterPos.Y - targetPos.Y) * (monsterPos.Y - targetPos.Y)) /
                               monsterGoto;
                _currentDest.Z = targetPos.Z;
                _currentDest.X = targetPos.X - moveTo.X / factor;
                _currentDest.Y = targetPos.Y - moveTo.Y / factor;
                moveTo = Vector3.Subtract(_currentDest, monsterPos);

                // Now do the move
                distance = GetDistance(monsterPos, _currentDest);
                //Logger.Debug($"Moving distance [{distance}] currentDest.X [{currentDest.X}] currentDest.Y [{currentDest.Y}] currentDest.Z [{currentDest.Z}]");
                if (distance <= monster.GetGotoDistance())
                    return;
                float travelTime = distance / monster.monsterRunVelocity;
                MonsterTick tick = new MonsterTick();
                tick.xTick = moveTo.X;
                tick.yTick = moveTo.Y;
                tick.zTick = moveTo.Z;
                _agroTickMove.xTick = moveTo.X / travelTime / tickDivisor;
                _agroTickMove.yTick = moveTo.Y / travelTime / tickDivisor;
                _agroTickMove.zTick = 0;
                //Logger.Debug($"Moving distance [{distance}] monsterVelocity [{_monster.MonsterRunVelocity}]  travelTime[{travelTime}] xTick [{tick.xTick}] yTick [{tick.yTick}] moveTo.X [{moveTo.X}] moveTo.Y [{moveTo.Y}] moveTo.Z [{moveTo.Z}]");
                monster.MonsterMove(server, 3, 0, tick, travelTime);
            }
            else
            {
                //float travelTime = (float)distance / _monster.MonsterRunVelocity;
                //Vector3 moveTo = Vector3.Subtract(currentDest, monsterPos);
                distance = GetDistance(monsterPos, _currentDest);
                if (distance >= monster.monsterRunVelocity / tickDivisor)
                {
                    monster.x = monster.x + _agroTickMove.xTick;
                    monster.y = monster.y + _agroTickMove.yTick;
                    //_monster.Z = _monster.Z + (moveTo.Z / travelTime) / tickDivisor;
                }
                else
                {
                    monster.x = _currentDest.X;
                    monster.y = _currentDest.Y;
                    monster.z = _currentDest.Z;
                }

                bool inMovePov = CheckFov(targetPos, _agroMoveAngle);
                if (!inMovePov && monsterMoving)
                {
                    Thread.Sleep(_updateTime);
                    monsterMoving = false;
                    monster.MonsterStop(server, 1, 0, 0.1F);
                    Thread.Sleep(100);
                    return;
                }

                _agroMoveTime += _agroTick;
            }
        }

        private bool MonsterAgroCheck()
        {
            List<NecClient> mapsClients = _map.clientLookup.GetAll();

            Vector3 monster = new Vector3(this.monster.x, this.monster.y, this.monster.z);
            foreach (NecClient client in mapsClients)
                if (client.character.hp.depleted == false)
                {
                    Vector3 character = new Vector3(client.character.x, client.character.y, client.character.z);
                    float distanceChar = GetDistance(character, monster);
                    if (distanceChar <= agroRange && !StealthCheck(client))
                    {
                        Vector3 characterPos = new Vector3(character.X, character.Y, character.Z);
                        if (CheckFov(characterPos, _agroDetectAngle))
                        {
                            this.monster.SetCurrentTarget(client.character);
                            _currentDest = new Vector3(client.character.x, client.character.y, client.character.z);
                            this.monster.SetAgro(true);
                            this.monster.AddAgroList(client.character.instanceId, 0);
                        }
                    }
                }

            return this.monster.GetAgro();
        }

        private bool StealthCheck(NecClient client)
        {
            // Needs to be expanded to consider skill, distance and orientation
            if (((uint)client.character.state & 0x100) > 0) return true;

            return false;
        }

        private void MonsterAgroAdjust()
        {
            Character currentTarget = monster.GetCurrentTarget();
            if (_agroCheckTime != -1 && _agroCheckTime < 3000)
            {
                _agroCheckTime += _updateTime;
                return;
            }

            uint currentInstance = monster.GetAgroHigh();
            if (currentTarget.instanceId != currentInstance) currentTarget = _map.clientLookup.GetByCharacterInstanceId(currentInstance).character;

            _agroCheckTime = 0;
        }

        private bool CheckFov(Vector3 target, float angle)
        {
            //Vector3 target = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            Vector3 source = new Vector3(monster.x, monster.y, monster.z);
            Vector3 targetVector = Vector3.Normalize(source - target);
            double sourceRadian = ConvertToRadians(monster.heading, true);
            Vector3 sourceVector = new Vector3((float)Math.Cos(sourceRadian), (float)Math.Sin(sourceRadian), 0);
            sourceVector = Vector3.Normalize(sourceVector);
            float dotProduct = Vector3.Dot(sourceVector, targetVector);
            //Logger.Debug($"sourceVector.X[{sourceVector.X}] sourceVector.Y[{sourceVector.Y}]");
            //Logger.Debug($"Monster {_monster.Name} heading [{_monster.Heading}] dotProduct [{dotProduct}] fovAngle [{angle}]");
            //if (dotProduct > angle)
            //Logger.Debug($"Target is in FOV of {_monster.Name}!!");
            //else
            //Logger.Debug($"Monster {_monster.Name} is oblivious dotProduct [{dotProduct}] fovAngle [{agroDetectAngle}]");
            return dotProduct > angle;
        }

        private double ConvertToRadians(double angle, bool adjust)
        {
            angle = angle * 2;
            if (adjust)
                angle = angle <= 90 ? angle + 270 : angle - 90;
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return Math.PI / 180 * angle;
        }

        private void OrientMonster()
        {
            if (monster.GetAgro())
                AdjustHeading();

            monster.MonsterOrient(server);
        }

        private float GetDistance(Vector3 target, Vector3 source)
        {
            return Vector3.Distance(target, source);
        }

        private void ShowVectorInfo(double targetX, double targetY, double targetZ, double objectX, double objectY,
            double objectZ)
        {
            Vector3 target = new Vector3((float)targetX, (float)targetY, (float)targetZ);
            Vector3 source = new Vector3((float)objectX, (float)objectY, (float)objectZ);
            Vector3 moveTo = Vector3.Subtract(target, source);
            float distance = Vector3.Distance(target, source);
            double dx = objectX - targetX;
            double dy = objectY - targetY;
            double dz = objectZ - targetZ;
            _Logger.Debug(
                $"dx [{dx}]   dy[{dy}]  dz[{dz}] distance [{distance}] moveTo.X [{moveTo.X}]  moveTo.Y [{moveTo.Y}]  moveTo.Z [{moveTo.Z}]");
        }

        private void ShowMonsterInfo()
        {
            _Logger.Debug(
                $"monster [{monster.name}]    X[{monster.x}]   Y [{monster.y}] monster.Z [{monster.z}]  Heading [{monster.heading}]");
        }

        private double GetHeading(Vector3 destination) // Will return heading for x2/y2 object to look at x1/y1 object
        {
            double dx = monster.x - destination.X;
            double dy = monster.y - destination.Y;
            double direction = Math.Atan2(dy, dx) / Math.PI * 180f;
            ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return direction;
        }

        private void AdjustHeading()
        {
            Character currentTarget = monster.GetCurrentTarget();
            double dx = monster.x - currentTarget.x;
            double dy = monster.y - currentTarget.y;
            double direction = Math.Atan2(dy, dx) / Math.PI * 180f;
            ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"New direction [{direction}]");
            monster.heading = (byte)direction;
        }

        private bool CheckHeading() // Will return heading for x2/y2 object to look at x1/y1 object
        {
            Character currentTarget = monster.GetCurrentTarget();
            double dx = monster.x - currentTarget.x;
            double dy = monster.y - currentTarget.y;
            double direction = Math.Atan2(dy, dx) / Math.PI * 180f;
            ;
            if (direction < 0) direction += 360f;
            direction = direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            //Logger.Debug($"direction after [{direction}]");
            return monster.heading == (byte)direction;
        }

        private void SendBattleReportStartNotify()
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(monster.instanceId);
            server.router.Send(_map, (ushort)AreaPacketId.recv_battle_report_start_notify, res, ServerType.Area);
        }

        private void SendBattleReportEndNotify()
        {
            IBuffer res = BufferProvider.Provide();
            server.router.Send(_map, (ushort)AreaPacketId.recv_battle_report_end_notify, res, ServerType.Area);
        }
    }
}
