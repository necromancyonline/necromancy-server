using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyLootComplete2 : ClientHandler
    {
        private readonly NecServer _server;

        public SendCharabodyLootComplete2(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort id => (ushort)AreaPacketId.send_charabody_loot_complete2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.map.deadBodies.TryGetValue(client.character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.instances.GetCharacterByInstanceId(deadBody.characterInstanceId);
            //Todo - server or map needs to maintain characters in memory for a period of time after disconnect
            NecClient deadClient = _server.clients.GetByCharacterInstanceId(deadBody.characterInstanceId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //result, 0 sucess.  interupted, etc.
            res.WriteFloat(0); // time remaining
            router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_complete2_r, res, ServerType.Area);

            if (deadClient != null)
            {
                res = BufferProvider.Provide();
                res.WriteByte((byte)deadCharacter.lootNotify.zoneType);
                res.WriteByte(deadCharacter.lootNotify.container);
                res.WriteInt16(deadCharacter.lootNotify.slot);

                res.WriteInt16(1 /*iteminstance.Quantity*/); //Number here is "pieces"
                res.WriteCString($"{client.soul.name}"); // soul name
                res.WriteCString($"{client.character.name}"); // chara name
                router.Send(deadClient, (ushort)AreaPacketId.recv_charabody_notify_loot_item, res, ServerType.Area);
            }

            //if (successfull loot condition here)
            {
                ItemService itemService = new ItemService(client.character);
                ItemService deadCharacterItemService = new ItemService(deadCharacter);

                ItemInstance iteminstance = deadCharacterItemService.GetLootedItem(deadCharacter.lootNotify);
                //remove the icon from the deadClient's inventory if they are online.
                RecvItemRemove recvItemRemove = new RecvItemRemove(deadClient, iteminstance);
                if (deadClient != null) router.Send(recvItemRemove);

                //Add RecvItemRemove to remove the icon from the charabody window on successfull loot as well.//ToDo - this didnt work
                RecvItemRemove recvItemRemove2 = new RecvItemRemove(client, iteminstance);
                router.Send(recvItemRemove2);

                //update the item statuses to unidentified
                iteminstance.statuses |= ItemStatuses.Unidentified;
                //put the item in the new owners inventory
                itemService.PutLootedItem(iteminstance);

                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, iteminstance);
                router.Send(client, recvItemInstanceUnidentified.ToPacket());
            }
        }
    }
}
