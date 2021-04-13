using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Model.Skills
{
    public class Stealth : IInstance
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(Stealth));

        private readonly NecClient _client;
        private readonly NecServer _server;
        private readonly int _skillid;
        private readonly SkillBaseSetting _skillSetting;

        public Stealth(NecServer server, NecClient client, int skillId)
        {
            _server = server;
            _client = client;
            _skillid = skillId;
            if (!_server.settingRepository.skillBase.TryGetValue(skillId, out _skillSetting)) _Logger.Error($"Could not get SkillBaseSetting for skillId : {skillId}");
        }

        public uint instanceId { get; set; }

        public void StartCast()
        {
            _Logger.Debug($"CastingTime : {_skillSetting.castingTime}");
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(_skillid, _skillSetting.castingTime);
            _server.router.Send(_client, startCast.ToPacket()); //do not send "Self"  recvs to map. that breaks things.
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(_skillid);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
        }

        public void SkillExec()
        {
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec =
                new RecvBattleReportActionSkillExec(_client.character.skillStartCast);
            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);


            Task.Delay(TimeSpan.FromSeconds(_skillSetting.rigidityTime)).ContinueWith
            (t1 =>
                {
                    //Make it so you can kinda see yourself
                    _client.character.AddStateBit(CharacterState.InvulnerableForm); //todo. fix stealth form
                    RecvCharaNotifyStateflag myStateFlag = new RecvCharaNotifyStateflag(_client.character.instanceId, (ulong)_client.character.state);
                    _server.router.Send(_client, myStateFlag.ToPacket());

                    //make other players not able to see you
                    _client.character.AddStateBit(CharacterState.InvisibleForm);
                    RecvCharaNotifyStateflag stateFlag = new RecvCharaNotifyStateflag(_client.character.instanceId, (ulong)_client.character.state);
                    _server.router.Send(_client.map, stateFlag, _client);
                }
            );


            //clear stealth after 10 seconds.   //ToDo  ,  change seconds to skills effective time.
            Task.Delay(TimeSpan.FromSeconds(_skillSetting.effectTime + _skillSetting.rigidityTime)).ContinueWith
            (t1 =>
                {
                    _client.character.ClearStateBit(CharacterState.InvisibleForm);
                    _client.character.ClearStateBit(CharacterState.InvulnerableForm);
                    RecvCharaNotifyStateflag recvCharaNotifyStateflag = new RecvCharaNotifyStateflag(_client.character.instanceId, (ulong)_client.character.state);
                    _server.router.Send(_client.map, recvCharaNotifyStateflag.ToPacket());
                }
            );

            //0bxxxxxxx1 - 1 Soul Form / 0 Normal  | (Soul form is Glowing with No armor)
            //0bxxxxxx1x - 1 Battle Pose / 0 Normal
            //0bxxxxx1xx - 1 Block Pose / 0 Normal | (for coming out of stealth while blocking)
            //0bxxxx1xxx - 1 transparent / 0 solid  | (stealth in party partial visibility)
            //0bxxx1xxxx -
            //0bxx1xxxxx - 1 invisible / 0 visible  | (Stealth to enemies)
            //0bx1xxxxxx - 1 blinking  / 0 solid    | (10  second invulnerability blinking)
            //0b1xxxxxxx -
        }
    }
}
