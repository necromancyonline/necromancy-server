using System;
using System.Collections.Generic;

namespace Necromancy.Server.Systems.Item
{
    /// <summary>
    ///     Holds item cache in memory.<br /> <br />
    ///     Stores information about published items, and their locations. <b>Does not validate any actions.</b>
    ///     Do not access from other clients, does not function.
    /// </summary>
    public class ItemLocationVerifier
    {
        private const int MAX_CONTAINERS_ADV_BAG = 1;
        private const int MAX_CONTAINER_SIZE_ADV_BAG = 24;

        private const int MAX_CONTAINERS_EQUIPPED_BAGS = 7;
        private const int MAX_CONTAINER_EQUIPPED_BAGS = 24;

        private const int MAX_CONTAINERS_AVATAR = 9;
        private const int MAX_CONTAINER_SIZE_AVATAR = 50;

        private const int MAX_CONTAINERS_BAG_SLOT = 1;
        private const int MAX_CONTAINER_SIZE_BAG_SLOT = 7;

        private const int MAX_CONTAINERS_ROYAL_BAG = 1;
        private const int MAX_CONTAINER_SIZE_ROYAL_BAG = 24;

        private const int MAX_CONTAINERS_TREASURE_BOX = 1;
        private const int MAX_CONTAINER_SIZE_TREASURE_BOX = 10;

        private const int MAX_CONTAINERS_WAREHOUSE = 27;
        private const int MAX_CONTAINER_SIZE_WAREHOUSE = 50;

        private const int MAX_CONTAINERS_AUCTION_LOTS = 1;
        private const int MAX_CONTAINER_SIZE_AUCTION_LOTS = 15;

        private const int MAX_CONTAINERS_AUCTION_BIDS = 1; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int MAX_CONTAINER_SIZE_AUCTION_BIDS = 15; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private const int MAX_CONTAINERS_AUCTION_SEARCH = 1; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int MAX_CONTAINER_SIZE_AUCTION_SEARCH = 1000; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private readonly Dictionary<ItemZoneType, ItemZone> _zoneMap = new Dictionary<ItemZoneType, ItemZone>();

