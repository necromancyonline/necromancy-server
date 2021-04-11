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
    public class SendSkillExec : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendSkillExec));

        private readonly NecServer _server;

        public SendSkillExec(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort) AreaPacketId.send_skill_exec;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int targetId = packet.data.ReadInt32();
            int skillId = client.character.skillStartCast;
            float xCoordinate = packet.data.ReadFloat();
            float yCoordinate = packet.data.ReadFloat();
            float zCoodrinate = packet.data.ReadFloat();

            int errcode = packet.data.ReadInt32();

            _Logger.Debug($"myTargetID : {targetId}");
            _Logger.Debug($"Target location : X-{xCoordinate}Y-{yCoordinate}Z-{zCoodrinate}");
            _Logger.Debug($"ErrorCode : {errcode}");
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
            _Logger.Debug($"skillLookup : {skillLookup}");
            var eventSwitchPerObjectId = new Dictionary<Func<int, bool>, Action>
            {
                {x => (x > 114100 && x < 114199), () => ThiefSkill(client, skillId, targetId)},
                {x => (x > 114300 && x < 114399), () => ThiefSkill(client, skillId, targetId)},
                {x => x == 114607               , () => ThiefSkill(client, skillId, targetId)},
                {x => (x > 113000 && x < 113999), () => MageSkill(client, skillId, targetId)},
                {x => (x > 1 && x < 999999), () => MageSkill(client, skillId, targetId)} //this is a default catch statement for unmapped skills to prevent un-handled exceptions
            };

            eventSwitchPerObjectId.First(sw => sw.Key(skillLookup)).Value();
            client.character.castingSkill = false;


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
            if (!_server.settingRepository.skillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
                return;
            }

            RecvSkillExecR execSuccess =
                new RecvSkillExecR(0, skillBaseSetting.castingCooldown, skillBaseSetting.rigidityTime);
            router.Send(execSuccess, client);

            Spell spell = (Spell) server.instances.GetInstance((uint) client.character.activeSkillInstance);
            spell.SkillExec();
        }

        private void ThiefSkill(NecClient client, int skillId, int targetId)
        {
            int skillBase = skillId / 1000;
            if (!_server.settingRepository.skillBase.TryGetValue(skillId, out SkillBaseSetting skillBaseSetting))
            {
                _Logger.Error($"Getting SkillBaseSetting from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
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
                new RecvSkillExecR(0, skillBaseSetting.castingCooldown, skillBaseSetting.rigidityTime);
            router.Send(execSuccess, client);
            ThiefSkill thiefSkill =
                (ThiefSkill) server.instances.GetInstance((uint) client.character.activeSkillInstance);
            thiefSkill.SkillExec();
        }

        private void Trap(NecClient client, int skillId, SkillBaseSetting skillBaseSetting)
        {
            _Logger.Debug($"skillId : {skillId}");
            if (!int.TryParse($"{skillId}".Substring(1, 5), out int skillBase))
            {
                _Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
                return;
            }

            if (!int.TryParse($"{skillId}".Substring(1, 7), out int effectBase))
            {
                _Logger.Error($"Creating skillBase from skillid [{skillId}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
                return;
            }

            bool isBaseTrap = TrapTask.BaseTrap(skillBase);
            effectBase += 1;
            if (!_server.settingRepository.eoBase.TryGetValue(effectBase, out EoBaseSetting eoBaseSetting))
            {
                _Logger.Error($"Getting EoBaseSetting from effectBase [{effectBase}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
                return;
            }

            if (!_server.settingRepository.eoBase.TryGetValue(effectBase + 1, out EoBaseSetting eoBaseSettingTriggered))
            {
                _Logger.Error($"Getting EoBaseSetting from effectBase+1 [{effectBase + 1}]");
                int errorCode = -1;
                RecvSkillExecR execFail = new RecvSkillExecR(errorCode, 0, 0);
                router.Send(execFail, client);
                return;
            }

            RecvSkillExecR execSuccess =
                new RecvSkillExecR(0, skillBaseSetting.castingCooldown, skillBaseSetting.rigidityTime);
            router.Send(execSuccess, client);

            // ToDo  verify trap parts available and remove correct number from inventory
            TrapStack trapStack = (TrapStack) server.instances.GetInstance((uint) client.character.activeSkillInstance);
            Trap trap = new Trap(skillBase, skillBaseSetting, eoBaseSetting, eoBaseSettingTriggered);
            _server.instances.AssignInstance(trap);
            _Logger.Debug($"trap.InstanceId [{trap.instanceId}]  trap.Name [{trap.name}]  skillId[{skillId}]");
            trapStack.SkillExec(trap, isBaseTrap);
            Vector3 trapPos = new Vector3(client.character.x, client.character.y, client.character.z);
        }

        private void Stealth(NecClient client, int skillId, SkillBaseSetting skillBaseSetting)
        {
            float cooldown = 0.0F;
            _Logger.Debug($"IsStealthed [{client.character.IsStealthed()}]");
            if (client.character.IsStealthed())
                cooldown = skillBaseSetting.castingCooldown;
            RecvSkillExecR execSuccess = new RecvSkillExecR(0, cooldown, skillBaseSetting.rigidityTime);
            router.Send(execSuccess, client);

            Stealth stealth = (Stealth) server.instances.GetInstance((uint) client.character.activeSkillInstance);
            stealth.SkillExec();
        }
    }
}
