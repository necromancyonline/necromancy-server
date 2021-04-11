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
    public class send_skill_start_cast : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_skill_start_cast));

        private NecServer _server;

        public send_skill_start_cast(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_start_cast;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int skillID = packet.Data.ReadInt32();
            uint skillTarget = packet.Data.ReadUInt32();
            client.Character.eventSelectReadyCode = skillTarget;
            client.Character.skillStartCast = skillID;
            int skillLookup = skillID / 1000;
            Logger.Debug($"skillTarget [{skillTarget}]  skillID [{skillID}] skillLookup [{skillLookup}]");
            {
                var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
                {
                    {x => (x > 114100 && x < 114199), () => ThiefSkill(client, skillID, skillTarget)},
                    {x => (x > 114300 && x < 114399), () => ThiefSkill(client, skillID, skillTarget)},
                    {x => (x > 113000 && x < 113999), () => MageSkill(client, skillID, skillTarget)},
                    {x => x == 114607, () => ThiefSkill(client, skillID, skillTarget)},
                    {
                        x => (x > 1 && x < 999999), () => MageSkill(client, skillID, skillTarget)
                    } //this is a default catch statement for unmapped skills to prevent un-handled exceptions
                };
                eventSwitchPerObjectID.First(sw => sw.Key(skillLookup)).Value();
            }
        }

        private void MageSkill(NecClient client, int skillId, uint skillTarget)
        {
            Vector3 charCoord = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            Spell spell = new Spell(_server, client, skillId, skillTarget, charCoord);
            Server.Instances.AssignInstance(spell);
            client.Character.activeSkillInstance = spell.InstanceId;
            spell.StartCast();
        }

        private void ThiefSkill(NecClient client, int skillId, uint skillTarget)
        {
            int skillBase = skillId / 1000;
            if (client.Character.IsStealthed() && skillBase != 114607)
            {
                client.Character.ClearStateBit(CharacterState.StealthForm);
                RecvCharaNotifyStateflag charState =
                    new RecvCharaNotifyStateflag(client.Character.InstanceId, (uint)client.Character.State);
                _server.Router.Send(client.Map, charState);
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
                Logger.Debug($"Skill requires target!! [{skillId}]");
                int errorCode = -1311;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                Router.Send(skillFail, client);
                return;
            }

            ThiefSkill thiefSkill = new ThiefSkill(_server, client, skillId, skillTarget);
            Server.Instances.AssignInstance(thiefSkill);
            client.Character.activeSkillInstance = thiefSkill.InstanceId;
            thiefSkill.StartCast();
        }

        private void Trap(NecClient client, int skillId)
        {
            if (!int.TryParse($"{skillId}".Substring(1, 5), out int skillBase))
            {
                Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                Router.Send(skillFail, client);
                return;
            }

            if (!int.TryParse($"{skillId}".Substring(1, 7), out int effectBase))
            {
                Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                Router.Send(skillFail, client);
                return;
            }

            effectBase += 1;
            Logger.Debug($"skillId [{skillId}] skillBase [{skillBase}] effectBase [{effectBase}]");
            if (!_server.SettingRepository.SkillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Getting SkillBaseSetting for skillid [{skillId}]");
                return;
            }

            if (!_server.SettingRepository.EoBase.TryGetValue(effectBase, out EoBaseSetting eoBaseSetting))
            {
                Logger.Error($"Getting EoBaseSetting from effectBase [{effectBase}]");
                return;
            }

            Vector3 charPos = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
            bool isBaseTrap = TrapTask.baseTrap(skillBase);
            TrapStack trapStack = null;
            if (isBaseTrap)
            {
                int trapRadius = eoBaseSetting.EffectRadius;
                trapStack = new TrapStack(_server, client, charPos, trapRadius);
                Server.Instances.AssignInstance(trapStack);
            }
            else
            {
                trapStack = client.Map.GetTrapCharacterRange(client.Character.InstanceId, 75, charPos);
            }

            if (isBaseTrap)
            {
                Logger.Debug(
                    $"Is base trap skillId [{skillId}] skillBase [{skillBase}] trapStack._trapRadius [{trapStack._trapRadius}]");
                if (client.Map.GetTrapsCharacterRange(client.Character.InstanceId, trapStack._trapRadius, charPos))
                {
                    Logger.Debug($"First trap with another trap too close [{skillId}]");
                    int errorCode = -1309;
                    RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                    Router.Send(skillFail, client);
                    return;
                }
            }
            else
            {
                Logger.Debug(
                    $"Is trap enhancement skillId [{skillId}] skillBase [{skillBase}] trapRadius [{trapStack._trapRadius}]");
                if (!client.Map.GetTrapsCharacterRange(client.Character.InstanceId, trapStack._trapRadius, charPos))
                {
                    Logger.Debug($"Trap enhancement without a base trap [{skillId}]");
                    int errorCode = -1;
                    RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                    Router.Send(skillFail, client);
                    return;
                }
            }

            Logger.Debug($"Valid position check for monsters skillId [{skillId}] skillBase [{skillBase}]");
            if (client.Map.MonsterInRange(charPos, trapStack._trapRadius))
            {
                Logger.Debug($"Monster too close [{skillId}]");
                int errorCode = -1310;
                RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, 0);
                Router.Send(skillFail, client);
                return;
            }

            Logger.Debug(
                $"skillBaseSetting.Id [{skillBaseSetting.Id}] skillBaseSetting.Name [{skillBaseSetting.Name} eoBaseSetting.]");
            Logger.Debug($"spearTrap.InstanceId [{trapStack.InstanceId}] SpearTrap skillID [{skillId}]");
            client.Character.activeSkillInstance = trapStack.InstanceId;
            client.Character.castingSkill = true;
            trapStack.StartCast(skillBaseSetting);
        }

        private void Stealth(NecClient client, int skillId)
        {
            // I am doing this from memory, it could very well be wrong  :)
            // Not blocking any actions if stealthed.
            // Stealth will be turned off if start casting another skill or damage is done.

            int errorCode = 0;
            Stealth stealth = new Stealth(_server, client, skillId);
            Server.Instances.AssignInstance(stealth);
            client.Character.activeSkillInstance = stealth.InstanceId;
            if (!_server.SettingRepository.SkillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                errorCode = -1;
                RecvSkillStartCastR startFail = new RecvSkillStartCastR(errorCode, 0.0F);
                Router.Send(startFail, client);
                return;
            }

            RecvSkillStartCastR skillFail = new RecvSkillStartCastR(errorCode, skillBaseSetting.CastingTime);
            Router.Send(skillFail, client);
            stealth.StartCast();
        }

        private void SendBattleReportSkillStartCast(NecClient client, int mySkillID)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteInt32(mySkillID);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_action_skill_start_cast, res4,
                ServerType.Area);
        }

        private void SendEoNotifyDisappearSchedule(NecClient client, int mySkillID, float castingTime)
        {
            IBuffer res5 = BufferProvider.Provide();
            res5.WriteInt32(mySkillID);
            res5.WriteFloat(castingTime);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_eo_notify_disappear_schedule, res5, ServerType.Area);
        }

        private void SendBattleReportStartNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(client.Character.InstanceId);
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_start_notify, res4, ServerType.Area);
        }

        private void SendBattleReportEndNotify(NecClient client)
        {
            IBuffer res4 = BufferProvider.Provide();
            Router.Send(client.Map, (ushort) AreaPacketId.recv_battle_report_end_notify, res4, ServerType.Area);
        }

        private void SendSkillStartCast(NecClient client, int mySkillID, int mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");


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
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_r, res, ServerType.Area);
        }

        private void SendSkillStartCastSelf(NecClient client, int mySkillID, uint mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(mySkillID); //previously Skill ID
            res.WriteFloat(castingTime);
            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_self, res, ServerType.Area);
        }

        private void SendSkillStartCastExR(NecClient client, int mySkillID, int mySkillTarget, float castingTime)
        {
            Logger.Debug($"Skill Int : {mySkillID}");
            Logger.Debug($"Target Int : {mySkillTarget}");
            Logger.Debug($"my Character ID : {client.Character.Id}");
            Logger.Debug($"my Character instanceId : {client.Character.InstanceId}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Error check     | 0 - success  See other codes above in SendSkillStartCast
            res.WriteFloat(castingTime); //casting time (countdown before auto-cast)    ./Skill_base.csv   Column L

            res.WriteInt32(100); //Cast Script?     ./Skill_base.csv   Column T
            res.WriteInt32(100); //Effect Script    ./Skill_base.csv   Column V
            res.WriteInt32(100); //Effect ID?   ./Skill_base.csv   Column X 
            res.WriteInt32(0100); //Effect ID 2     ./Skill_base.csv   Column Z 

            res.WriteInt32(mySkillID); //

            res.WriteInt32(10000); //Distance?              ./Skill_base.csv   Column AN 
            res.WriteInt32(10000); //Height?                 ./Skill_base.csv   Column AO 
            res.WriteInt32(500); //??                          ./Skill_base.csv   Column AP 
            res.WriteInt32(client.Character.Heading); //??                       ./Skill_base.csv   Column AQ 

            res.WriteInt32(5); // Effect time?

            Router.Send(client, (ushort) AreaPacketId.recv_skill_start_cast_ex_r, res, ServerType.Area);
        }
    }
}
