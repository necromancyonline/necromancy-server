using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    /// <summary>
    /// Holds item cache in memory.<br/> <br/>
    /// Stores information about published items, and their locations. <b>Does not validate any actions.</b>
    /// Do not access from other clients, does not function.
    /// </summary>
    public class ItemManager
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

        private const int MAX_CONTAINERS_AUCTION_BIDS = 1;          //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int MAX_CONTAINER_SIZE_AUCTION_BIDS = 15;     //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private const int MAX_CONTAINERS_AUCTION_SEARCH = 1;        //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY
        private const int MAX_CONTAINER_SIZE_AUCTION_SEARCH = 1000; //DO NOT POPULATE IN ITEM MANAGER, ZONE IS FOR CLIENT DISPLAY ONLY

        private Dictionary<ItemZoneType, ItemZone> ZoneMap = new Dictionary<ItemZoneType, ItemZone>();

        public ItemManager()
        {
            ZoneMap.Add(ItemZoneType.AdventureBag, new ItemZone(MAX_CONTAINERS_ADV_BAG, MAX_CONTAINER_SIZE_ADV_BAG));
            ZoneMap[ItemZoneType.AdventureBag].PutContainer(0, MAX_CONTAINER_SIZE_ADV_BAG);

            ZoneMap.Add(ItemZoneType.EquippedBags, new ItemZone(MAX_CONTAINERS_EQUIPPED_BAGS, MAX_CONTAINER_EQUIPPED_BAGS));

            ZoneMap.Add(ItemZoneType.PremiumBag, new ItemZone(MAX_CONTAINERS_ROYAL_BAG, MAX_CONTAINER_SIZE_ROYAL_BAG));
            ZoneMap.Add(ItemZoneType.BagSlot, new ItemZone(MAX_CONTAINERS_BAG_SLOT, MAX_CONTAINER_SIZE_BAG_SLOT));
            ZoneMap[ItemZoneType.BagSlot].PutContainer(0, MAX_CONTAINER_SIZE_BAG_SLOT);

            ZoneMap.Add(ItemZoneType.AvatarInventory, new ItemZone(MAX_CONTAINERS_AVATAR, MAX_CONTAINER_SIZE_AVATAR));
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(0, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(1, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(2, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(3, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(4, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(5, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(6, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(7, MAX_CONTAINER_SIZE_AVATAR);
            ZoneMap[ItemZoneType.AvatarInventory].PutContainer(8, MAX_CONTAINER_SIZE_AVATAR);

            ZoneMap.Add(ItemZoneType.TreasureBox, new ItemZone(MAX_CONTAINERS_TREASURE_BOX, MAX_CONTAINER_SIZE_TREASURE_BOX));

            ZoneMap.Add(ItemZoneType.Warehouse, new ItemZone(MAX_CONTAINERS_WAREHOUSE, MAX_CONTAINER_SIZE_WAREHOUSE));
            ZoneMap[ItemZoneType.Warehouse].PutContainer(0, MAX_CONTAINER_SIZE_WAREHOUSE);

            ZoneMap.Add(ItemZoneType.ProbablyAuctionLots, new ItemZone(MAX_CONTAINERS_AUCTION_LOTS, MAX_CONTAINER_SIZE_AUCTION_LOTS));
            ZoneMap[ItemZoneType.ProbablyAuctionLots].PutContainer(0, MAX_CONTAINER_SIZE_AUCTION_LOTS);
        }

        public ItemInstance GetItem(ItemLocation loc)
        {
            if (loc.Equals(ItemLocation.InvalidLocation)) return null;
            return ZoneMap[loc.ZoneType].GetContainer(loc.Container).GetItem(loc.Slot);
        }

        public bool HasItem(ItemLocation loc)
        {
            if (!ZoneMap.ContainsKey(loc.ZoneType)) return false;
            if (ZoneMap[loc.ZoneType].GetContainer(loc.Container) == null) return false;
            if (ZoneMap[loc.ZoneType].GetContainer(loc.Container).GetItem(loc.Slot) == null) return false;
            return true;
        }

        public void PutItem(ItemLocation loc, ItemInstance item)
        {
            RemoveItem(item);
            item.Location = loc;

            switch (loc.ZoneType)
            {
                case ItemZoneType.BagSlot:
                    {
                        ZoneMap[ItemZoneType.EquippedBags].PutContainer(loc.Slot, item.BagSize);
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).PutItem(loc.Slot, item);
                        break;
                    }
                default:
                    {
                        ZoneMap[loc.ZoneType].GetContainer(loc.Container).PutItem(loc.Slot, item);
                        break;
                    }
            }

        }
        public void RemoveItem(ItemInstance item)
        {
            if (item.Location.Equals(ItemLocation.InvalidLocation)) return;            

            switch (item.Location.ZoneType)
            {
                case ItemZoneType.BagSlot:
                    {
                        ZoneMap[ItemZoneType.EquippedBags].RemoveContainer(item.Location.Slot);
                        ZoneMap[item.Location.ZoneType]?.GetContainer(item.Location.Container)?.RemoveItem(item.Location.Slot);
                        break;
                    }
                default:
                    {
                        ZoneMap[item.Location.ZoneType]?.GetContainer(item.Location.Container)?.RemoveItem(item.Location.Slot);
                        break;
                    }
            }

            item.Location = ItemLocation.InvalidLocation;
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
        /// Finds the next open slot in adventure bag, equipped bags, and premium bag, in that order.
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
            int nextContainerWithSpace = ZoneMap[itemZoneType].NextContainerWithSpace;
            if (nextContainerWithSpace != ItemZone.NO_CONTAINERS_WITH_SPACE)
            {
                int nextOpenSlot = ZoneMap[itemZoneType].GetContainer(nextContainerWithSpace).NextOpenSlot;
                if(nextOpenSlot != Container.NO_OPEN_SLOTS)
                {
                    ItemLocation itemLocation = new ItemLocation(itemZoneType, (byte)nextContainerWithSpace, (short) nextOpenSlot);
                    return itemLocation;
                }
            }
            return ItemLocation.InvalidLocation;
        }
        public ItemLocation[] NextOpenSlots(ItemZoneType itemZoneType, int amount)
        {
            if (amount > ZoneMap[itemZoneType].TotalFreeSpace) throw new ArgumentOutOfRangeException("Not enough open slots");
            return ZoneMap[itemZoneType].GetNextXFreeSpaces(itemZoneType, amount);
        }

        public int GetTotalFreeSpace(ItemZoneType itemZoneType)
        {
            return ZoneMap[itemZoneType].TotalFreeSpace;
        }

        public bool IsEmptyContainer(ItemZoneType itemZoneType, int container)
        {
            return ZoneMap[itemZoneType].GetContainer(container).Count == 0;
        }
    }
}
