using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public interface IItemDao
    {
        public ItemInstance InsertItemInstance(int baseId);
        public List<ItemInstance> InsertItemInstances(int ownerId, ItemLocation[] locs, int[] baseId, ItemSpawnParams[] spawnParams);
        public ItemInstance SelectItemInstance(long instanceId);
        public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation);
        public void DeleteItemInstance(ulong instanceIds);
        public void UpdateItemLocation(ulong instanceId, ItemLocation loc);
        public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs);
        public void UpdateItemQuantities(ulong[] instanceIds, byte[] quantities);
        public void UpdateItemEquipMask(ulong instanceId, ItemEquipSlots equipSlots);
        public void UpdateItemEnhancementLevel(ulong instanceId, int level);
        public void UpdateItemCurrentDurability(ulong instanceId, int currentDurability);
        public void UpdateItemOwnerAndStatus(ulong instanceId, int ownerId, int statuses);

        /// <summary>
        /// This selects only the items in the player's inventory: Adventure bag, Equipped bags, Royal bag, Bag Slots, and Avatar inventory.
        /// </summary>
        /// <param name="ownerId">Owner of items.</param>
        /// <returns></returns>
        public List<ItemInstance> SelectOwnedInventoryItems(int ownerId);

    }
}
