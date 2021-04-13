using System;
using System.Collections.Generic;
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using Xunit;
using static Necromancy.Server.Systems.Item.ItemService;

namespace Necromancy.Test.Systems
{
    public class ItemTest
    {
        private class DummyItemDao : IItemDao
        {
            public void DeleteItemInstance(ulong instanceIds)
            {
                //ignore
            }

            public ItemInstance InsertItemInstance(int baseId)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> InsertItemInstances(int ownerId, ItemLocation[] locs, int[] baseId, ItemSpawnParams[] spawnParams)
            {
                List<ItemInstance> dummyItems = new List<ItemInstance>(2);
                for (int i = 0; i < locs.Length; i++)
                {
                    ItemInstance itemInstance = new ItemInstance((ulong)i);
                    itemInstance.baseId = baseId[i];
                    itemInstance.location = locs[i];
                    itemInstance.quantity = spawnParams[i].quantity;
                    //itemInstance.PlusDurability = spawnParams[i].plus_maximum_durability;
                    //itemInstance.PlusPhysical = spawnParams[i].plus_physical;
                    //itemInstance.PlusMagical  = spawnParams[i].plus_magical;
                    //itemInstance.PlusGP  = spawnParams[i].plus_gp;
                    //itemInstance.PlusWeight  = spawnParams[i].plus_weight;
                    //itemInstance.PlusRangedEff  = spawnParams[i].plus_ranged_eff;
                    //itemInstance.PlusReservoirEff  = spawnParams[i].plus_reservoir_eff;
                    dummyItems.Add(itemInstance);
                }

                return dummyItems;
            }

            public ItemInstance SelectItemInstance(long instanceId)
            {
                throw new NotImplementedException();
            }

            public ItemInstance SelectItemInstance(int characterId, ItemLocation itemLocation)
            {
                throw new NotImplementedException();
            }

            public void UpdateItemEquipMask(ulong instanceId, ItemEquipSlots equipSlots)
            {
                throw new NotImplementedException();
            }

            public void UpdateItemLocations(ulong[] instanceIds, ItemLocation[] locs)
            {
                //ignore
            }

            public void UpdateItemQuantities(ulong[] instanceIds, byte[] quantities)
            {
                //ignore
            }

            public void UpdateItemEnhancementLevel(ulong instanceId, int level)
            {
                //ignore
            }

            public void UpdateItemCurrentDurability(ulong instanceId, int currentDurability)
            {
                //ignore
            }

            public void UpdateItemLocation(ulong instanceId, ItemLocation itemLocation)
            {
                //ignore
            }

            public void UpdateItemOwnerAndStatus(ulong instanceId, int ownerId, int statuses)
            {
                //curse you DummyItemDAO!!! muahaha
            }

            public List<ItemInstance> SelectOwnedInventoryItems(int ownerId)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> SelectLootableInventoryItems(uint ownerId)
            {
                throw new NotImplementedException();
            }

            public void UpdateAuctionExhibit(ItemInstance itemInstance)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> SelectBids(int characterId)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> SelectLots(int characterId)
            {
                throw new NotImplementedException();
            }

            public List<ItemInstance> SelectAuctions(uint ownerSoulId)
            {
                throw new NotImplementedException();
            }

            public void UpdateAuctionCancelExhibit(ulong instanceId)
            {
                throw new NotImplementedException();
            }

            public ulong SelectBuyoutPrice(ulong instanceId)
            {
                throw new NotImplementedException();
            }

            public void InsertAuctionBid(ulong instanceId, int bidderSoulId, ulong bid)
            {
                throw new NotImplementedException();
            }

            public void UpdateAuctionWinner(ulong instanceId, int winnerSoulId)
            {
                throw new NotImplementedException();
            }

            public int SelectAuctionWinnerSoulId(ulong instanceId)
            {
                throw new NotImplementedException();
            }
        }

        public class TestMove
        {
            private readonly Character _dummyCharacter;
            private readonly ItemService _itemService;

            public TestMove()
            {
                _dummyCharacter = new Character();
                _itemService = new ItemService(_dummyCharacter, new DummyItemDao());
            }

            [Theory]
            [InlineData(1)]
            [InlineData(2)]
            [InlineData(100)]
            public void TestItemMovePlace(byte quantity)
            {
                const ulong INSTANCE_ID = 756366;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                itemInstance.quantity = quantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, quantity);

                Assert.Equal(MoveType.Place, moveResult.type);
                Assert.Null(moveResult.originItem);
                Assert.Equal(INSTANCE_ID, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(INSTANCE_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(itemInstance.location, toLoc);
                Assert.Equal(quantity, itemInstance.quantity);
            }

            [Fact]
            public void TestItemMoveNoItemAtLocation()
            {
                const byte QUANTITY = 2;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, QUANTITY));

