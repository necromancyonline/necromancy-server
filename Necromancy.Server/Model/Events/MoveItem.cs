//using Necromancy.Server.Model.ItemModel;
//using Necromancy.Server.Packet.Id;
//using Necromancy.Server.Packet.Receive;
//
//namespace Necromancy.Server.Model
//{
//    public class MoveItem : Event
//    {
//        public byte
//            toStoreType; // [0 = adventure bag. 1 = character equipment], [then unknown byte], [then slot], [then unknown]
//
//        public byte toBagId { get; set; }
//        public short toSlot { get; set; }
//        public short fromSlot { get; set; }
//        public byte fromStoreType { get; set; }
//        public byte fromBagId { get; set; }
//        public byte itemCount { get; set; }
//        public Item item { get; set; }
//
//        public MoveItem()
//        {
//            EventType = (ushort) AreaPacketId.recv_event_request_int;
//        }
//
//        public void Move(NecServer server, NecClient client)
//        {
//            InventoryItem fromInvItem = client.Character.GetInventoryItem(fromStoreType, fromBagId, fromSlot);
//            InventoryItem toInvItem = client.Character.GetInventoryItem(toStoreType, toBagId, toSlot);
//
//            if (toInvItem != null && (fromInvItem.StorageCount + toInvItem.StorageCount > 255))
//            {
//                RecvNormalSystemMessage unlikeItems =
//                    new RecvNormalSystemMessage("The move would place too many items in destination slot!");
//                server.Router.Send(unlikeItems, client);
//                return;
//            }
//
//            if (fromInvItem.StorageCount == itemCount)
//            {
//                if (toInvItem != null)
//                {
//                    toInvItem.StorageCount =
//                        (byte) (fromInvItem.StorageCount +
//                                toInvItem.StorageCount); // huh??? byte - byte is cast as int???? 
//                    RecvItemUpdateNum itemNum = new RecvItemUpdateNum(toInvItem.InstanceId, toInvItem.StorageCount);
//                    server.Router.Send(itemNum, client);
//                    RecvItemRemove removeItem = new RecvItemRemove(fromInvItem.InstanceId);
//                    server.Router.Send(removeItem, client);
//                    client.Character.RemoveInventoryItem(fromInvItem);
//                    client.Character.UpdateInventoryItem(toInvItem);
//                }
//                else
//                {
//                    fromInvItem.StorageType = toStoreType;
//                    fromInvItem.StorageId = toBagId;
//                    fromInvItem.StorageSlot = toSlot;
//                    RecvItemUpdatePlace itemPlace = new RecvItemUpdatePlace(fromInvItem.InstanceId,
//                        fromInvItem.StorageType, fromInvItem.StorageId, fromInvItem.StorageSlot);
//                    server.Router.Send(itemPlace, client);
//                    client.Character.UpdateInventoryItem(fromInvItem);
//                }
//            }
//            else
//            {
//                byte remainCount =
//                    (byte) (fromInvItem.StorageCount - itemCount); // huh??? byte - byte is cast as int????
//                if (toInvItem != null)
//                {
//                    toInvItem.StorageCount =
//                        (byte) (itemCount + toInvItem.StorageCount); // huh??? byte - byte is cast as int???? 
//                    fromInvItem.StorageCount = remainCount;
//                    RecvItemUpdateNum toItemNum = new RecvItemUpdateNum(toInvItem.InstanceId, toInvItem.StorageCount);
//                    server.Router.Send(toItemNum, client);
//                    RecvItemUpdateNum fromItemNum =
//                        new RecvItemUpdateNum(fromInvItem.InstanceId, fromInvItem.StorageCount);
//                    server.Router.Send(fromItemNum, client);
//                    client.Character.RemoveInventoryItem(fromInvItem);
//                    client.Character.UpdateInventoryItem(toInvItem);
//                }
//                else
//                {
//                    RecvItemUpdateNum updateNum = new RecvItemUpdateNum(fromInvItem.InstanceId, remainCount);
//                    server.Router.Send(updateNum, client);
//                    InventoryItem newInvItem = client.Character.GetNextInventoryItem(server);
//                    newInvItem.StorageCount = (byte) itemCount;
//                    newInvItem.StorageType = toStoreType;
//                    newInvItem.StorageId = toBagId;
//                    newInvItem.StorageSlot = toSlot;
//                    newInvItem.StorageItem = item;
//                    RecvItemInstanceUnidentified itemUnidentified = new RecvItemInstanceUnidentified(newInvItem);
//                    server.Router.Send(itemUnidentified, client);
//                    fromInvItem.StorageCount = remainCount;
//                    client.Character.UpdateInventoryItem(fromInvItem);
//                }
//            }
//        }
//    }
//}



