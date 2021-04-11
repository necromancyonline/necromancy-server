using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Skills;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Tasks;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_skill_exec : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_skill_exec));

        private readonly NecServer _server;

        public send_skill_exec(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_skill_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetId = packet.Data.ReadInt32();
            int skillId = client.Character.skillStartCast;
            float X = packet.Data.ReadFloat();
            float Y = packet.Data.ReadFloat();
            float Z = packet.Data.ReadFloat();

            int errcode = packet.Data.ReadInt32();

            Logger.Debug($"myTargetID : {targetId}");
            Logger.Debug($"Target location : X-{X}Y-{Y}Z-{Z}");
            Logger.Debug($"ErrorCode : {errcode}");
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(errcode); //see sys_msg.csv
            /*
                -1      Unable to use skill
                -1322   Incorrect target
                -1325   Insufficient usage count for Power Level
                1       Not enough distance
                GENERIC Unable to use skill: < errcode >
            */
            res.WriteFloat(2); //Cool time      ./Skill_base.csv   Column J 
            res.WriteFloat(1); //Rigidity time  ./Skill_base.csv   Column L  
            //Router.Send(client.Map, (ushort)AreaPacketId.recv_skill_exec_r, res, ServerType.Area);

            int skillLookup = skillId / 1000;
            Logger.Debug($"skillLookup : {skillLookup}");
            var eventSwitchPerObjectID = new Dictionary<Func<int, bool>, Action>
            {
                {x => (x > 114100 && x < 114199), () => ThiefSkill(client, skillId, targetId)},
                {x => (x > 114300 && x < 114399), () => ThiefSkill(client, skillId, targetId)},
                {x => x == 114607               , () => ThiefSkill(client, skillId, targetId)},
                {x => (x > 113000 && x < 113999), () => MageSkill(client, skillId, targetId)},
                {x => (x > 1 && x < 999999), () => MageSkill(client, skillId, targetId)} //this is a default catch statement for unmapped skills to prevent un-handled exceptions 
            };

            eventSwitchPerObjectID.First(sw => sw.Key(skillLookup)).Value();
            client.Character.castingSkill = false;


            ////////////////////Battle testing below this line.

            //Delete all this. it was just for fun. and an example for how we impact targets with other more proper recvs.
            //IBuffer res3 = BufferProvider.Provide();
            //res3.WriteInt32(instance.InstanceId);
            //Router.Send(client, (ushort)AreaPacketId.recv_object_disappear_notify, res3, ServerType.Area);


            //IBuffer res4 = BufferProvider.Provide();
            //res4.WriteInt32(instance.InstanceId);
            //res4.WriteByte((byte)(Util.GetRandomNumber(0,70))); // % hp remaining of target.  need to store current NPC HP and OD as variables to "attack" them
            //Router.Send(client, (ushort)AreaPacketId.recv_object_hp_per_update_notify, res4, ServerType.Area);
        }

        private void MageSkill(NecClient client, int skillId, int targetId)
        {
            if (!_server.SettingRepository.SkillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            RecvSkillExecR execSuccess =
                new RecvSkillExecR(0, skillBaseSetting.CastingCooldown, skillBaseSetting.RigidityTime);
            Router.Send(execSuccess, client);

            Spell spell = (Spell) Server.Instances.GetInstance((uint) client.Character.activeSkillInstance);
            spell.SkillExec();
        }

        private void ThiefSkill(NecClient client, int skillId, int targetId)
        {
            int skillBase = skillId / 1000;
            if (!_server.SettingRepository.SkillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            if (skillBase > 114300 && skillBase < 114399)
            {
                Trap(client, skillId, skillBaseSetting);
                return;
            }
            else if (skillBase == 114607)
            {
                Stealth(client, skillId, skillBaseSetting);
                return;
            }

            RecvSkillExecR execSuccess =
                new RecvSkillExecR(0, skillBaseSetting.CastingCooldown, skillBaseSetting.RigidityTime);
            Router.Send(execSuccess, client);
            ThiefSkill thiefSkill =
                (ThiefSkill) Server.Instances.GetInstance((uint) client.Character.activeSkillInstance);
            thiefSkill.SkillExec();
        }

        private void Trap(NecClient client, int skillId, SkillBaseSetting skillBaseSetting)
        {
            Logger.Debug($"skillId : {skillId}");
            if (!int.TryParse($"{skillId}".Substring(1, 5), out int skillBase))
            {
                Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            if (!int.TryParse($"{skillId}".Substring(1, 7), out int effectBase))
            {
                Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            bool isBaseTrap = TrapTask.baseTrap(skillBase);
            effectBase += 1;
            if (!_server.SettingRepository.EoBase.TryGetValue(effectBase, out EoBaseSetting eoBaseSetting))
            {
                Logger.Error($"Getting EoBaseSetting from effectBase [{effectBase}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            if (!_server.SettingRepository.EoBase.TryGetValue(effectBase + 1, out EoBaseSetting eoBaseSettingTriggered))
            {
                Logger.Error($"Getting EoBaseSetting from effectBase+1 [{effectBase + 1}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                Router.Send(execFail, client);
                return;
            }

            RecvSkillExecR execSuccess =
                new RecvSkillExecR(0, skillBaseSetting.CastingCooldown, skillBaseSetting.RigidityTime);
            Router.Send(execSuccess, client);

            // ToDo  verify trap parts available and remove correct number from inventory
            TrapStack trapStack = (TrapStack) Server.Instances.GetInstance((uint) client.Character.activeSkillInstance);
            Trap trap = new Trap(skillBase, skillBaseSetting, eoBaseSetting, eoBaseSettingTriggered);
            _server.Instances.AssignInstance(trap);
            Logger.Debug($"trap.InstanceId [{trap.InstanceId}]  trap.Name [{trap._name}]  skillId[{skillId}]");
            trapStack.SkillExec(trap, isBaseTrap);
            Vector3 trapPos = new Vector3(client.Character.X, client.Character.Y, client.Character.Z);
        }

        private void Stealth(NecClient client, int skillId, SkillBaseSetting skillBaseSetting)
        {
            float cooldown = 0.0F;
            Logger.Debug($"IsStealthed [{client.Character.IsStealthed()}]");
            if (client.Character.IsStealthed())
                cooldown = skillBaseSetting.CastingCooldown;
            RecvSkillExecR execSuccess = new RecvSkillExecR(0, cooldown, skillBaseSetting.RigidityTime);
            Router.Send(execSuccess, client);

            Stealth stealth = (Stealth) Server.Instances.GetInstance((uint) client.Character.activeSkillInstance);
            stealth.SkillExec();
        }
    }
}
