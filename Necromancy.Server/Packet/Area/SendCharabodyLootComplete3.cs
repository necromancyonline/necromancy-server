using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_complete3 : ClientHandler
    {
        private NecServer _server;
        public send_charabody_loot_complete3(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_complete3;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.Data.ReadByte();
            byte fromContainer = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();
            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot);

            client.Map.DeadBodies.TryGetValue(client.Character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.Instances.GetInstance(deadBody.CharacterInstanceId) as Character;
            ItemService itemService = new ItemService(client.Character);
            ItemService deadCharacterItemService = new ItemService(deadCharacter);

            ItemInstance iteminstance = deadCharacterItemService.GetLootedItem(fromLoc);
            itemService.PutLootedItem(iteminstance);

            RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, iteminstance);
            Router.Send(client, recvItemInstanceUnidentified.ToPacket());
        }
    }
}
