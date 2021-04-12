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
        private const int _MaxContainersAdvBag = 1;
        private const int _MaxContainerSizeAdvBag = 24;

        private const int _MaxContainersEquippedBags = 7;
        private const int _MaxContainerEquippedBags = 24;

        private const int _MaxContainersAvatar = 9;
        private const int _MaxContainerSizeAvatar = 50;

        private const int _MaxContainersBagSlot = 1;
        private const int _MaxContainerSizeBagSlot = 7;

        private const int _MaxContainersRoyalBag = 1;
        private const int _MaxContainerSizeRoyalBag = 24;

        private const int _MaxContainersTreasureBox = 1;
        private const int _MaxContainerSizeTreasureBox = 10;

        private const int _MaxContainersWarehouse = 27;
        private const int _MaxContainerSizeWarehouse = 50;

        private const int _MaxContainersAuctionLots = 1;
        private const int _MaxContainerSizeAuctionLots = 15;

        private const int _MaxContainersAuctionBids = 1; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int _MaxContainerSizeAuctionBids = 15; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private const int _MaxContainersAuctionSearch = 1; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int _MaxContainerSizeAuctionSearch = 1000; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private readonly Dictionary<ItemZoneType, ItemZone> _zoneMap = new Dictionary<ItemZoneType, ItemZone>();

        public ItemLocationVerifier()
        {
            _zoneMap.Add(ItemZoneType.AdventureBag, new ItemZone(_MaxContainersAdvBag, _MaxContainerSizeAdvBag));
            _zoneMap[ItemZoneType.AdventureBag].PutContainer(0, _MaxContainerSizeAdvBag);

            _zoneMap.Add(ItemZoneType.EquippedBags, new ItemZone(_MaxContainersEquippedBags, _MaxContainerEquippedBags));

            _zoneMap.Add(ItemZoneType.PremiumBag, new ItemZone(_MaxContainersRoyalBag, _MaxContainerSizeRoyalBag));
            _zoneMap.Add(ItemZoneType.BagSlot, new ItemZone(_MaxContainersBagSlot, _MaxContainerSizeBagSlot));
            _zoneMap[ItemZoneType.BagSlot].PutContainer(0, _MaxContainerSizeBagSlot);

            _zoneMap.Add(ItemZoneType.AvatarInventory, new ItemZone(_MaxContainersAvatar, _MaxContainerSizeAvatar));
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(0, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(1, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(2, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(3, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(4, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(5, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(6, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(7, _MaxContainerSizeAvatar);
            _zoneMap[ItemZoneType.AvatarInventory].PutContainer(8, _MaxContainerSizeAvatar);

            _zoneMap.Add(ItemZoneType.TreasureBox, new ItemZone(_MaxContainersTreasureBox, _MaxContainerSizeTreasureBox));
            _zoneMap.Add(ItemZoneType.Warehouse, new ItemZone(_MaxContainersWarehouse, _MaxContainerSizeWarehouse));
            _zoneMap[ItemZoneType.Warehouse].PutContainer(0, _MaxContainerSizeWarehouse);

            _zoneMap.Add(ItemZoneType.ProbablyAuctionLots, new ItemZone(_MaxContainersAuctionLots, _MaxContainerSizeAuctionLots));
            _zoneMap[ItemZoneType.ProbablyAuctionLots].PutContainer(0, _MaxContainerSizeAuctionLots);
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
            if (nextContainerWithSpace != ItemZone.NoContainersWithSpace)
            {
                int nextOpenSlot = _zoneMap[itemZoneType].GetContainer(nextContainerWithSpace).nextOpenSlot;
                if (nextOpenSlot != Container.NoOpenSlots)
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
                foreach (ItemInstance itemInstance in container.slots)
                {
                    if (itemInstance == null) continue;
                    if (itemInstance.instanceId == instanceId) return itemInstance;
                }
            }

            return null;
        }
    }
}
