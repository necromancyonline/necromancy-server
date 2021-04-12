using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyLootComplete3 : ClientHandler
    {
        private readonly NecServer _server;

        public SendCharabodyLootComplete3(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort id => (ushort)AreaPacketId.send_charabody_loot_complete3;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.data.ReadByte();
            byte fromContainer = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();
            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);

            client.map.deadBodies.TryGetValue(client.character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.instances.GetInstance(deadBody.characterInstanceId) as Character;
            ItemService itemService = new ItemService(client.character);
            ItemService deadCharacterItemService = new ItemService(deadCharacter);

            ItemInstance iteminstance = deadCharacterItemService.GetLootedItem(fromLoc);
            itemService.PutLootedItem(iteminstance);

            RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, iteminstance);
            router.Send(client, recvItemInstanceUnidentified.ToPacket());
        }
    }
}
