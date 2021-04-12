using System;
using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Packet.Area
{
    public class SendBattleAttackExec : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendBattleAttackExec));

        private readonly NecServer _server;

        public SendBattleAttackExec(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort)AreaPacketId.send_battle_attack_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int unknown1 = packet.data.ReadInt32();
            uint instanceId = packet.data.ReadUInt32();
            int unknown2 = packet.data.ReadInt32();

            client.character.eventSelectReadyCode = instanceId;
            _Logger.Debug($"Just attacked Target {client.character.eventSelectReadyCode}");


            int damage = client.character.battleParam.plusPhysicalAttack;
            int seed = Util.GetRandomNumber(0, 20);
            if (seed < 2)
                damage += Util.GetRandomNumber(1, 4); // Light hit
            else if (seed < 19)
                damage += Util.GetRandomNumber(16, 24); // Normal hit
            else
                damage *= 2;
            damage += Util.GetRandomNumber(32, 48); // Critical hit
            if (client.account.state == AccountStateType.Admin) damage *= 100; //testing death and revival is slow with low dmg.

            AttackObjectsInRange(client, damage);


            SendBattleAttackExecR(client);
        }

        //What do the other _r  recvs do?
        private void SendBattleAttackExecR(NecClient client)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // If not zero next attack is allowed before first complete
            router.Send(client, (ushort)AreaPacketId.recv_battle_attack_exec_r, res, ServerType.Area);
        }

        private static double Distance(double targetX, double targetY, double objectX, double objectY)
        {
            // Calculating distance
            return Math.Sqrt(Math.Pow(objectX - targetX, 2) +
                             Math.Pow(objectY - targetY, 2) * 1.0);
        }

        private void AttackObjectsInRange(NecClient client, int damage)
        {
            float perHp = 100.0f;

            //Damage Players in range
            foreach (NecClient targetClient in client.map.clientLookup.GetAll())
            {
                if (targetClient == client) continue; //skip damaging yourself
                if (targetClient.character.partyId == client.character.partyId && client.character.partyId != 0) continue; //skip damaging party members
                //if (targetClient.Soul.CriminalLevel == 0 && client.CriminalOnlyDamage == true) continue; //skip attacking non criminal players.  TODO

                double distanceToCharacter = Distance(targetClient.character.x, targetClient.character.y,
                    client.character.x, client.character.y);
                _Logger.Debug(
                    $"target Character name [{targetClient.character.name}] distanceToCharacter [{distanceToCharacter}] Radius { /*[{monsterSpawn.Radius}]*/"125"} {targetClient.character.name}");
                if (distanceToCharacter > /*targetClient.Character.Radius +*/ 125) continue;

                if (targetClient.character.hp.depleted) continue;

                damage -= targetClient.character.battleParam.plusPhysicalDefence;
                if (damage < 0) damage = 1; //pity damage

                targetClient.character.hp.Modify(-damage, client.character.instanceId);
                perHp = (float)targetClient.character.hp.current / targetClient.character.hp.max * 100;
                _Logger.Debug(
                    $"CurrentHp [{targetClient.character.hp.current}] MaxHp[{targetClient.character.hp.max}] perHp[{perHp}]");
                RecvCharaUpdateHp cHpUpdate = new RecvCharaUpdateHp(targetClient.character.hp.current);
                _server.router.Send(targetClient, cHpUpdate.ToPacket());

                //logic to turn characters to criminals on criminal actions.  possibly should move to character task.
                client.character.criminalState += 1;
                if ((client.character.criminalState == 1) | (client.character.criminalState == 2) |
                    (client.character.criminalState == 3))
                {
                    IBuffer res40 = BufferProvider.Provide();
                    res40.WriteUInt32(client.character.instanceId);
                    res40.WriteByte(client.character.criminalState);

                    _Logger.Debug($"Setting crime level for Character {client.character.name} to {client.character.criminalState}");
                    router.Send(client.map, (ushort)AreaPacketId.recv_chara_update_notify_crime_lv, res40, ServerType.Area);
                    //Router.Send(client.Map, (ushort) AreaPacketId.recv_charabody_notify_crime_lv, res40, ServerType.Area, client);
                }

                if (client.character.criminalState > 255) client.character.criminalState = 255;

                DamageTheObject(client, targetClient.character.instanceId, damage, perHp);
            }

            //Damage Monsters in range
            foreach (MonsterSpawn monsterSpawn in client.map.monsterSpawns.Values)
            {
                double distanceToObject =
                    Distance(monsterSpawn.x, monsterSpawn.y, client.character.x, client.character.y);
                _Logger.Debug(
                    $"target Monster name [{monsterSpawn.name}] distanceToObject [{distanceToObject}] Radius [{monsterSpawn.radius}] {monsterSpawn.name}");
                if (distanceToObject > monsterSpawn.radius * 5
                ) //increased hitbox for monsters by a factor of 5.  Beetle radius is 40
                    continue;

                if (monsterSpawn.hp.depleted) continue;

                monsterSpawn.hp.Modify(-damage, client.character.instanceId);
                perHp = (float)monsterSpawn.hp.current / monsterSpawn.hp.max * 100;
                _Logger.Debug($"CurrentHp [{monsterSpawn.hp.current}] MaxHp[{monsterSpawn.hp.max}] perHp[{perHp}]");

                //just for fun. turn on inactive monsters
                if (monsterSpawn.active == false)
                {
                    monsterSpawn.active = true;
                    monsterSpawn.spawnActive = true;
                    if (!monsterSpawn.taskActive)
                    {
                        MonsterTask monsterTask = new MonsterTask(_server, monsterSpawn);
                        if (monsterSpawn.defaultCoords)
                            monsterTask.monsterHome = monsterSpawn.monsterCoords[0];
                        else
                            monsterTask.monsterHome = monsterSpawn.monsterCoords.Find(x => x.coordIdx == 64);
                        monsterTask.Start();
                    }
                }

                DamageTheObject(client, monsterSpawn.instanceId, damage, perHp);
            }

            //Damage NPCs in range
            foreach (NpcSpawn npcSpawn in client.map.npcSpawns.Values)
            {
                double distanceToObject = Distance(npcSpawn.x, npcSpawn.y, client.character.x, client.character.y);
                _Logger.Debug(
                    $"target NPC name [{npcSpawn.name}] distanceToObject [{distanceToObject}] Radius [{npcSpawn.radius}] {npcSpawn.name}");
                if (distanceToObject > npcSpawn.radius) continue;

                DamageTheObject(client, npcSpawn.instanceId, damage, perHp);
            }
        }

        private void DamageTheObject(NecClient client, uint instanceId, int damage, float perHp)
        {
            List<PacketResponse> brTargetList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionAttackExec
                brAttack = new RecvBattleReportActionAttackExec(
                    (int)instanceId); //should this be the instance ID of the attacker? we have it marked as skillId
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(instanceId);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(instanceId, damage);
            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(instanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(instanceId, perHp);
            RecvBattleReportNotifyKnockback brKnockBack = new RecvBattleReportNotifyKnockback(instanceId, 001, 001);

            brTargetList.Add(brStart);
            brTargetList.Add(brAttack);
            brTargetList.Add(brHit);
            //brTargetList.Add(brPhyHp);
            brTargetList.Add(brHp);
            brTargetList.Add(oHpUpdate);
            //brTargetList.Add(brKnockBack); //knockback doesn't look right here. need to make it better.
            brTargetList.Add(brEnd);
            router.Send(client.map, brTargetList);
        }

        //To be implemented for monsters
        private void SendBattleReportKnockBack(NecClient client, IInstance instance)
        {
            MonsterSpawn monster = (MonsterSpawn)instance;
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(monster.instanceId);
            res.WriteFloat(0);
            res.WriteFloat(2); // delay in seconds
            router.Send(client.map, (ushort)AreaPacketId.recv_battle_report_noact_notify_knockback, res,
                ServerType.Area);
        }
    }
}