        public ItemLocationVerifier()
        {
            _zoneMap.Add(ItemZoneType.AdventureBag, new ItemZone(MAX_CONTAINERS_ADV_BAG, MAX_CONTAINER_SIZE_ADV_BAG));
            _zoneMap[ItemZoneType.AdventureBag].PutContainer(0, MAX_CONTAINER_SIZE_ADV_BAG);

            _zoneMap.Add(ItemZoneType.EquippedBags, new ItemZone(MAX_CONTAINERS_EQUIPPED_BAGS, MAX_CONTAINER_EQUIPPED_BAGS));

            _zoneMap.Add(ItemZoneType.PremiumBag, new ItemZone(MAX_CONTAINERS_ROYAL_BAG, MAX_CONTAINER_SIZE_ROYAL_BAG));
            _zoneMap.Add(ItemZoneType.BagSlot, new ItemZone(MAX_CONTAINERS_BAG_SLOT, MAX_CONTAINER_SIZE_BAG_SLOT));
            _zoneMap[ItemZoneType.BagSlot].PutContainer(0, MAX_CONTAINER_SIZE_BAG_SLOT);

            _zoneMap.Add(ItemZoneType.AvatarInventory, new ItemZone(MAX_CONTAINERS_AVATAR, MAX_CONTAINER_SIZE_AVATAR));
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(0, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(1, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(2, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(3, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(4, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(5, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(6, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(7, MAX_CONTAINER_SIZE_AVATAR);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(8, MAX_CONTAINER_SIZE_AVATAR);

            _zoneMap.Add(ItemZoneType.TreasureBox, new ItemZone(MAX_CONTAINERS_TREASURE_BOX, MAX_CONTAINER_SIZE_TREASURE_BOX));
            _zoneMap.Add(ItemZoneType.Warehouse, new ItemZone(MAX_CONTAINERS_WAREHOUSE, MAX_CONTAINER_SIZE_WAREHOUSE));
            _zoneMap[ItemZoneType.Warehouse].PutContainer(0, MAX_CONTAINER_SIZE_WAREHOUSE);

            _zoneMap.Add(ItemZoneType.ProbablyAuctionLots, new ItemZone(MAX_CONTAINERS_AUCTION_LOTS, MAX_CONTAINER_SIZE_AUCTION_LOTS));
            _zoneMap[ItemZoneType.ProbablyAuctionLots].PutContainer(0, MAX_CONTAINER_SIZE_AUCTION_LOTS);
        }

        public ItemInstance GetItem(ItemLocation loc)
        {
            if (loc.Equals(ItemLocation.InvalidLocation)) return null;
            return _zoneMap[loc.zoneType].GetContainer(loc.container).GetItem(loc.slot);
        }

        public bool HasItem(ItemLocation loc)
        {
            if (!_zoneMap.ContainsKey(loc.zoneType)) return false;
            if (_zoneMap[loc.zoneType].GetContainer(loc.container) == null) return false;
            if (_zoneMap[loc.zoneType].GetContainer(loc.container).GetItem(loc.slot) == null) return false;
            return true;
        }

        public void PutItem(ItemLocation loc, ItemInstance item)
        {
            RemoveItem(item);
            item.location = loc;

            switch (loc.zoneType)
            {
                case ItemZoneType.BagSlot:
                {
                    _zoneMap[ItemZoneType.EquippedBags].PutContainer(loc.slot, item.bagSize);
                    _zoneMap[loc.zoneType].GetContainer(loc.container).PutItem(loc.slot, item);
                    break;
                }
                default:
                {
                    _zoneMap[loc.zoneType].GetContainer(loc.container).PutItem(loc.slot, item);
                    break;
                }
            }
        }

        public void RemoveItem(ItemInstance item)
        {
            if (item.location.Equals(ItemLocation.InvalidLocation)) return;

            switch (item.location.zoneType)
            {
                case ItemZoneType.BagSlot:
                {
                    _zoneMap[ItemZoneType.EquippedBags].RemoveContainer(item.location.slot);
                    _zoneMap[item.location.zoneType]?.GetContainer(item.location.container)?.RemoveItem(item.location.slot);
                    break;
                }
                default:
                {
                    _zoneMap[item.location.zoneType]?.GetContainer(item.location.container)?.RemoveItem(item.location.slot);
                    break;
                }
            }

            item.location = ItemLocation.InvalidLocation;
        }


        public ItemInstance RemoveItem(ItemLocation loc)
        {
            ItemInstance item = GetItem(loc);
            if (item != null) RemoveItem(item);
            return item;
        }

        public ItemLocation PutItemInNextOpenSlot(ItemZoneType itemZoneType, ItemInstance item)
        {
            ItemLocation nextOpenSlot = NextOpenSlot(itemZoneType);
            if (!nextOpenSlot.Equals(ItemLocation.InvalidLocation)) PutItem(nextOpenSlot, item);
            return nextOpenSlot;
        }

        /// <summary>
        ///     Finds the next open slot in adventure bag, equipped bags, and premium bag, in that order.
        /// </summary>
        /// <returns></returns>
        public ItemLocation NextOpenSlotInInventory()
        {
            ItemLocation nextOpenSlot = ItemLocation.InvalidLocation;
            nextOpenSlot = NextOpenSlot(ItemZoneType.AdventureBag);
            if (!nextOpenSlot.Equals(ItemLocation.InvalidLocation)) return nextOpenSlot;
            nextOpenSlot = NextOpenSlot(ItemZoneType.EquippedBags);
            if (!nextOpenSlot.Equals(ItemLocation.InvalidLocation)) return nextOpenSlot;
            nextOpenSlot = NextOpenSlot(ItemZoneType.PremiumBag);
            if (!nextOpenSlot.Equals(ItemLocation.InvalidLocation)) return nextOpenSlot;
            return nextOpenSlot;
        }

        public ItemLocation NextOpenSlot(ItemZoneType itemZoneType)
        {
            int nextContainerWithSpace = _zoneMap[itemZoneType].nextContainerWithSpace;
            if (nextContainerWithSpace != ItemZone.NO_CONTAINERS_WITH_SPACE)
            {
                int nextOpenSlot = _zoneMap[itemZoneType].GetContainer(nextContainerWithSpace).nextOpenSlot;
                if (nextOpenSlot != Container.NO_OPEN_SLOTS)
                {
                    ItemLocation itemLocation = new ItemLocation(itemZoneType, (byte)nextContainerWithSpace, (short)nextOpenSlot);
                    return itemLocation;
                }
            }

            return ItemLocation.InvalidLocation;
        }

        public ItemLocation[] NextOpenSlots(ItemZoneType itemZoneType, int amount)
        {
            if (amount > _zoneMap[itemZoneType].totalFreeSpace) throw new ArgumentOutOfRangeException("Not enough open slots");
            return _zoneMap[itemZoneType].GetNextXFreeSpaces(itemZoneType, amount);
        }

        public int GetTotalFreeSpace(ItemZoneType itemZoneType)
        {
            return _zoneMap[itemZoneType].totalFreeSpace;
        }

        public bool IsEmptyContainer(ItemZoneType itemZoneType, int container)
        {
            return _zoneMap[itemZoneType].GetContainer(container).count == 0;
        }

        public ItemInstance GetItemByInstanceId(ulong instanceId)
        {
            foreach (ItemZone itemZone in _zoneMap.Values)
            foreach (Container container in itemZone.containers)
            {
                if (container == null) continue;
                foreach (ItemInstance itemInstance in container.Slots)
                {
                    if (itemInstance == null) continue;
                    if (itemInstance.instanceId == instanceId) return itemInstance;
                }
            }

            return null;
        }
    }
}
