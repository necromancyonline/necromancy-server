using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Model.Skills
{
    public class ThiefSkill : IInstance
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(ThiefSkill));

        private readonly NecClient _client;
        private readonly NecServer _server;
        private readonly int _skillId;
        private readonly uint _targetInstanceId;

        public ThiefSkill(NecServer server, NecClient client, int skillId, uint targetInstanceId)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _targetInstanceId = targetInstanceId;
        }

        public uint instanceId { get; set; }

        public void StartCast()
        {
            IInstance target = _server.instances.GetInstance(_targetInstanceId);
            switch (target) // ToDO     Do a hositilty check to make sure this is allowed
            {
                case NpcSpawn npcSpawn:
                    _Logger.Debug(
                        $"Start casting Skill [{_skillId}] on NPCId: {npcSpawn.instanceId} SerialId: {npcSpawn.npcId}");
                    break;
                case MonsterSpawn monsterSpawn:
                    _Logger.Debug($"Start casting Skill [{_skillId}] on MonsterId: {monsterSpawn.instanceId}");
                    break;
                case Character character:
                    _Logger.Debug($"Start casting Skill [{_skillId}] on CharacterId: {character.instanceId}");
                    break;
                default:
                    _Logger.Error(
                        $"Instance with InstanceId: {_targetInstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.settingRepository.skillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            float castTime = skillBaseSetting.castingTime;
            _Logger.Debug($"Start casting Skill [{_skillId}] cast time is [{castTime}]");
            RecvSkillStartCastR thiefSkill = new RecvSkillStartCastR(0, castTime);
            _server.router.Send(thiefSkill, _client);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(_skillId);
            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
        }

        public void SkillExec()
        {
            Vector3 trgCoord = new Vector3();
            NpcSpawn npcSpawn = null;
            MonsterSpawn monsterSpawn = null;
            Character character = null;
            IInstance target = _server.instances.GetInstance(_targetInstanceId);
            switch (target)
            {
                case NpcSpawn npc:
                    npcSpawn = npc;
                    _Logger.Debug(
                        $"NPCId: {npcSpawn.instanceId} is gettin blasted by Skill Effect {_client.character.skillStartCast}");
                    trgCoord.X = npcSpawn.x;
                    trgCoord.Y = npcSpawn.y;
                    trgCoord.Z = npcSpawn.z;
                    break;
                case MonsterSpawn monster:
                    monsterSpawn = monster;
                    _Logger.Debug(
                        $"MonsterId: {monsterSpawn.instanceId} is gettin blasted by Skill Effect {_client.character.skillStartCast}");
                    trgCoord.X = monsterSpawn.x;
                    trgCoord.Y = monsterSpawn.y;
                    trgCoord.Z = monsterSpawn.z;

                    break;
                case Character chara:
                    character = chara;
                    _Logger.Debug(
                        $"CharacterId: {character.instanceId} is gettin blasted by Skill Effect {_client.character.skillStartCast}");
                    trgCoord.X = character.x;
                    trgCoord.Y = character.y;
                    trgCoord.Z = character.z;
                    break;
                default:
                    _Logger.Error(
                        $"Instance with InstanceId: {_targetInstanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.settingRepository.skillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            if (!int.TryParse($"{_skillId}".Substring(1, 6) + 1, out int effectId)) _Logger.Error($"Creating effectId from skillid [{_skillId}]");

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec =
                new RecvBattleReportActionSkillExec(_client.character.skillStartCast);
            RecvBattleReportActionEffectOnHit brEof = new RecvBattleReportActionEffectOnHit(600021);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEof);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
            brList.Clear();
            trgCoord.Z += 10;
            _Logger.Debug($"skillid [{_skillId}] effectId [{effectId}]");

            RecvDataNotifyEoData eoData =
                new RecvDataNotifyEoData(instanceId, _targetInstanceId, effectId, trgCoord, 2, 2);
            //_server.Router.Send(_client.Map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(instanceId, 2.0F);
            //_server.Router.Send(_client.Map, eoDisappear);

            //Vector3 _srcCoord  = new Vector3(_client.Character.X, _client.Character.Y, _client.Character.Z);
            //Recv8D92 effectMove = new Recv8D92(_srcCoord, trgCoord, InstanceId, _client.Character.skillStartCast, 3000, 2, 2);  // ToDo need real velocities
            //_server.Router.Send(_client.Map, effectMove);

            int damage = Util.GetRandomNumber(70, 90);
            //RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(_client.Character.InstanceId, monsterSpawn.InstanceId, effectId, _srcCoord, 2, 2);
            //_server.Router.Send(_client.Map, eoTriggerData);
            int monsterHp = monsterSpawn.Hp.current;
            List<PacketResponse> brList2 = new List<PacketResponse>();
            float perHp = monsterHp > 0 ? monsterHp / (float)monsterSpawn.Hp.max * 100 : 0;
            RecvBattleReportStartNotify brStart1 = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd1 = new RecvBattleReportEndNotify();
            //RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(monsterSpawn.InstanceId, damage);
            RecvBattleReportPhyDamageHp brPhyHp = new RecvBattleReportPhyDamageHp(monsterSpawn.instanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(monsterSpawn.instanceId, perHp);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(monsterSpawn.instanceId);

            brList2.Add(brStart1);
            //brList2.Add(brHp);
            brList2.Add(brPhyHp);
            brList2.Add(oHpUpdate);
            brList2.Add(brHit);
            brList2.Add(brEnd1);
            //brList.Add(oHpUpdate);
            _server.router.Send(_client.map, brList2);
            //if (monsterSpawn.GetAgroCharacter(_client.Character.InstanceId))
            //{
            // monsterSpawn.UpdateHP(-damage);
            //}
            //else
            //{
            //monsterSpawn.UpdateHP(-damage, _server, true, _client.Character.InstanceId);
            //}
            _Logger.Debug($"{monsterSpawn.name} has {monsterSpawn.Hp.current} HP left.");
        }
    }
}
