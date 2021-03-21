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
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(TrapTask));

        private readonly object TrapLock = new object();
        protected NecServer _server { get; }
        private List<Trap> TrapList;
        private List<MonsterSpawn> MonsterList;
        public Vector3 TrapPos { get; }
        private DateTime expireTime;
        private Map _map;
        public uint ownerInstanceId { get; }
        public uint stackInstanceId { get; }
        private int triggerRadius;
        private int detectRadius;
        private int detectHeight;
        private int tickTime;
        private bool triggered;
        private bool trapActive;

        public TrapTask(NecServer server, Map map, Vector3 trapPos, uint instanceId, Trap trap, uint stackId)
        {
            _server = server;
            TrapList = new List<Trap>();
            MonsterList = new List<MonsterSpawn>();
            TrapPos = trapPos;
            TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, (int) trap._trapTime * 1000);
            expireTime = DateTime.Now + activeTime;
            _map = map;
            triggerRadius = trap._triggerRadius;
            detectHeight = 25;
            detectRadius = 1000;
            tickTime = 400;
            triggered = false;
            ownerInstanceId = instanceId;
            stackInstanceId = stackId;
            trapActive = true;
            Logger.Debug($"trap._trapTime [{trap._trapTime}]");
        }

        public override string TaskName => $"TrapTask {ownerInstanceId}";
        public override TimeSpan TaskTimeSpan { get; }
        protected override bool TaskRunAtStart => false;


        protected override void Execute()
        {
            while (expireTime > DateTime.Now && trapActive)
            {
                List<MonsterSpawn> monsters = _map.GetMonstersRange(TrapPos, detectRadius);
                if (triggered)
                {
                    Trap trap = TrapList[0];
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                        Logger.Debug(
                            $"Enhancement monster.InstanceId [{monster.InstanceId}] trap._name [{trap._name}] trap._effectRadius [{trap._effectRadius}]");
                        if (Vector3.Distance(monsterPos, TrapPos) <= trap._effectRadius)
                        {
                            TriggerTrap(trap, monster);
                        }
                    }

                    TrapList.Remove(trap);
                    if (TrapList.Count == 0)
                        trapActive = false;
                }
                else if (monsters.Count > 0)
                {
                    foreach (MonsterSpawn monster in monsters)
                    {
                        Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                        if ((Vector3.Distance(monsterPos, TrapPos) <= triggerRadius) &&
                            (monsterPos.Z - TrapPos.Z <= detectHeight))
                        {
                            triggered = true;
                            break;
                        }
                    }

                    if (!triggered)
                        tickTime = 50;
                    else
                    {
                        tickTime = 500;
                        Trap trap = TrapList[0];
                        foreach (MonsterSpawn monster in monsters)
                        {
                            Vector3 monsterPos = new Vector3(monster.X, monster.Y, monster.Z);
                            Logger.Debug(
                                $"Base trap monster.InstanceId [{monster.InstanceId}] trap._name [{trap._name}] trap._effectRadius [{trap._effectRadius}]");
                            if (Vector3.Distance(monsterPos, TrapPos) <= trap._effectRadius)
                            {
                                MonsterList.Add(monster);
                                TriggerTrap(trap, monster);
                            }
                        }

                        TrapList.Remove(trap);
                        if (TrapList.Count == 0)
                            trapActive = false;
                    }
                }
                else
                    tickTime = 400;

                Thread.Sleep(tickTime);
            }

            foreach (Trap trap in TrapList)
            {
                RecvDataNotifyEoData eoDestroyData =
                    new RecvDataNotifyEoData(trap.InstanceId, trap.InstanceId, 0, TrapPos, 0, 0);
                _server.Router.Send(_map, eoDestroyData);
                RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(trap.InstanceId, 0.0F);
                _server.Router.Send(_map, eoDisappear);
            }

            TrapList.Clear();
            _map.RemoveTrap(stackInstanceId);
            this.Stop();
        }

        public void TriggerTrap(Trap trap, MonsterSpawn monster)
        {
            Logger.Debug(
                $"trap._name [{trap._name}] trap.InstanceId [{trap.InstanceId}] trap._skillEffectId [{trap._skillEffectId}] trap._triggerEffectId [{trap._triggerEffectId}]");
            NecClient client = _map.ClientLookup.GetByCharacterInstanceId(ownerInstanceId);
            if (client.Character.IsStealthed())
            {
                client.Character.ClearStateBit(CharacterState.StealthForm);
                RecvCharaNotifyStateflag charState =
                    new RecvCharaNotifyStateflag(client.Character.InstanceId, (uint) client.Character.State);
                _server.Router.Send(client.Map, charState);
            }

            int damage = Util.GetRandomNumber(70, 90);
            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(trap.InstanceId, monster.InstanceId,
                trap._triggerEffectId, TrapPos, 2, 2);
            _server.Router.Send(_map, eoTriggerData);
            float perHp = (((float) monster.Hp.current / (float) monster.Hp.max) * 100);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(ownerInstanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec brAttack = new RecvBattleReportActionAttackExec(trap._skillId);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(monster.InstanceId);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(monster.InstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(monster.InstanceId, perHp);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(monster.InstanceId, damage);

            brList.Add(brStart);
            //brList.Add(brAttack);
            brList.Add(brHit);
            //brList.Add(brPhyHp);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brEnd);
            _server.Router.Send(_map, brList);
            if (monster.GetAgroCharacter(ownerInstanceId))
            {
                monster.UpdateHP(-damage);
            }
            else
            {
                monster.UpdateHP(-damage, _server, true, ownerInstanceId);
            }
        }

        public void AddTrap(Trap trap)
        {
            lock (TrapLock)
            {
                TrapList.Add(trap);
            }
        }

        public void UpdateTrapTime(int activeTimeMs)
        {
            lock (TrapLock)
            {
                TimeSpan activeTime = new TimeSpan(0, 0, 0, 0, activeTimeMs);
                expireTime = DateTime.Now + activeTime;
            }
        }

        private double ConvertToRadians(double angle, bool adjust)
        {
            angle = angle * 2;
            if (adjust)
                angle = (angle <= 90 ? angle + 270 : angle - 90);
            //direction < 270 ? (direction + 90) / 2 : (direction - 270) / 2;
            return (Math.PI / 180) * angle;
        }

        public static bool baseTrap(int skillId)
        {
            return baseTraps.Contains(skillId);
        }

        private static int[] baseTraps = {14301, 14302};
        private static int[] trapEnhancements = { };
    }
}
