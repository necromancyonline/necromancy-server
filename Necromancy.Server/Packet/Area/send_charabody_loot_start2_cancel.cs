using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_start2_cancel : ClientHandler
    {
        private NecServer _server;
        public send_charabody_loot_start2_cancel(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_start2_cancel;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Map.DeadBodies.TryGetValue(client.Character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.Instances.GetInstance(deadBody.CharacterInstanceId) as Character;
            ItemLocation itemLocation = deadCharacter.lootNotify;
            NecClient necClient = client.Map.ClientLookup.GetByCharacterInstanceId(deadBody.CharacterInstanceId);

            RecvCharaBodyNotifyLootStartCancel recvCharaBodyNotifyLootStartCancel = new RecvCharaBodyNotifyLootStartCancel((byte)itemLocation.ZoneType, itemLocation.Container, itemLocation.Slot, client.Soul.Name, client.Character.Name);
            if (necClient != null) Router.Send(necClient, recvCharaBodyNotifyLootStartCancel.ToPacket());
            RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.Character.InstanceId);
            if (necClient != null) Router.Send(necClient, recvObjectDisappearNotify.ToPacket());

        }
    }
}
