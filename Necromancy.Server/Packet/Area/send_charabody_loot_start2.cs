using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_start2 : ClientHandler
    {
        private NecServer _server;
        public send_charabody_loot_start2(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_start2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemZoneType fromZone = (ItemZoneType)packet.Data.ReadByte();
            byte fromContainer = packet.Data.ReadByte();
            short fromSlot = packet.Data.ReadInt16();

            if (fromZone == ItemZoneType.BCAdventureBag) fromZone = ItemZoneType.AdventureBag;
            if (fromZone == ItemZoneType.BCEquippedBag) fromZone = ItemZoneType.EquippedBags;
            if (fromZone == ItemZoneType.BCPremiumBag) fromZone = ItemZoneType.PremiumBag;
            ItemLocation fromLoc = new ItemLocation(fromZone, fromContainer, fromSlot); //subtract 71 to get from BodyCollection to Adventurer

            client.Map.DeadBodies.TryGetValue(client.Character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.Instances.GetInstance(deadBody.CharacterInstanceId) as Character;
            NecClient deadClient = _server.Clients.GetByCharacterInstanceId(deadBody.CharacterInstanceId);
            if (deadCharacter != null)
            {
                deadCharacter.lootNotify = fromLoc; //gotta be able to pass this information to loot_complete2 on successful loot

            }
           
            //Tell your character to start looting the dead body.  Updates Pose
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //result / err check
            res.WriteInt32(10); //loot time
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_start2_r, res, ServerType.Area);

            if (deadClient != null)
            {
                //Tell the other player that they are getting lootified.  
                res = BufferProvider.Provide();
                res.WriteByte((byte)fromZone);
                res.WriteByte(fromContainer);
                res.WriteInt16(fromSlot);
                res.WriteFloat(1); //base loot time
                res.WriteFloat(10); // loot time
                res.WriteCString($"{client.Soul.Name}"); // soul name
                res.WriteCString($"{client.Character.Name}"); // chara name
                Router.Send(deadClient, (ushort)AreaPacketId.recv_charabody_notify_loot_start2, res, ServerType.Area);
            }

        }
    }
}
