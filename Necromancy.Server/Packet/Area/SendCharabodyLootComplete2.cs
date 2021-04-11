using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_loot_complete2 : ClientHandler
    {
        private NecServer _server;
        public send_charabody_loot_complete2(NecServer server) : base(server)
        {
            _server = server;
        }


        public override ushort Id => (ushort) AreaPacketId.send_charabody_loot_complete2;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Map.DeadBodies.TryGetValue(client.Character.eventSelectReadyCode, out DeadBody deadBody);
            Character deadCharacter = _server.Instances.GetCharacterByInstanceId(deadBody.CharacterInstanceId); 
            //Todo - server or map needs to maintain characters in memory for a period of time after disconnect
            NecClient deadClient = _server.Clients.GetByCharacterInstanceId(deadBody.CharacterInstanceId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //result, 0 sucess.  interupted, etc.
            res.WriteFloat(0); // time remaining
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_loot_complete2_r, res, ServerType.Area);

            if (deadClient != null)
            {
                res = BufferProvider.Provide();
                res.WriteByte((byte)deadCharacter.lootNotify.ZoneType);
                res.WriteByte(deadCharacter.lootNotify.Container);
                res.WriteInt16(deadCharacter.lootNotify.Slot);

                res.WriteInt16(1/*iteminstance.Quantity*/); //Number here is "pieces" 
                res.WriteCString($"{client.Soul.Name}"); // soul name
                res.WriteCString($"{client.Character.Name}"); // chara name
                Router.Send(deadClient, (ushort)AreaPacketId.recv_charabody_notify_loot_item, res, ServerType.Area);
            }

            //if (successfull loot condition here)
            {
                ItemService itemService = new ItemService(client.Character);
                ItemService deadCharacterItemService = new ItemService(deadCharacter);

                ItemInstance iteminstance = deadCharacterItemService.GetLootedItem(deadCharacter.lootNotify);
                //remove the icon from the deadClient's inventory if they are online.
                RecvItemRemove recvItemRemove = new RecvItemRemove(deadClient, iteminstance);
                if (deadClient != null) Router.Send(recvItemRemove);

                //Add RecvItemRemove to remove the icon from the charabody window on successfull loot as well.//ToDo - this didnt work
                RecvItemRemove recvItemRemove2 = new RecvItemRemove(client, iteminstance);
                Router.Send(recvItemRemove2);

                //update the item statuses to unidentified
                iteminstance.Statuses |= ItemStatuses.Unidentified;
                //put the item in the new owners inventory
                itemService.PutLootedItem(iteminstance);

                RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, iteminstance);
                Router.Send(client, recvItemInstanceUnidentified.ToPacket());
            }
        }
    }
}
