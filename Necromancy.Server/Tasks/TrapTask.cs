using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks.Core;

namespace Necromancy.Server.Tasks
{
    public class TrapTask : PeriodicTask
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(TrapTask));

        private static readonly int[] _baseTraps = {14301, 14302};
        private static int[] _trapEnhancements = { };
        private readonly int _detectHeight;
        private readonly int _detectRadius;
        private readonly Map _map;
        private readonly List<MonsterSpawn> _monsterList;
        private readonly List<Trap> _trapList;

        private readonly object _trapLock = new object();
        private readonly int _triggerRadius;
        private DateTime _expireTime;
        private int _tickTime;
        private bool _trapActive;
        private bool _triggered;

        public TrapTask(NecServer server, Map map, Vector3 trapPos, uint instanceId, Trap trap, uint stackId)
        {
            this.server = server;
            _trapList = new List<Trap>();
            _monsterList = new List<MonsterSpawn>();
            this.trapPos = trapPos;
            TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, (int)trap.trapTime * 1000);
            _expireTime = DateTime.Now + activeTime;
            _map = map;
            _triggerRadius = trap.triggerRadius;
            _detectHeight = 25;
            _detectRadius = 1000;
            _tickTime = 400;
            _triggered = false;
            ownerInstanceId = instanceId;
            stackInstanceId = stackId;
            _trapActive = true;
            _Logger.Debug($"trap._trapTime [{trap.trapTime}]");
        }

        protected NecServer server { get; }
        public Vector3 trapPos { get; }
        public uint ownerInstanceId { get; }
        public uint stackInstanceId { get; }

        public override string taskName => $"TrapTask {ownerInstanceId}";
        public override TimeSpan taskTimeSpan { get; }
        protected override bool taskRunAtStart => false;


        protected override void Execute()
        {
            while (_expireTime > DateTime.Now && _trapActive)
            {
                List<MonsterSpawn> monsters = _map.GetMonstersRange(trapPos, _detectRadius);
                if (_triggered)
                {
                    Trap trap = _trapList[0];
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                        _Logger.Debug(
                            $"Enhancement monster.InstanceId [{monster.instanceId}] trap._name [{trap.name}] trap._effectRadius [{trap.effectRadius}]");
                        if (Vector3.Distance(monsterPos, trapPos) <= trap.effectRadius) TriggerTrap(trap, monster);
                    }

                    _trapList.Remove(trap);
                    if (_trapList.Count == 0)
                        _trapActive = false;
                }
                else if (monsters.Count > 0)
                {
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                        if (Vector3.Distance(monsterPos, trapPos) <= _triggerRadius &&
                            monsterPos.Z - trapPos.Z <= _detectHeight)
                        {
                            _triggered = true;
                            break;
                        }
                    }

                    if (!_triggered)
                    {
                        _tickTime = 50;
                    }
                    else
                    {
                        _tickTime = 500;
                        Trap trap = _trapList[0];
                        foreach (MonsterSpawn monster in monsters)
                        {
                            Vector3 monsterPos = new Vector3(monster.x, monster.y, monster.z);
                            _Logger.Debug(
                                $"Base trap monster.InstanceId [{monster.instanceId}] trap._name [{trap.name}] trap._effectRadius [{trap.effectRadius}]");
                            if (Vector3.Distance(monsterPos, trapPos) <= trap.effectRadius)
                            {
                                _monsterList.Add(monster);
                                TriggerTrap(trap, monster);
                            }
                        }

                        _trapList.Remove(trap);
                        if (_trapList.Count == 0)
                            _trapActive = false;
                    }
                }
                else
                {
                    _tickTime = 400;
                }

                Thread.Sleep(_tickTime);
            }

            foreach (Trap trap in _trapList)
            {
                RecvDataNotifyEoData eoDestroyData =
                    new RecvDataNotifyEoData(trap.instanceId, trap.instanceId, 0, trapPos, 0, 0);
                server.router.Send(_map, eoDestroyData);
                RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(trap.instanceId, 0.0F);
                server.router.Send(_map, eoDisappear);
            }

            _trapList.Clear();
            _map.RemoveTrap(stackInstanceId);
            Stop();
        }

        public void TriggerTrap(Trap trap, MonsterSpawn monster)
        {
            _Logger.Debug(
                $"trap._name [{trap.name}] trap.InstanceId [{trap.instanceId}] trap._skillEffectId [{trap.skillEffectId}] trap._triggerEffectId [{trap.triggerEffectId}]");
            NecClient client = _map.clientLookup.GetByCharacterInstanceId(ownerInstanceId);
            if (client.character.IsStealthed())
            {
                client.character.ClearStateBit(CharacterState.StealthForm);
                RecvCharaNotifyStateflag charState =
                    new RecvCharaNotifyStateflag(client.character.instanceId, (uint)client.character.state);
                server.router.Send(client.map, charState);
            }

            int damage = Util.GetRandomNumber(70, 90);
            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(trap.instanceId, monster.instanceId,
                trap.triggerEffectId, trapPos, 2, 2);
            server.router.Send(_map, eoTriggerData);
            float perHp = monster.hp.current / (float)monster.hp.max * 100;
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(ownerInstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec(trap.skillId);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(monster.instanceId);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(monster.instanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(monster.instanceId, perHp);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(monster.instanceId, damage);

            brList.Add(brStart);
            //brList.Add(brAttack);
            brList.Add(brHit);
            //brList.Add(brPhyHp);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brEnd);
            server.router.Send(_map, brList);
            if (monster.GetAgroCharacter(ownerInstanceId))
                monster.UpdateHp(-damage);
            else
                monster.UpdateHp(-damage, server, true, ownerInstanceId);
        }

        public void AddTrap(Trap trap)
        {
            lock (_trapLock)
            {
                _trapList.Add(trap);
            }
        }

        public void UpdateTrapTime(int activeTimeMs)
        {
            lock (_trapLock)
            {
                TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, activeTimeMs);
                _expireTime = DateTime.Now + activeTime;
            }
        }

        private double ConvertToRadians(double angle, bool adjust)
        {
            angle = angle * 2;
            if (adjust)
                angle = angle <= 90 ? angle + 270 : angle - 90;
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return Math.PI / 180 * angle;
        }

        public static bool BaseTrap(int skillId)
        {
            return _baseTraps.Contains(skillId);
        }
    }
}
