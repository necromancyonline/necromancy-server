using System.Collections.Generic;
using System.Numerics;
using Arrowgene.Logging;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model.Skills
{
    public class TrapStack : IInstance
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(TrapStack));

        public uint instanceId { get; set; }

        private NecClient _client;
        private readonly NecServer _server;
        private uint _ownerInstanceId;
        public int trapRadius { get; }
        public TrapTask trapTask;
        private Map _map;
        private Vector3 _trapPos;

        public TrapStack(NecServer server, NecClient client, Vector3 trapPos, int trapRadius)
        {
            _server = server;
            _client = client;
            _map = _client.map;
            _ownerInstanceId = client.character.instanceId;
            _trapPos = trapPos;
            this.trapRadius = trapRadius;
        }

        public void StartCast(SkillBaseSetting skillBase)
        {
            _Logger.Debug(
                $"Trap StartCast skillBase.Id [{skillBase.id}] skillBase.CastingTime [{skillBase.castingTime}]");
            RecvSkillStartCastSelf startCast = new RecvSkillStartCastSelf(skillBase.id, skillBase.castingTime);
            _server.router.Send(startCast, _client);
            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillStartCast brStartCast = new RecvBattleReportActionSkillStartCast(skillBase.id);

            brList.Add(brStart);
            brList.Add(brStartCast);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
        }

        public void SkillExec(Trap trap, bool isBaseTrap)
        {
            Vector3 trgCoord = new Vector3(_client.character.x, _client.character.y, _client.character.z);
            if (!int.TryParse($"{trap.skillId}".Substring(1, 6) + 1, out int effectId))
            {
                _Logger.Error($"Creating effectId from skillid [{trap.skillId}]");
            }

            List<PacketResponse> brList = new List<PacketResponse>();
            RecvBattleReportStartNotify brStart = new RecvBattleReportStartNotify(_client.character.instanceId);
            RecvBattleReportEndNotify brEnd = new RecvBattleReportEndNotify();
            RecvBattleReportActionSkillExec brExec = new RecvBattleReportActionSkillExec(trap.skillId);

            brList.Add(brStart);
            brList.Add(brExec);
            brList.Add(brEnd);
            _server.router.Send(_client.map, brList);
            _Logger.Debug($"SpearTrap effectId [{effectId}]");
            RecvDataNotifyEoData eoData = new RecvDataNotifyEoData(trap.instanceId, _client.character.instanceId,
                effectId, trgCoord, 2, 2);
            _server.router.Send(_map, eoData);

            if (isBaseTrap)
            {
                trapTask = new TrapTask(_server, _map, _trapPos, _ownerInstanceId, trap, this.instanceId);
                trapTask.AddTrap(trap);
                _map.AddTrap(this.instanceId, this);
                trapTask.Start();
            }
            else
            {
                trapTask.AddTrap(trap);
            }
        }
    }
}
