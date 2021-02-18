using Arrowgene.Logging;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.Stats;
using Necromancy.Server.Packet;
using Necromancy.Server.Packet.Receive.Area;
using System;
using System.Collections.Generic;

namespace Necromancy.Server.Systems.Item
{
    public class ItemService
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(ItemService));

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

        internal ItemInstance GetIdentifiedItem(ItemLocation location)
        {
            throw new NotImplementedException();
        }
        internal ItemInstance GetLootedItem(ItemLocation location)
        {
            ItemInstance item = _character.ItemManager.GetItem(location);
            if (item.CurrentEquipSlot != ItemEquipSlots.None)
            {
                Unequip(item.CurrentEquipSlot);
            }
            Remove(item.Location, item.Quantity);
            return item;
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
            if (_character.EquippedItems.ContainsKey(equipSlot))
            {
                _character.EquippedItems[equipSlot] = item;
            } 
            else
            {
                _character.EquippedItems.Add(equipSlot, item);
            }
            _itemDao.UpdateItemEquipMask(item.InstanceID, equipSlot);
            return item;
        }
        public ItemInstance CheckAlreadyEquipped(ItemEquipSlots equipmentSlotType)
        {
            ItemInstance itemInstance = null;
            if (equipmentSlotType == ItemEquipSlots.LeftHand | equipmentSlotType == ItemEquipSlots.RightHand)
            {
                if (_character.EquippedItems.ContainsKey(ItemEquipSlots.LeftHand | ItemEquipSlots.RightHand))
                {
                    _character.EquippedItems.TryGetValue((ItemEquipSlots.LeftHand | ItemEquipSlots.RightHand), out itemInstance);
                }
                else
                {
                    _character.EquippedItems.TryGetValue(equipmentSlotType, out itemInstance);
                }
            }
            else
            {
                _character.EquippedItems.TryGetValue(equipmentSlotType, out itemInstance);
            }
            return itemInstance;
        }
        /// <returns></returns>
        public ItemInstance Unequip(ItemEquipSlots equipSlot)
        {

            ItemInstance item = _character.EquippedItems[equipSlot];
            _character.EquippedItems.Remove(equipSlot);
            item.CurrentEquipSlot = ItemEquipSlots.None;
            _itemDao.UpdateItemEquipMask(item.InstanceID, ItemEquipSlots.None);
            return item;
        }
        public ItemInstance PutLootedItem(ItemInstance itemInstance) //I hate this the most. it's so aweful and shoehorned. replace immediatly.
        {
            ItemInstance myNewItem = itemInstance;
            ItemSpawnParams SpawnParams = new ItemSpawnParams()
            {
                plus_gp = itemInstance.PlusGP,
                plus_magical = itemInstance.PlusMagical,
                plus_maximum_durability = itemInstance.PlusDurability,
                plus_physical = itemInstance.PlusPhysical,
                plus_ranged_eff = itemInstance.PlusRangedEff,
                plus_reservoir_eff = itemInstance.PlusReservoirEff,
                plus_weight = itemInstance.PlusWeight,
                Quantity = itemInstance.Quantity,
                ItemStatuses = itemInstance.Statuses |= ItemStatuses.Unidentified
            };
            ItemSpawnParams[] spawnParamsA = new ItemSpawnParams[1] { SpawnParams };
            int[] ids = new int[1] { (int)itemInstance.InstanceID };
            //ToDo,  make this find space in more than just your adventure bag.
            ItemLocation[] nextOpenLocations = _character.ItemManager.NextOpenSlots(ItemZoneType.AdventureBag, 1);
            //ToDo,  implement a single InsertItemIstance DAO so this whole thing can be simplified
            List<ItemInstance> itemInstances = _itemDao.InsertItemInstances(_character.Id, nextOpenLocations, ids, spawnParamsA);
            foreach (ItemInstance item in itemInstances)
            {
                _character.ItemManager.PutItem(item.Location, item);
                myNewItem = item;
            }
            return myNewItem;
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A list of items in your adventure bag, equipped bags, bag slot, premium bag, and avatar inventory.</returns>
        public List<ItemInstance> LoadOwneditemInstances(NecServer server)
        {
            //Clear Equipped Items from send_data_get_self_chara_data_request
            _character.EquippedItems.Clear();
            List<ItemInstance> ownedItems = _itemDao.SelectOwneditemInstances(_character.Id);
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
            foreach (ItemInstance itemInstance in ownedItems)
            {

                if (itemInstance.Location.ZoneType != ItemZoneType.BagSlot)
                {
                    ItemLocation location = itemInstance.Location; //only needed on load inventory because item's location is already populated and it is not in the manager
                    itemInstance.Location = ItemLocation.InvalidLocation; //only needed on load inventory because item's location is already populated and it is not in the manager

                    //Temporary ItemLibrary.CSV lookup until Item_decrypted.csv and Table are fully mapped/ populated
                     server.SettingRepository.ItemLibrary.TryGetValue(itemInstance.BaseID, out ItemLibrarySetting itemLibrarySetting);
                    if (itemLibrarySetting != null)
                    {
                        itemInstance.MaximumDurability = itemLibrarySetting.Durability; //Temporary until we get durability in itemLibrary
                        if (itemInstance.CurrentDurability > itemInstance.MaximumDurability) { itemInstance.CurrentDurability = itemInstance.MaximumDurability; }
                        if (itemInstance.Weight == 0) { itemInstance.Weight += 1234; }
                        if (itemInstance.Type == ItemType.SHIELD_LARGE || itemInstance.Type == ItemType.SHIELD_MEDIUM || itemInstance.Type == ItemType.SHIELD_SMALL)
                        {
                            if (itemInstance.GP == 0)
                            {
                            itemInstance.GP += 50;
                            }
                        }
                    }
                    //update items base stats per enchantment level.
                    ForgeMultiplier forgeMultiplier = this.LoginLoadMultiplier(itemInstance.EnhancementLevel);
                    itemInstance.Physical = (short)(itemInstance.Physical * forgeMultiplier.Factor);
                    itemInstance.Magical = (short)(itemInstance.Magical * forgeMultiplier.Factor);
                    itemInstance.MaximumDurability = (short)(itemInstance.MaximumDurability * forgeMultiplier.Durability);
                    itemInstance.Hardness = (byte)(itemInstance.Hardness + forgeMultiplier.Hardness);
                    itemInstance.Weight = (short)(itemInstance.Weight - forgeMultiplier.Weight);
                    if (itemInstance.Weight < 0) { itemInstance.Weight = 0; } //this is lazy, fix the weight math issue.

                    _character.ItemManager.PutItem(location, itemInstance);
                }
                if (itemInstance.CurrentEquipSlot != ItemEquipSlots.None)
                {
                    _character.EquippedItems.Add(itemInstance.CurrentEquipSlot, itemInstance);
                }
            }
            return ownedItems;
        }

        public List<ItemInstance> LoadEquipmentModels()
        {
            _character.EquippedItems.Clear();
            List<ItemInstance> ownedItems = _itemDao.SelectOwneditemInstances(_character.Id);
            foreach (ItemInstance item in ownedItems)
            {
                if (item.CurrentEquipSlot != ItemEquipSlots.None)
                {
                    if (!_character.EquippedItems.ContainsKey(item.CurrentEquipSlot))
                    {
                        _character.EquippedItems.Add(item.CurrentEquipSlot, item);
                    }
                    else
                    {
                        //Clean up duplicate equipped items since we don't have a unique constraint on table
                        item.CurrentEquipSlot = ItemEquipSlots.None;
                        _itemDao.UpdateItemEquipMask(item.InstanceID, ItemEquipSlots.None);
                    }
                }
            }
            return ownedItems;
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

        public ForgeMultiplier LoginLoadMultiplier(int level)
        {
            double factor = 1;
            double durability = 1;
            int hardness = 0;
            switch (level)
            {
                case 0:     factor = 1.00; durability = 1.0; hardness = 0; break;
                case 1:     factor = 1.05; durability = 1.1; hardness = 0; break;
                case 2:     factor = 1.15; durability = 1.2; hardness = 0; break;
                case 3:     factor = 1.27; durability = 1.3; hardness = 0; break;
                case 4:     factor = 1.39; durability = 1.4; hardness = 0; break;
                case 5:     factor = 1.54; durability = 1.5; hardness = 1; break;
                case 6:     factor = 1.69; durability = 1.6; hardness = 0; break;
                case 7:     factor = 1.84; durability = 1.7; hardness = 0; break;
                case 8:     factor = 1.99; durability = 1.8; hardness = 0; break;
                case 9:     factor = 2.14; durability = 1.9; hardness = 0; break;
                case 10:    factor = 2.29; durability = 2.0; hardness = 2; break;
                default: break;
            }
            ForgeMultiplier forgeMultiplier = new ForgeMultiplier();
            forgeMultiplier.Factor = factor;
            forgeMultiplier.Durability = durability;
            forgeMultiplier.Hardness = hardness;
            forgeMultiplier.Weight = 100; //toDo
            return forgeMultiplier;
        }
        public ForgeMultiplier ForgeMultiplier(int level)
        {
            double factor = 1;
            double durability = 1;
            int hardness = 0;
            switch (level)
            {
                case 0: factor = 1.00; durability = 1.0; hardness = 0; break;
                case 1: factor = 1.05; durability = 1.1; hardness = 0; break;
                case 2: factor = 1.10; durability = 1.1; hardness = 0; break;
                case 3: factor = 1.12; durability = 1.1; hardness = 0; break;
                case 4: factor = 1.12; durability = 1.1; hardness = 0; break;
                case 5: factor = 1.15; durability = 1.1; hardness = 1; break;
                case 6: factor = 1.15; durability = 1.1; hardness = 0; break;
                case 7: factor = 1.15; durability = 1.1; hardness = 0; break;
                case 8: factor = 1.15; durability = 1.1; hardness = 0; break;
                case 9: factor = 1.15; durability = 1.1; hardness = 0; break;
                case 10: factor = 1.15; durability = 1.1; hardness = 1; break;
                default: factor = 1.00; durability = 1.0; hardness = 0; break;
            }
            ForgeMultiplier forgeMultiplier = new ForgeMultiplier();
            forgeMultiplier.Factor = factor;
            forgeMultiplier.Durability = durability;
            forgeMultiplier.Hardness = hardness;
            forgeMultiplier.Weight = 100; //toDo
            return forgeMultiplier;
        }
        public void UpdateEnhancementLevel(ItemInstance itemInstance)
        {
            _itemDao.UpdateItemEnhancementLevel(itemInstance.InstanceID, itemInstance.EnhancementLevel);
        }

        public List<ItemInstance> Repair(List<ItemLocation> locations)
        {
            List<ItemInstance> ItemInstances = new List<ItemInstance>();
            foreach (ItemLocation location in locations)
            {
                ItemInstance itemInstance = _character.ItemManager.GetItem(location);
                ItemInstances.Add(itemInstance);
                _itemDao.UpdateItemCurrentDurability(itemInstance.InstanceID, itemInstance.MaximumDurability);
            }
            return ItemInstances;
        }
        public long SubtractGold(long amount)
        {
            return 0;//throw new NotImplementedException();
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

        public List<PacketResponse>CalculateBattleStats(NecClient client)
        {
            List<PacketResponse> responses = new List<PacketResponse>();
            BattleParam battleParam = new BattleParam();

            client.Character.Weight.setCurrent(0);
            client.Character.Gp.setMax(0);
            bool ShieldCheck = false;

            foreach (ItemInstance itemInstance in client.Character.EquippedItems.Values)
            {
                if (itemInstance.CurrentEquipSlot.HasFlag(ItemEquipSlots.RightHand) | itemInstance.CurrentEquipSlot == ItemEquipSlots.Quiver)
                {
                    battleParam.PlusPhysicalAttack += (short)(itemInstance.Physical + itemInstance.PlusPhysical);
                    battleParam.PlusMagicalAttack += (short)(itemInstance.Magical + itemInstance.PlusMagical);
                }
                else
                {
                    battleParam.PlusPhysicalDefence += (short)(itemInstance.Physical + itemInstance.PlusPhysical);
                    battleParam.PlusMagicalDefence += (short)(itemInstance.Magical + itemInstance.PlusMagical);
                }
                client.Character.Gp.setMax(client.Character.Gp.max + itemInstance.GP + itemInstance.PlusGP);
                client.Character.Weight.Modify(itemInstance.Weight + itemInstance.PlusWeight);
                if (itemInstance.Type == ItemType.SHIELD_LARGE | itemInstance.Type == ItemType.SHIELD_MEDIUM | itemInstance.Type == ItemType.SHIELD_SMALL)
                { ShieldCheck = true; }
            }

            //if you dont have a shield on,  set your GP to 0.  no blocking for you
            if (ShieldCheck == false) 
            { 
                client.Character.Gp.setMax(0);
                RecvCharaUpdateAc recvCharaUpdateAc = new RecvCharaUpdateAc(client.Character.Gp.max);
                responses.Add(recvCharaUpdateAc);
            }

            RecvCharaUpdateMaxWeight recvCharaUpdateMaxWeight = new RecvCharaUpdateMaxWeight(client.Character.Weight.max/10, client.Character.Weight.current/10/*Weight.Diff*/);
            responses.Add(recvCharaUpdateMaxWeight);

            RecvCharaUpdateWeight recvCharaUpdateWeight = new RecvCharaUpdateWeight(client.Character.Weight.current/10);
            responses.Add(recvCharaUpdateWeight);

            RecvCharaUpdateMaxAc recvCharaUpdateMaxAc = new RecvCharaUpdateMaxAc(client.Character.Gp.max);
            responses.Add(recvCharaUpdateMaxAc);

            RecvCharaUpdateBattleBaseParam recvCharaUpdateBattleBaseParam = new RecvCharaUpdateBattleBaseParam(client.Character, battleParam);
            responses.Add(recvCharaUpdateBattleBaseParam);

            return responses;
        }


    }
}
