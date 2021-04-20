using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyLootStart2 : ClientHandler
    {
        private readonly NecServer _server;

        public SendCharabodyLootStart2(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort)AreaPacketId.send_charabody_loot_start2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.data.ReadByte();
            byte fromContainer = packet.data.ReadByte();
            short fromSlot = packet.data.ReadInt16();

            if (fromZone == ItemZoneType.CorpseAdventureBag) fromZone = ItemZoneType.AdventureBag;
            if (fromZone == ItemZoneType.CorpseEquippedBags) fromZone = ItemZoneType.EquippedBags;
            if (fromZone == ItemZoneType.CorpsePremiumBag) fromZone = ItemZoneType.PremiumBag;
            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot); //subtract 71 to get from BodyCollection to Adventurer

            client.map.deadBodies.TryGetValue(client.character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.instances.GetInstance(deadBody.characterInstanceId) as Character;
            NecClient deadClient = _server.clients.GetByCharacterInstanceId(deadBody.characterInstanceId);
            if (deadCharacter != null) deadCharacter.lootNotify = fromLoc; //gotta be able to pass this information to loot_complete2 on successful loot

            //Tell your character to start looting the dead body.  Updates Pose
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //result / err check
            res.WriteInt32(10); //loot time
            router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_start2_r, res, ServerType.Area);

            if (deadClient != null)
            {
                //Tell the other player that they are getting lootified.
                res = BufferProvider.Provide();
                res.WriteByte((byte)fromZone);
                res.WriteByte(fromContainer);
                res.WriteInt16(fromSlot);
                res.WriteFloat(1); //base loot time
                res.WriteFloat(10); // loot time
                res.WriteCString($"{client.soul.name}"); // soul name
                res.WriteCString($"{client.character.name}"); // chara name
                router.Send(deadClient, (ushort)AreaPacketId.recv_charabody_notify_loot_start2, res, ServerType.Area);

                RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.character, client.soul.name);
                _server.router.Send(deadClient, cData.ToPacket());
            }
        }
    }
}
