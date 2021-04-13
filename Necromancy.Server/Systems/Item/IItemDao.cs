using System.Collections.Generic;

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
        ///     This selects only the items in the character's inventory: Adventure bag, Equipped bags, Royal bag, Bag Slots, and
        ///     Avatar inventory.
        /// </summary>
        /// <param name="ownerId">Owner of items.</param>
        /// <returns>A list of all the items in the character's inventory.</returns>
        public List<ItemInstance> SelectOwnedInventoryItems(int ownerId);

        /// <summary>
        ///     This selects only the items in the character's inventory that are lootable: Adventure bag, Equipped bags, and Royal
        ///     bag.
        /// </summary>
        /// <param name="ownerId">Owner of items.</param>
        /// <returns>A list of unidentified lootable items.</returns>
        public List<ItemInstance> SelectLootableInventoryItems(uint ownerId);

        //Auction Methods
        public List<ItemInstance> SelectAuctions(uint ownerSoulId);
        public void UpdateAuctionExhibit(ItemInstance itemInstance);
        public void UpdateAuctionCancelExhibit(ulong instanceId);
        public List<ItemInstance> SelectBids(int characterId);
        public List<ItemInstance> SelectLots(int characterId);
        public ulong SelectBuyoutPrice(ulong instanceId);
        public void InsertAuctionBid(ulong instanceId, int bidderSoulId, ulong bid);
        public void UpdateAuctionWinner(ulong instanceId, int winnerSoulId);
        public int SelectAuctionWinnerSoulId(ulong instanceId);
    }
}
