using Necromancy.Server.Model;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Systems.Item
{
    public class ItemService
    {
        private readonly Character _character;
        private readonly IItemDao _itemDao;

        public class MoveResult
        {
            /// <summary>
            /// The type of move that is done. Determines which server responses to send back.
            /// </summary>
            public MoveType Type { get; internal set; } = MoveType.None;
            /// <summary>
            /// The item that is at the location moved from. Can be null if there is no item swapped.
            /// </summary>
            public ItemInstance OriginItem { get; internal set; }
            /// <summary>
            /// The item that is at the destination. Will not be null unless an error occurs.
            /// </summary>
            public ItemInstance DestItem { get; internal set; }
            public MoveResult() { }

            public MoveResult(MoveType moveType)
            {
                Type = moveType;
            }
        }
        public enum MoveType
        {
            Place,
            Swap,
            PlaceQuantity,
            AddQuantity,
            AllQuantity,
            None
        }

        public ItemService(Character character)
        {
            _itemDao = new ItemDao();
            _character = character;
        }

        public ItemService(Character character, IItemDao itemDao)
        {
            _itemDao = itemDao;
            _character = character;
        }

        public ItemInstance Equip(ItemLocation location, ItemEquipSlots equipSlot)
        {
            ItemInstance item = _character.ItemManager.GetItem(location);
            item.CurrentEquipSlot = equipSlot;
            if (_character.EquippedItems.ContainsKey(equipSlot)){
                _character.EquippedItems[equipSlot] = item;
            } else
            {
                _character.EquippedItems.Add(equipSlot, item);
            }
            _itemDao.UpdateItemEquipMask(item.InstanceID, equipSlot);
            return item;
        }
        public ItemInstance Unequip(ItemEquipSlots equipSlot)
        {

            ItemInstance item = _character.EquippedItems[equipSlot];
            _character.EquippedItems.Remove(equipSlot);
            item.CurrentEquipSlot = ItemEquipSlots.None;
            _itemDao.UpdateItemEquipMask(item.InstanceID, ItemEquipSlots.None);
            return item;
        }
        /// <summary>
        /// Creates an unidentified instance of the base item specified by ID in the next open bag slot.
        /// </summary>
        /// <param name="baseId">The ID of the base item being spawned.</param>
        /// <returns>An new instance of the base item that is unidentified. Name will be "? <c>ItemType</c>"</returns>
        /// <exception cref="Necromancy.Server.Systems.Item.ItemException">Thrown when inventory is full or base ID does not exist.</exception>
        public ItemInstance SpawnUnidentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public ItemInstance SpawnIdentifiedItem(int baseId)
        {
            throw new NotImplementedException();
        }
        public List<ItemInstance> SpawnItemInstances(ItemZoneType itemZoneType, int[] baseIds, ItemSpawnParams[] spawnParams)
        {
            if (_character.ItemManager.GetTotalFreeSpace(itemZoneType) < baseIds.Length) throw new ItemException(ItemExceptionType.InventoryFull);
            ItemLocation[] nextOpenLocations = _character.ItemManager.NextOpenSlots(itemZoneType, baseIds.Length);
            List<ItemInstance> itemInstances = _itemDao.InsertItemInstances(_character.Id, nextOpenLocations, baseIds, spawnParams);
            foreach (ItemInstance item in itemInstances)
            {
                _character.ItemManager.PutItem(item.Location, item);
            }
            return itemInstances;
        }
        public List<ItemInstance> GetIdentifiedItems(params long[] spawnIds)
        {
            throw new NotImplementedException();
        }
        public ItemInstance GetIdentifiedItem(ItemLocation location)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A list of items in your adventure bag, equipped bags, bag slot, premium bag, and avatar inventory.</returns>
        public List<ItemInstance> LoadOwnedInventoryItems()
        {
            List<ItemInstance> ownedItems = _itemDao.SelectOwnedInventoryItems(_character.Id);
            //load bags first
            foreach (ItemInstance item in ownedItems)
            {
                if (item.Location.ZoneType == ItemZoneType.BagSlot)
                {
                    ItemLocation location = item.Location; //only needed on load inventory because item's location is already populated and it is not in the manager
                    item.Location = ItemLocation.InvalidLocation; //only needed on load inventory because item's location is already populated and it is not in the manager
                    _character.ItemManager.PutItem(location, item);
                }
            }
            foreach (ItemInstance item in ownedItems)
            {

                if (item.Location.ZoneType != ItemZoneType.BagSlot)
                {
                    ItemLocation location = item.Location; //only needed on load inventory because item's location is already populated and it is not in the manager
                    item.Location = ItemLocation.InvalidLocation; //only needed on load inventory because item's location is already populated and it is not in the manager
                    _character.ItemManager.PutItem(location, item);
                }
                if (item.CurrentEquipSlot != ItemEquipSlots.None)
                {
                    _character.EquippedItems.Add(item.CurrentEquipSlot, item);
                }
            }
            return ownedItems;
        }
        public ItemInstance[] Sort(ItemZoneType zone, byte container, short[] fromSlots, short[] toSlots, short[] quantities)
        {
            ItemInstance[] sortedItems = new ItemInstance[fromSlots.Length];
            ulong[] instanceIds = new ulong[fromSlots.Length];
            ItemLocation[] itemLocations = new ItemLocation[fromSlots.Length];
            for (int i = 0; i < fromSlots.Length; i++)
            {
                ItemLocation loc = new ItemLocation(zone, container, fromSlots[i]);
                if (!_character.ItemManager.HasItem(loc)) throw new ItemException(ItemExceptionType.Generic); //make sure there is an actual item at the location

                sortedItems[i] = _character.ItemManager.RemoveItem(loc);
                if(sortedItems[i].Quantity != quantities[i]) throw new ItemException(ItemExceptionType.Amount); //should never happen as quanties arent even checked besides compromised client
                instanceIds[i] = sortedItems[i].InstanceID;
            }
            for (int i = 0; i < fromSlots.Length; i++)
            {
                itemLocations[i] = new ItemLocation(zone, container, toSlots[i]);
                _character.ItemManager.PutItem(itemLocations[i], sortedItems[i]);
            }
            _itemDao.UpdateItemLocations(instanceIds, itemLocations);
            return sortedItems;
        }
        public ItemInstance Remove(ItemLocation location, byte quantity)
        {
            ItemInstance item = _character.ItemManager.GetItem(location);
            ulong instanceId = item.InstanceID;
            if (item is null) throw new ItemException(ItemExceptionType.Generic);
            if (item.Quantity < quantity) throw new ItemException(ItemExceptionType.Amount);

            item.Quantity -= quantity;
            if (item.Quantity == 0)
            {
                _itemDao.DeleteItemInstance(instanceId);
                _character.ItemManager.RemoveItem(item);
            }
            else
            {
                ulong[] instanceIds = new ulong[1];
                byte[] quantities = new byte[1];
                instanceIds[0] = instanceId;                
                quantities[0] = item.Quantity;
                _itemDao.UpdateItemQuantities(instanceIds, quantities);
            }
            return item;
        }
        public long Sell(ItemLocation location, byte quantity)
        {
            throw new NotImplementedException();
        }
        public MoveResult Move(ItemLocation from, ItemLocation to, byte quantity)
        {
            ItemInstance fromItem = _character.ItemManager.GetItem(from);
            bool hasToItem = _character.ItemManager.HasItem(to);
            ItemInstance toItem = _character.ItemManager.GetItem(to);
            MoveResult moveResult = new MoveResult();

            //check possible errors. these should only occur if client is compromised
            if (fromItem is null || quantity == 0) throw new ItemException(ItemExceptionType.Generic);
            if (quantity > fromItem.Quantity) throw new ItemException(ItemExceptionType.Amount);
            if (quantity > 1 && quantity < fromItem.Quantity && hasToItem && toItem.BaseID != fromItem.BaseID) throw new ItemException(ItemExceptionType.BagLocation);
            if (fromItem.Location.ZoneType == ItemZoneType.BagSlot && !_character.ItemManager.IsEmptyContainer(ItemZoneType.EquippedBags, fromItem.Location.Slot)) throw new ItemException(ItemExceptionType.BagLocation);

            if (!hasToItem && quantity == fromItem.Quantity)
            {
                moveResult = MoveItemPlace(to, fromItem);
            }
            else if (!hasToItem && quantity < fromItem.Quantity)
            {
                moveResult = MoveItemPlaceQuantity(to, fromItem, quantity);
            }
            else if (hasToItem && quantity == fromItem.Quantity && (fromItem.BaseID != toItem.BaseID || fromItem.Quantity == fromItem.MaxStackSize))
            {
                moveResult = MoveItemSwap(from, to, fromItem, toItem);
            }
            else if (hasToItem && quantity < fromItem.Quantity && toItem.BaseID == fromItem.BaseID)
            {
                moveResult = MoveItemAddQuantity(fromItem, toItem, quantity);
            }
            else if (hasToItem && quantity == fromItem.Quantity && toItem.BaseID == fromItem.BaseID && quantity <= (toItem.MaxStackSize - toItem.Quantity))
            {
                moveResult = MoveItemAllQuantity(fromItem, toItem, quantity);
            }

            return moveResult;
        }

        /// <summary>
        /// Used when the there is no item already in the end location and the quantity moved is equal to the total quantity of items in the original location.        
        /// </summary>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move this item.</param>
        /// <returns>Result with origin item null and the destination the moved item.</returns>
        private MoveResult MoveItemPlace(ItemLocation to, ItemInstance fromItem)
        {
            MoveResult moveResult = new MoveResult(MoveType.Place);
            _character.ItemManager.PutItem(to, fromItem);
            moveResult.DestItem = fromItem;

            ulong[] instanceIds = new ulong[1];
            ItemLocation[] locs = new ItemLocation[1];
            instanceIds[0] = moveResult.DestItem.InstanceID;
            locs[0] = moveResult.DestItem.Location;
            _itemDao.UpdateItemLocations(instanceIds, locs);

            return moveResult;
        }
        /// <summary>
        ///  Used when the there is an item already in the end location,the quantity moved is equal to the total quantity<br/>
        ///  of items in the original location, and the item at the end location is a different base item or stacked full.
        /// </summary>
        /// <param name="from">Swap back to this location.</param>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move this item.</param>
        /// <param name="toItem">Swap this item.</param>
        /// <returns>Result with the origin item and destination item being the swapped items.</returns>
        private MoveResult MoveItemSwap(ItemLocation from, ItemLocation to, ItemInstance fromItem, ItemInstance toItem)
        {
            MoveResult moveResult = new MoveResult(MoveType.Swap);
            _character.ItemManager.PutItem(to, fromItem);
            _character.ItemManager.PutItem(from, toItem);
            moveResult.DestItem = fromItem;
            moveResult.OriginItem = toItem;            

            ulong[] instanceIds = new ulong[2];
            ItemLocation[] locs = new ItemLocation[2];
            instanceIds[0] = moveResult.OriginItem.InstanceID;
            locs[0] = moveResult.OriginItem.Location;
            instanceIds[1] = moveResult.DestItem.InstanceID;
            locs[1] = moveResult.DestItem.Location;

            _itemDao.UpdateItemLocations(instanceIds, locs);

            return moveResult;
        }

        /// <summary>
        /// Used when the there is no item already in the end location and the quantity moved is less than total quantity of items in the original location.
        /// </summary>
        /// <param name="to">Move to this location.</param>
        /// <param name="fromItem">Move a quantity from this item.</param>
        /// <returns>Result containing the original item with less quantity and a new instance with the remaining amount at the destination.</returns>
        private MoveResult MoveItemPlaceQuantity(ItemLocation to, ItemInstance fromItem, byte quantity)
        {
            MoveResult moveResult = new MoveResult(MoveType.PlaceQuantity);
            moveResult.OriginItem = fromItem;
            moveResult.OriginItem.Quantity -= quantity;

            ItemSpawnParams spawnParam = new ItemSpawnParams();
            spawnParam.Quantity = quantity;
            spawnParam.ItemStatuses = moveResult.OriginItem.Statuses;

            const int size = 1;
            ItemLocation[] locs = new ItemLocation[size];
            int[] baseIds = new int[size];
            ItemSpawnParams[] spawnParams = new ItemSpawnParams[size];

            locs[0] = to;
            baseIds[0] = moveResult.OriginItem.BaseID;
            spawnParams[0] = spawnParam;

            List<ItemInstance> insertedItem = _itemDao.InsertItemInstances(fromItem.OwnerID, locs, baseIds, spawnParams);
            _character.ItemManager.PutItem(to, insertedItem[0]);
            moveResult.DestItem = insertedItem[0];

            return moveResult;
        }

        /// <summary>
        /// Used if there is the same item at the end location that is not at max stack size and the quantity moved is less than total quantity of items in the original location.<br/>
        /// If the stack would be filled with less than the passed quantity, fill the stack and return leftovers.
        /// </summary>        
        /// <param name="fromItem">Item to subract quantity from.</param>
        /// <param name="toItem">Location of item to add quantity to.</param>
        /// <param name="quantity">Maximum amount to transfer.</param>
        /// <returns>Result containing the original items with updated quantities.</returns>
        private MoveResult MoveItemAddQuantity(ItemInstance fromItem, ItemInstance toItem, byte quantity)
        {
            MoveResult moveResult = new MoveResult(MoveType.AddQuantity);
            moveResult.OriginItem = fromItem;
            moveResult.DestItem = toItem;

            int freeSpace = moveResult.DestItem.MaxStackSize - moveResult.DestItem.Quantity;
            if (freeSpace < quantity)
                quantity = (byte)freeSpace;
            moveResult.OriginItem.Quantity -= quantity;
            moveResult.DestItem.Quantity += quantity;

            ulong[] instanceIds = new ulong[2];
            byte[] quantities = new byte[2];
            instanceIds[0] = moveResult.OriginItem.InstanceID;
            quantities[0] = moveResult.OriginItem.Quantity;
            instanceIds[1] = moveResult.DestItem.InstanceID;
            quantities[1] = moveResult.DestItem.Quantity;
            _itemDao.UpdateItemQuantities(instanceIds, quantities);

            return moveResult;
        }
        /// <summary>
        /// Used if there is the same item at the end location that is not at max stack size and the quantity moved is less equal to the quantity of items in the original location.        
        /// </summary>
        /// <param name="fromItem">Removed item.</param>
        /// <param name="toItem">Updated item.</param>
        /// <param name="quantity">Quantity to add to end item</param>
        /// <returns>Result with the origin item null and the destination item with an updated quantity.</returns>
        private MoveResult MoveItemAllQuantity(ItemInstance fromItem, ItemInstance toItem, byte quantity)
        {
            MoveResult moveResult = new MoveResult(MoveType.AllQuantity);
            moveResult.DestItem = toItem;
            moveResult.DestItem.Quantity += quantity;
            _character.ItemManager.RemoveItem(fromItem.Location);

            ulong[] instanceIds = new ulong[1];
            byte[] quantities = new byte[1];
            instanceIds[0] = moveResult.DestItem.InstanceID;
            quantities[0] = moveResult.DestItem.Quantity;
            //TODO MAKE TRANSACTION
            _itemDao.DeleteItemInstance(fromItem.InstanceID);
            _itemDao.UpdateItemQuantities(instanceIds, quantities);

            return moveResult;
        }

        public List<ItemInstance> Repair(List<ItemLocation> locations)
        {
            throw new NotImplementedException();
        }
        public long SubtractGold(long amount)
        {
            throw new NotImplementedException();
        }
        public long AddGold(long amount)
        {
            throw new NotImplementedException();
        }

        public List<PacketResponse> GetMoveResponses(NecClient client, MoveResult moveResult)
        {
            List<PacketResponse> responses = new List<PacketResponse>();
            switch (moveResult.Type)
            {
                case MoveType.Place:
                    RecvItemUpdatePlace recvItemUpdatePlace = new RecvItemUpdatePlace(client, moveResult.DestItem);
                    responses.Add(recvItemUpdatePlace);
                    break;
                case MoveType.Swap:
                    RecvItemUpdatePlaceChange recvItemUpdatePlaceChange = new RecvItemUpdatePlaceChange(client, moveResult.OriginItem, moveResult.DestItem);
                    responses.Add(recvItemUpdatePlaceChange);
                    break;
                case MoveType.PlaceQuantity:
                    RecvItemUpdateNum recvItemUpdateNum = new RecvItemUpdateNum(client, moveResult.OriginItem);
                    responses.Add(recvItemUpdateNum);
                    RecvItemInstance recvItemInstance = new RecvItemInstance(client, moveResult.DestItem);
                    responses.Add(recvItemInstance);
                    break;
                case MoveType.AddQuantity:
                    RecvItemUpdateNum recvItemUpdateNum0 = new RecvItemUpdateNum(client, moveResult.OriginItem);
                    responses.Add(recvItemUpdateNum0);
                    RecvItemUpdateNum recvItemUpdateNum1 = new RecvItemUpdateNum(client, moveResult.DestItem);
                    responses.Add(recvItemUpdateNum1);
                    break;
                case MoveType.AllQuantity:
                    RecvItemRemove recvItemRemove = new RecvItemRemove(client, moveResult.OriginItem);
                    responses.Add(recvItemRemove);
                    RecvItemUpdateNum recvItemUpdateNum2 = new RecvItemUpdateNum(client, moveResult.DestItem);
                    responses.Add(recvItemUpdateNum2);
                    break;
                case MoveType.None:                    
                    break;
            }
            return responses;
        }
    }
}
