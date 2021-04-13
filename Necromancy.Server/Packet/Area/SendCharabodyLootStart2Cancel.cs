using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyLootStart2Cancel : ClientHandler
    {
        private readonly NecServer _server;

        public SendCharabodyLootStart2Cancel(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort)AreaPacketId.send_charabody_loot_start2_cancel;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.map.deadBodies.TryGetValue(client.character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.instances.GetInstance(deadBody.characterInstanceId) as Character;
            ItemLocation itemLocation = deadCharacter.lootNotify;
            NecClient necClient = client.map.clientLookup.GetByCharacterInstanceId(deadBody.characterInstanceId);

            RecvCharaBodyNotifyLootStartCancel recvCharaBodyNotifyLootStartCancel = new RecvCharaBodyNotifyLootStartCancel((byte)itemLocation.zoneType, itemLocation.container, itemLocation.slot, client.soul.name, client.character.name);
            if (necClient != null) router.Send(necClient, recvCharaBodyNotifyLootStartCancel.ToPacket());
            RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.character.instanceId);
            if (necClient != null) router.Send(necClient, recvObjectDisappearNotify.ToPacket());
        }
    }
}
