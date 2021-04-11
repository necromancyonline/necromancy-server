using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.CharacterModel;

namespace Necromancy.Server.Packet.Area
{
    public class SendSkillStartCast : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSkillStartCast));

        private NecServer _server;

        public SendSkillStartCast(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillId = packet.data.ReadInt32();
            uint skillTarget = packet.data.ReadUInt32();
            client.character.eventSelectReadyCode = skillTarget;
            client.character.skillStartCast = skillId;
            int skillLookup = skillId / 1000;
            _Logger.Debug($"skillTarget [{skillTarget}]  skillID [{skillId}] skillLookup [{skillLookup}]");
            {
                var eventSwitchPerObjectId = new Dictionary<Func<int, bool>, Action>
                {
                    {x => (x > 114100 && x < 114199), () => ThiefSkill(client, skillId, skillTarget)},
                    {x => (x > 114300 && x < 114399), () => ThiefSkill(client, skillId, skillTarget)},
                    {x => (x > 113000 && x < 113999), () => MageSkill(client, skillId, skillTarget)},
                    {x => x == 114607, () => ThiefSkill(client, skillId, skillTarget)},
                    {
                        x => (x > 1 && x < 999999), () => MageSkill(client, skillId, skillTarget)
                    } //this is a default catch statement for unmapped skills to prevent un-handled exceptions
                };
                eventSwitchPerObjectId.First(sw => sw.Key(skillLookup)).Value();
            }
        }

        private void MageSkill(NecClient client, int skillId, uint skillTarget)
        {
            Vector3 charCoord = new Vector3(client.character.x, client.character.y, client.character.z);
            Spell spell = new Spell(_server, client, skillId, skillTarget, charCoord);
            server.instances.AssignInstance(spell);
            client.character.activeSkillInstance = spell.instanceId;
            spell.StartCast();
        }

        private void ThiefSkill(NecClient client, int skillId, uint skillTarget)
        {
            int skillBase = skillId / 1000;
            if (client.character.IsStealthed() && skillBase != 114607)
            {
                client.character.ClearStateBit(CharacterState.StealthForm);
                RecvCharaNotifyStateflag charState =
                    new RecvCharaNotifyStateflag(client.character.instanceId, (uint)client.character.state);
                _server.router.Send(client.map, charState);
            }

            if (skillBase > 114300 && skillBase < 114399)
            {
                Trap(client, skillId);
                return;
            }
            else if (skillBase == 114607)
            {
                Stealth(client, skillId);
                return;
            }

            if (skillTarget == 0)
            {
                _Logger.Debug($"Skill requires target!! [{skillId}]");
                int errorCode = -1311;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                router.Send(skillFail, client);
                return;
            }

            ThiefSkill thiefSkill = new ThiefSkill(_server, client, skillId, skillTarget);
            server.instances.AssignInstance(thiefSkill);
            client.character.activeSkillInstance = thiefSkill.instanceId;
            thiefSkill.StartCast();
        }

        private void Trap(NecClient client, int skillId)
        {
            if (!int.TryParse($"{skillId}".Substring(1, 5), out int skillBase))
            {
                _Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                router.Send(skillFail, client);
                return;
            }

            if (!int.TryParse($"{skillId}".Substring(1, 7), out int effectBase))
            {
                _Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                router.Send(skillFail, client);
                return;
            }

            effectBase += 1;
            _Logger.Debug($"skillId [{skillId}] skillBase [{skillBase}] effectBase [{effectBase}]");
            if (!_server.settingRepository.skillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Getting SkillBaseSetting for skillid [{skillId}]");
                return;
            }

            if (!_server.settingRepository.eoBase.TryGetValue(effectBase, out EoBaseSetting eoBaseSetting))
            {
                _Logger.Error($"Getting EoBaseSetting from effectBase [{effectBase}]");
                return;
            }

            Vector3 charPos = new Vector3(client.character.x, client.character.y, client.character.z);
            bool isBaseTrap = TrapTask.BaseTrap(skillBase);
            TrapStack trapStack = null;
            if (isBaseTrap)
            {
                int trapRadius = eoBaseSetting.effectRadius;
                trapStack = new TrapStack(_server, client, charPos, trapRadius);
                server.instances.AssignInstance(trapStack);
            }
            else
            {
                trapStack = client.map.GetTrapCharacterRange(client.character.instanceId, 75, charPos);
            }

            if (isBaseTrap)
            {
                _Logger.Debug(
                    $"Is base trap skillId [{skillId}] skillBase [{skillBase}] trapStack._trapRadius [{trapStack.trapRadius}]");
                if (client.map.GetTrapsCharacterRange(client.character.instanceId, trapStack.trapRadius, charPos))
                {
                    _Logger.Debug($"First trap with another trap too close [{skillId}]");
                    int errorCode = -1309;
                    RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                    router.Send(skillFail, client);
                    return;
                }
            }
            else
            {
                _Logger.Debug(
                    $"Is trap enhancement skillId [{skillId}] skillBase [{skillBase}] trapRadius [{trapStack.trapRadius}]");
                if (!client.map.GetTrapsCharacterRange(client.character.instanceId, trapStack.trapRadius, charPos))
                {
                    _Logger.Debug($"Trap enhancement without a base trap [{skillId}]");
                    int errorCode = -1;
                    RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                    router.Send(skillFail, client);
                    return;
                }
            }

            _Logger.Debug($"Valid position check for monsters skillId [{skillId}] skillBase [{skillBase}]");
            if (client.map.MonsterInRange(charPos, trapStack.trapRadius))
            {
                _Logger.Debug($"Monster too close [{skillId}]");
                int errorCode = -1310;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                router.Send(skillFail, client);
                return;
            }

            _Logger.Debug(
                $"skillBaseSetting.Id [{skillBaseSetting.id}] skillBaseSetting.Name [{skillBaseSetting.name} eoBaseSetting.]");
            _Logger.Debug($"spearTrap.InstanceId [{trapStack.instanceId}] SpearTrap skillID [{skillId}]");
            client.character.activeSkillInstance = trapStack.instanceId;
            client.character.castingSkill = true;
            trapStack.StartCast(skillBaseSetting);
        }

        private void Stealth(NecClient client, int skillId)
        {
            // I am doing this from memory, it could very well be wrong  :)
            // Not blocking any actions if stealthed.
            // Stealth will be turned off if start casting another skill or damage is done.

            int errorCode = 0;
            Stealth stealth = new Stealth(_server, client, skillId);
            server.instances.AssignInstance(stealth);
            client.character.activeSkillInstance = stealth.instanceId;
            if (!_server.settingRepository.skillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                errorCode = -1;
                RecvSkillStartCastR startFail = new RecvSkillStartCastR(errorCode, 0.0F);
                router.Send(startFail, client);
                return;
            }

            RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, skillBaseSetting.castingTime);
            router.Send(skillFail, client);
            stealth.StartCast();
        }

        private void SendBattleReportSkillStartCast(NecClient client, int mySkillId)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(mySkillId);
            router.Send(client.map, (ushort) AreaPacketId.recv_battle_report_action_skill_start_cast, res4,
                ServerType.Area);
        }

        private void SendEoNotifyDisappearSchedule(NecClient client, int mySkillId, float castingTime)
        {
            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(mySkillId);
            res5.WriteFloat(castingTime);
            router.Send(client.map, (ushort) AreaPacketId.recv_eo_notify_disappear_schedule, res5, ServerType.Area);
        }

        private void SendBattleReportStartNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(client.character.instanceId);
            router.Send(client.map, (ushort) AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }

        private void SendBattleReportEndNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            router.Send(client.map, (ushort) AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }

        private void SkillStartCast(NecClient client, int mySkillId, int mySkillTarget, float castingTime)
        {
            _Logger.Debug($"Skill Int : {mySkillId}");
            _Logger.Debug($"Target Int : {mySkillTarget}");
            _Logger.Debug($"my Character ID : {client.character.id}");
            _Logger.Debug($"my Character instanceId : {client.character.instanceId}");


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error check     | 0 - success
            /*
            SKILLCAST_FAILED	-1	You failed to cast <skill name>
            SKILLCAST_FAILED	-236	This skill cannot be used in town
            SKILLCAST_FAILED	-1300	Not enough HP
            SKILLCAST_FAILED	-1301	Not enough MP
            SKILLCAST_FAILED	-1302	Not enough OD
            SKILLCAST_FAILED	-1303	Not enough GP
            SKILLCAST_FAILED	-1304	Action failed since it is not ready
            SKILLCAST_FAILED	-1305	Skill cannot be used because you have not drawn your sword
            SKILLCAST_FAILED	-1306	Skill cannot be used because you are casting
            SKILLCAST_FAILED	-1307	Not enough <skill cost name>
            SKILLCAST_FAILED	-1308	Max traps laid
            SKILLCAST_FAILED	-1309	Skill cannot be used because it is already used in a trap
            SKILLCAST_FAILED	-1310	Skill cannot be used because an enemy is in range of the trap
            SKILLCAST_FAILED	-1311	No target to be added
            SKILLCAST_FAILED	-1312	No more locations can be added for this skill
            SKILLCAST_FAILED	-1320	Second trap has already been set
            SKILLCAST_FAILED	-1321	End trap has already been set
            SKILLCAST_FAILED	-1322	Ineligible target
            SKILLCAST_FAILED	-1325	Insufficient usage count for Power Level
            SKILLCAST_FAILED	-1326	You've used a skill that hasn't been set to a custom slot
            SKILLCAST_FAILED	-1327	Unable to use since character level is low
            SKILLCAST_FAILED	GENERIC	Error: <errcode>
            SKILLCAST_FAILED	5	You can not make any more <skill cost name>

            */

            res.WriteFloat(
                castingTime); //Casting time (countdown before auto-cast)    ./Skill_base.csv   Column I
            router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res, ServerType.Area);
        }

        private void SendSkillStartCastSelf(NecClient client, int mySkillId, uint mySkillTarget, float castingTime)
        {
            _Logger.Debug($"Skill Int : {mySkillId}");
            _Logger.Debug($"Target Int : {mySkillTarget}");
            _Logger.Debug($"my Character ID : {client.character.id}");
            _Logger.Debug($"my Character instanceId : {client.character.instanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillId); //previously Skill ID
            res.WriteFloat(castingTime);
            router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res, ServerType.Area);
        }

        private void SendSkillStartCastExR(NecClient client, int mySkillId, int mySkillTarget, float castingTime)
        {
            _Logger.Debug($"Skill Int : {mySkillId}");
            _Logger.Debug($"Target Int : {mySkillTarget}");
            _Logger.Debug($"my Character ID : {client.character.id}");
            _Logger.Debug($"my Character instanceId : {client.character.instanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error check     | 0 - success  See other codes above in SendSkillStartCast
            res.WriteFloat(castingTime); //casting time (countdown before auto-cast)    ./Skill_base.csv   Column L

            res.WriteInt32(100); //Cast Script?     ./Skill_base.csv   Column T
            res.WriteInt32(100); //Effect Script    ./Skill_base.csv   Column V
            res.WriteInt32(100); //Effect ID?   ./Skill_base.csv   Column X
            res.WriteInt32(0100); //Effect ID 2     ./Skill_base.csv   Column Z

            res.WriteInt32(mySkillId); //

            res.WriteInt32(10000); //Distance?              ./Skill_base.csv   Column AN
            res.WriteInt32(10000); //Height?                 ./Skill_base.csv   Column AO
            res.WriteInt32(500); //??                          ./Skill_base.csv   Column AP
            res.WriteInt32(client.character.heading); //??                       ./Skill_base.csv   Column AQ

            res.WriteInt32(5); // Effect time?

            router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_ex_r, res, ServerType.Area);
        }
    }
}
