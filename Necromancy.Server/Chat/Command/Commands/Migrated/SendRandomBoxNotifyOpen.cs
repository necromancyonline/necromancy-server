using System.Collections.Generic;
using Necromancy.Server.Model;

namespace Necromancy.Server.Chat.Command.Commands
{
    public class SendRandomBoxNotifyOpen : ServerChatCommand
    {
        public SendRandomBoxNotifyOpen(NecServer server) : base(server)
        {
        }

        //public long ItemGenerator (int itemId, NecClient client,int i)
        //{
        //    Item item = Server.Items[itemId];
        //    InventoryItem inventoryItem = new InventoryItem();
        //    inventoryItem.Id = 50000 + i;
        //    inventoryItem.Item = item;
        //    inventoryItem.ItemId = item.Id;
        //    inventoryItem.Quantity = 1;
        //    inventoryItem.CurrentDurability = item.Durability;
        //    inventoryItem.CharacterId = client.Character.Id;
        //    inventoryItem.CurrentEquipmentSlotType = EquipmentSlotType.NONE;
        //    inventoryItem.State = 0;
        //    inventoryItem.StorageType = (int)BagType.AvatarInventory;
        //    client.Character.Inventory.AddAvatarItem(inventoryItem);

        //    RecvItemInstance recvItemInstance = new RecvItemInstance(inventoryItem, client);
        //    Router.Send(recvItemInstance, client);
        //    RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(inventoryItem, client);
        //    Router.Send(recvItemInstanceUnidentified, client);
        //    return inventoryItem.Id;
        //}

        public override AccountStateType accountState => AccountStateType.Admin;
        public override string key => "rbox";

        public override void Execute(string[] command, NecClient client, ChatMessage message,
            List<ChatResponse> responses)
        {
            ////recv_random_box_notify_open = 0xC374,
            //IBuffer res = BufferProvider.Provide();

            //int numEntries = 10; // Slots
            //res.WriteInt32(numEntries); //less than or equal to 10

            //// Weapon
            //int itemId = 90031008;

            //for (int i = 0; i < numEntries; i++)
            //{
            //    res.WriteInt64(ItemGenerator(itemId,client,i)); // item instance ID per slot
            //}
            //res.WriteInt32(itemId); // Show item name as RBox title. grabs string from itemInfo.csv
            //Router.Send(client, (ushort) AreaPacketId.recv_random_box_notify_open, res, ServerType.Area); // Trying to spawn item in this boxe, maybe i need the item instance ?
        }
    }
}
