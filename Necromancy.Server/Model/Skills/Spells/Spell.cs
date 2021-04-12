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
    public class Spell : IInstance
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(Spell));

        private readonly NecClient _client;
        private readonly NecServer _server;
        private readonly int _skillId;
        private Vector3 _srcCoord;
        private uint _targetInstanceId;

        public Spell(NecServer server, NecClient client, int skillId, uint targetInstanceId, Vector3 srcCoord)
        {
            _server = server;
            _client = client;
            _skillId = skillId;
            _targetInstanceId = targetInstanceId;
            _srcCoord = srcCoord;
        }

        public uint instanceId { get; set; }

        public void StartCast()
        {
            if (_targetInstanceId == 0) _targetInstanceId = _client.character.instanceId;
            IInstance target = _server.instances.GetInstance(_targetInstanceId);
            switch (target) // ToDO     Do a hositilty check to make sure this is allowed
            {
                case NpcSpawn npcSpawn:
                    _Logger.Debug($"Start casting Skill [{_skillId}] on NPCId: {npcSpawn.instanceId}");
                    break;
                case MonsterSpawn monsterSpawn:
                    _Logger.Debug($"Start casting Skill [{_skillId}] on MonsterId: {monsterSpawn.instanceId}");
                    break;
                case Character character:
                    _Logger.Debug($"Start casting Skill [{_skillId}] on CharacterId: {character.instanceId}");
                    break;
                default:
                    _Logger.Error(
                        $"Instance with InstanceId: {target.instanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.settingRepository.skillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            float castTime = skillBaseSetting.castingTime;
            _Logger.Debug($"Start casting Skill [{_skillId}] cast time is [{castTime}]");
            RecvSkillStartCastR spell = new RecvSkillStartCastR(0, castTime);
            _server.router.Send(spell, _client);
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
            float perHp = 0;
            int damage = Util.GetRandomNumber(70, 90);
            IInstance target = _server.instances.GetInstance(_targetInstanceId);
            switch (target)
            {
                case NpcSpawn npc:
                    npcSpawn = npc;
                    _Logger.Debug(
                        $"NPCId: {npcSpawn.instanceId} SerialId: {npcSpawn.id} is gettin blasted by Skill Effect {_client.character.skillStartCast}");
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
                    int monsterHp = monsterSpawn.hp.current;
                    perHp = monsterHp > 0 ? monsterHp / (float)monsterSpawn.hp.max * 100 : 0;
                    if (monsterSpawn.GetAgroCharacter(_client.character.instanceId))
                        monsterSpawn.UpdateHp(-damage);
                    else
                        monsterSpawn.UpdateHp(-damage, _server, true, _client.character.instanceId);

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
                        $"Instance with InstanceId: {target.instanceId} does not exist.  the ground is gettin blasted");
                    break;
            }

            if (!_server.settingRepository.skillBase.TryGetValue(_skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Could not get SkillBaseSetting for skillId : {_skillId}");
                return;
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec =
                new RecvBattleReportActionSkillExec(_client.character.skillStartCast);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
            if (!int.TryParse($"{_skillId}".Substring(1, 6) + 1, out int effectId)) _Logger.Error($"Creating effectId from skillid [{_skillId}]");

            trgCoord.Z += 10;
            _Logger.Debug($"skillid [{_skillId}] effectId [{effectId}]");
            RecvDataNotifyEoData eoData =
                new RecvDataNotifyEoData(instanceId, _targetInstanceId, effectId, trgCoord, 2, 2);
            _server.router.Send(_client.map, eoData);
            RecvEoNotifyDisappearSchedule eoDisappear = new RecvEoNotifyDisappearSchedule(instanceId, 2.0F);
            _server.router.Send(_client.map, eoDisappear);

            Vector3 srcCoord = new Vector3(_client.character.x, _client.character.y, _client.character.z);
            Recv8D92 effectMove = new Recv8D92(srcCoord, trgCoord, instanceId, _client.character.skillStartCast, 3000,
                2, 2); // ToDo need real velocities
            _server.router.Send(_client.map, effectMove);

            RecvDataNotifyEoData eoTriggerData = new RecvDataNotifyEoData(_client.character.instanceId,
                _targetInstanceId, effectId, srcCoord, 2, 2);
            _server.router.Send(_client.map, eoTriggerData);

            RecvBattleReportDamageHp brHp = new RecvBattleReportDamageHp(_targetInstanceId, damage);
            RecvObjectHpPerUpdateNotify oHpUpdate = new RecvObjectHpPerUpdateNotify(_targetInstanceId, perHp);
            RecvBattleReportNotifyHitEffect brHit = new RecvBattleReportNotifyHitEffect(_targetInstanceId);

            brList.Add(brStart);
            brList.Add(brHp);
            brList.Add(oHpUpdate);
            brList.Add(brHit);
            brList.Add(brEnd);
            brList.Add(oHpUpdate);
            _server.router.Send(_client.map, brList);
        }
    }
}