                Assert.Equal(ItemExceptionType.Generic, e.type);
            }

            [Fact]
            public void TestItemMovePlaceInvalidQuantity()
            {
                const ulong INSTANCE_ID = 756366;
                const byte START_QUANTITY = 5;
                const byte MOVE_QUANTITY = 10;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                itemInstance.quantity = START_QUANTITY;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, MOVE_QUANTITY));

                Assert.Equal(ItemExceptionType.Amount, e.type);
            }

            [Fact]
            public void TestItemMovePlaceQuantity()
            {
                const ulong INSTANCE_ID = 756366;
                const int START_QUANTITY = 10;
                const int MOVE_QUANTITY = 6;
                ItemInstance itemOriginal = new ItemInstance(INSTANCE_ID);
                itemOriginal.quantity = START_QUANTITY;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemOriginal);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, MOVE_QUANTITY);

                Assert.Equal(MoveType.PlaceQuantity, moveResult.type);
                Assert.Equal(INSTANCE_ID, moveResult.originItem.instanceId);

                Assert.Equal(fromLoc, itemOriginal.location);
                Assert.Equal(START_QUANTITY - MOVE_QUANTITY, itemOriginal.quantity);
                Assert.NotNull(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(INSTANCE_ID, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);

                Assert.Equal(itemOriginal.baseId, moveResult.destItem.baseId);
                Assert.Equal(toLoc, moveResult.destItem.location);
                Assert.Equal(MOVE_QUANTITY, moveResult.destItem.quantity);
                Assert.NotNull(_dummyCharacter.itemLocationVerifier.GetItem(toLoc));
            }

            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 1)]
            [InlineData(100, 50)]
            [InlineData(50, 67)]
            public void TestItemMoveSwap(byte fromQuantity, byte toQuantity)
            {
                const ulong FROM_ID = 234987915;
                const ulong TO_ID = 33388888;

                ItemInstance fromItem = new ItemInstance(FROM_ID);
                fromItem.quantity = fromQuantity;
                fromItem.maxStackSize = fromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(TO_ID);
                toItem.quantity = toQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, fromQuantity);

                Assert.Equal(MoveType.Swap, moveResult.type);
                Assert.Equal(FROM_ID, moveResult.destItem.instanceId);
                Assert.Equal(TO_ID, moveResult.originItem.instanceId);

                Assert.Equal(fromItem.location, toLoc);
                Assert.Equal(FROM_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(fromQuantity, fromItem.quantity);
                Assert.Equal(toItem.location, fromLoc);
                Assert.Equal(TO_ID, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);
                Assert.Equal(toQuantity, toItem.quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantity()
            {
                const ulong TO_ID = 234987915;
                const ulong FROM_ID = 34151555;
                const int FROM_QUANTITY = 10;
                const int TO_QUANTITY = 30;
                const int MOVE_QUANTITY = FROM_QUANTITY - 1;

                ItemInstance fromItem = new ItemInstance(FROM_ID);
                fromItem.quantity = FROM_QUANTITY;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(TO_ID);
                toItem.maxStackSize = TO_QUANTITY + MOVE_QUANTITY + 5;
                toItem.quantity = TO_QUANTITY;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, MOVE_QUANTITY);

                Assert.Equal(MoveType.AddQuantity, moveResult.type);
                Assert.Equal(FROM_ID, moveResult.originItem.instanceId);
                Assert.Equal(TO_ID, moveResult.destItem.instanceId);

                Assert.Equal(FROM_ID, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);
                Assert.Equal(FROM_QUANTITY - MOVE_QUANTITY, fromItem.quantity);
                Assert.Equal(TO_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(TO_QUANTITY + MOVE_QUANTITY, toItem.quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantityWrongItem()
            {
                const ulong TO_ID = 234987915;
                const int BASE_TO_ID = 1234;
                const ulong FROM_ID = 34151555;
                const int BASE_FROM_ID = 5678;
                const int FROM_QUANTITY = 10;
                const int TO_QUANTITY = 30;
                const int MOVE_QUANTITY = FROM_QUANTITY - 1;

                ItemInstance fromItem = new ItemInstance(FROM_ID);
                fromItem.quantity = FROM_QUANTITY;
                fromItem.baseId = BASE_FROM_ID;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(TO_ID);
                toItem.maxStackSize = TO_QUANTITY + MOVE_QUANTITY + 5;
                toItem.quantity = TO_QUANTITY;
                toItem.baseId = BASE_TO_ID;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, MOVE_QUANTITY));

                Assert.Equal(ItemExceptionType.BagLocation, e.type);
            }

            [Fact]
            public void TestItemMovePlaceAllQuantity()
            {
                const ulong FROM_ID = 34151555;
                const ulong TO_ID = 234987915;
                const int FROM_QUANTITY = 10;
                const int TO_QUANTITY = 30;

                ItemInstance fromItem = new ItemInstance(FROM_ID);
                fromItem.quantity = FROM_QUANTITY;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(TO_ID);
                toItem.maxStackSize = TO_QUANTITY + FROM_QUANTITY + 5;
                toItem.quantity = TO_QUANTITY;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, FROM_QUANTITY);

                Assert.Equal(MoveType.AllQuantity, moveResult.type);
                Assert.Null(moveResult.originItem);
                Assert.Equal(TO_ID, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(FROM_QUANTITY + TO_QUANTITY, toItem.quantity);
                Assert.Equal(TO_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
            }

            [Fact]
            public void TestItemMovePlaceInEquippedBag()
            {
                const ulong INSTANCE_ID = 756366;
                const ulong BAG_ID = 534577777;
                const byte BAG_SIZE = 10;
                const int QUANTITY = 1;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);

                ItemInstance equippedBag = new ItemInstance(BAG_ID);
                equippedBag.bagSize = BAG_SIZE;
                ItemLocation bagLocation = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLocation, equippedBag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, QUANTITY);

                Assert.Equal(MoveType.Place, moveResult.type);
                Assert.Equal(INSTANCE_ID, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(INSTANCE_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(itemInstance.location, toLoc);
                Assert.Equal(QUANTITY, itemInstance.quantity);
                Assert.Equal(BAG_SIZE - 1, _dummyCharacter.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.EquippedBags));
            }

            [Fact]
            public void TestItemMoveEmptyBagOutOfSlot()
            {
                const ulong BAG_ID = 534577777;
                const int QUANTITY = 1;
                ItemInstance bag = new ItemInstance(BAG_ID);
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLoc, bag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(bagLoc, toLoc, QUANTITY);

                Assert.Equal(MoveType.Place, moveResult.type);
                Assert.Equal(BAG_ID, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(bagLoc));
                Assert.Equal(BAG_ID, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(bag.location, toLoc);
                Assert.Equal(QUANTITY, bag.quantity);
            }

            [Fact]
            public void TestItemMoveNonEmptyBagOutOfSlot()
            {
                const ulong BAG_ID = 534577777;
                const ulong ITEM_IN_BAG_ID = 5117;
                const int QUANTITY = 1;

                ItemInstance bag = new ItemInstance(BAG_ID);
                bag.bagSize = 8;
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLoc, bag);

                ItemInstance itemInBag = new ItemInstance(ITEM_IN_BAG_ID);
                ItemLocation itemInBagLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(itemInBagLoc, itemInBag);

                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(bagLoc, toLoc, QUANTITY));

                Assert.Equal(ItemExceptionType.BagLocation, e.type);
            }
        }

        public class TestRemove
        {
            private readonly Character _dummyCharacter;
            private readonly ItemService _itemService;

            public TestRemove()
            {
                _dummyCharacter = new Character();
                _itemService = new ItemService(_dummyCharacter, new DummyItemDao());
            }

            [Fact]
            public void TestItemRemoveAll()
            {
                const ulong INSTANCE_ID = 756366;
                const int QUANTITY = 5;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                itemInstance.quantity = QUANTITY;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, QUANTITY);

                Assert.Equal(0, result.quantity);
                Assert.Equal(INSTANCE_ID, result.instanceId);
                Assert.Equal(ItemLocation.InvalidLocation, result.location);
            }

            [Fact]
            public void TestItemRemoveSome()
            {
                const ulong INSTANCE_ID = 756366;
                const int QUANTITY_TO_REMOVE = 5;
                const int QUANTITY_AVAILABLE = QUANTITY_TO_REMOVE + 5;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                itemInstance.quantity = QUANTITY_AVAILABLE;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, QUANTITY_TO_REMOVE);

                Assert.Equal(QUANTITY_AVAILABLE - QUANTITY_TO_REMOVE, result.quantity);
                Assert.Equal(INSTANCE_ID, result.instanceId);
                Assert.Equal(loc, result.location);
            }

            [Fact]
            public void TestItemRemoveTooMany()
            {
                const ulong INSTANCE_ID = 756366;
                const int QUANTITY_AVAILABLE = 5;
                const int QUANTITY_TO_REMOVE = QUANTITY_AVAILABLE + 5;
                ItemInstance itemInstance = new ItemInstance(INSTANCE_ID);
                itemInstance.quantity = QUANTITY_AVAILABLE;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Remove(loc, QUANTITY_TO_REMOVE));

                Assert.Equal(ItemExceptionType.Amount, e.type);
            }
        }
    }
}
