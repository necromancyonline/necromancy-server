using System.Collections.Generic;
using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Chat.Command.Commands
{
    /// <summary>
    /// Spawns a monster
    /// </summary>
    public class MonsterCommand : ServerChatCommand
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(MonsterCommand));

        //protected NecServer server { get; }
        public MonsterCommand(NecServer server) : base(server)
        {
            //this.server = server;
        }

        int _i = 0;

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            MonsterSpawn monsterSpawn = new MonsterSpawn();
            server.instances.AssignInstance(monsterSpawn);
            if (!int.TryParse(command[0], out int monsterId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[0]}"));
                return;
            }

            if (!server.settingRepository.monster.TryGetValue(monsterId, out MonsterSetting monsterSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid MonsterId: {monsterId}"));
                return;
            }

            if (!int.TryParse(command[1], out int modelId))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid Number: {command[1]}"));
                return;
            }


            if (!server.settingRepository.modelCommon.TryGetValue(modelId, out ModelCommonSetting modelSetting))
            {
                responses.Add(ChatResponse.CommandError(client, $"Invalid ModelId: {modelId}"));
                return;
            }

            _Logger.Debug($"modelSetting.Radius [{modelSetting.radius}]");
            monsterSpawn.monsterId = monsterSetting.id;
            monsterSpawn.name = monsterSetting.name;
            monsterSpawn.title = monsterSetting.title;
            monsterSpawn.level = (byte) monsterSetting.level;

            monsterSpawn.modelId = modelSetting.id;
            monsterSpawn.size = (short) (modelSetting.height / 2);
            monsterSpawn.radius = (short) modelSetting.radius;

            monsterSpawn.mapId = client.character.mapId;

            monsterSpawn.x = client.character.x;
            monsterSpawn.y = client.character.y;
            monsterSpawn.z = client.character.z;
            monsterSpawn.heading = client.character.heading;

            monsterSpawn.hp.SetMax(100);
            monsterSpawn.hp.SetCurrent(100);

            if (!server.database.InsertMonsterSpawn(monsterSpawn))
            {
                responses.Add(ChatResponse.CommandError(client, "MonsterSpawn could not be saved to database"));
                return;
            }

            RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
            router.Send(client.map, monsterData);

            IBuffer res = BufferProvider.Provide();

            res.WriteUInt32((uint)monsterSetting.id);
            //Toggles state between Alive(attackable),  Dead(lootable), or Inactive(nothing).
            res.WriteInt32(_i);
            _i++;

            router.Send(client, (ushort)AreaPacketId.recv_monster_state_update_notify, res, ServerType.Area);
        }


        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "mon";
        public override string helpText => "usage: `/mon [monsterId] [modelId]` - Spawns a Monster";
    }
}
