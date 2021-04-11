
using Necromancy.Server.Model;
using Necromancy.Server.Systems.Item;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
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
                const ulong InstanceId = 756366;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                itemInstance.quantity = quantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, quantity);

                Assert.Equal(MoveType.Place, moveResult.type);
                Assert.Null(moveResult.originItem);
                Assert.Equal(InstanceId, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(InstanceId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(itemInstance.location, toLoc);
                Assert.Equal(quantity, itemInstance.quantity);
            }

            [Fact]
            public void TestItemMoveNoItemAtLocation()
            {
                const byte Quantity = 2;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, Quantity));

                Assert.Equal(ItemExceptionType.Generic, e.type);
            }

            [Fact]
            public void TestItemMovePlaceInvalidQuantity()
            {
                const ulong InstanceId = 756366;
                const byte StartQuantity = 5;
                const byte MoveQuantity = 10;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                itemInstance.quantity = StartQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, MoveQuantity));

                Assert.Equal(ItemExceptionType.Amount, e.type);
            }

            [Fact]
            public void TestItemMovePlaceQuantity()
            {
                const ulong InstanceId = 756366;
                const int StartQuantity = 10;
                const int MoveQuantity = 6;
                ItemInstance itemOriginal = new ItemInstance(InstanceId);
                itemOriginal.quantity = StartQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemOriginal);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, MoveQuantity);

                Assert.Equal(MoveType.PlaceQuantity, moveResult.type);
                Assert.Equal(InstanceId, moveResult.originItem.instanceId);

                Assert.Equal(fromLoc, itemOriginal.location);
                Assert.Equal(StartQuantity - MoveQuantity, itemOriginal.quantity);
                Assert.NotNull(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(InstanceId, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);

                Assert.Equal(itemOriginal.baseId, moveResult.destItem.baseId);
                Assert.Equal(toLoc, moveResult.destItem.location);
                Assert.Equal(MoveQuantity, moveResult.destItem.quantity);
                Assert.NotNull(_dummyCharacter.itemLocationVerifier.GetItem(toLoc));
            }

            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 1)]
            [InlineData(100, 50)]
            [InlineData(50, 67)]
            public void TestItemMoveSwap(byte fromQuantity, byte toQuantity)
            {
                const ulong FromId = 234987915;
                const ulong ToId = 33388888;

                ItemInstance fromItem = new ItemInstance(FromId);
                fromItem.quantity = fromQuantity;
                fromItem.maxStackSize = fromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(ToId);
                toItem.quantity = toQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, fromQuantity);

                Assert.Equal(MoveType.Swap, moveResult.type);
                Assert.Equal(FromId, moveResult.destItem.instanceId);
                Assert.Equal(ToId, moveResult.originItem.instanceId);

                Assert.Equal(fromItem.location, toLoc);
                Assert.Equal(FromId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(fromQuantity, fromItem.quantity);
                Assert.Equal(toItem.location, fromLoc);
                Assert.Equal(ToId, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);
                Assert.Equal(toQuantity, toItem.quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantity()
            {
                const ulong ToId = 234987915;
                const ulong FromId = 34151555;
                const int FromQuantity = 10;
                const int ToQuantity = 30;
                const int MoveQuantity = FromQuantity - 1;

                ItemInstance fromItem = new ItemInstance(FromId);
                fromItem.quantity = FromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(ToId);
                toItem.maxStackSize = ToQuantity + MoveQuantity + 5;
                toItem.quantity = ToQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, MoveQuantity);

                Assert.Equal(MoveType.AddQuantity, moveResult.type);
                Assert.Equal(FromId, moveResult.originItem.instanceId);
                Assert.Equal(ToId, moveResult.destItem.instanceId);

                Assert.Equal(FromId, _dummyCharacter.itemLocationVerifier.GetItem(fromLoc).instanceId);
                Assert.Equal(FromQuantity - MoveQuantity, fromItem.quantity);
                Assert.Equal(ToId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(ToQuantity + MoveQuantity, toItem.quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantityWrongItem()
            {
                const ulong ToId = 234987915;
                const int BaseToId = 1234;
                const ulong FromId = 34151555;
                const int BaseFromId = 5678;
                const int FromQuantity = 10;
                const int ToQuantity = 30;
                const int MoveQuantity = FromQuantity - 1;

                ItemInstance fromItem = new ItemInstance(FromId);
                fromItem.quantity = FromQuantity;
                fromItem.baseId = BaseFromId;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(ToId);
                toItem.maxStackSize = ToQuantity + MoveQuantity + 5;
                toItem.quantity = ToQuantity;
                toItem.baseId = BaseToId;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, MoveQuantity));

                Assert.Equal(ItemExceptionType.BagLocation, e.type);
            }

            [Fact]
            public void TestItemMovePlaceAllQuantity()
            {
                const ulong FromId = 34151555;
                const ulong ToId = 234987915;
                const int FromQuantity = 10;
                const int ToQuantity = 30;

                ItemInstance fromItem = new ItemInstance(FromId);
                fromItem.quantity = FromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(ToId);
                toItem.maxStackSize = ToQuantity + FromQuantity + 5;
                toItem.quantity = ToQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.itemLocationVerifier.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, FromQuantity);

                Assert.Equal(MoveType.AllQuantity, moveResult.type);
                Assert.Null(moveResult.originItem);
                Assert.Equal(ToId, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(FromQuantity + ToQuantity, toItem.quantity);
                Assert.Equal(ToId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
            }

            [Fact]
            public void TestItemMovePlaceInEquippedBag()
            {
                const ulong InstanceId = 756366;
                const ulong BagId = 534577777;
                const byte BagSize = 10;
                const int Quantity = 1;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(fromLoc, itemInstance);

                ItemInstance equippedBag = new ItemInstance(BagId);
                equippedBag.bagSize = BagSize;
                ItemLocation bagLocation = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLocation, equippedBag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, Quantity);

                Assert.Equal(MoveType.Place, moveResult.type);
                Assert.Equal(InstanceId, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(fromLoc));
                Assert.Equal(InstanceId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(itemInstance.location, toLoc);
                Assert.Equal(Quantity, itemInstance.quantity);
                Assert.Equal(BagSize - 1, _dummyCharacter.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.EquippedBags));
            }

            [Fact]
            public void TestItemMoveEmptyBagOutOfSlot()
            {
                const ulong BagId = 534577777;
                const int Quantity = 1;
                ItemInstance bag = new ItemInstance(BagId);
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLoc, bag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(bagLoc, toLoc, Quantity);

                Assert.Equal(ItemService.MoveType.Place, moveResult.type);
                Assert.Equal(BagId, moveResult.destItem.instanceId);

                Assert.Null(_dummyCharacter.itemLocationVerifier.GetItem(bagLoc));
                Assert.Equal(BagId, _dummyCharacter.itemLocationVerifier.GetItem(toLoc).instanceId);
                Assert.Equal(bag.location, toLoc);
                Assert.Equal(Quantity, bag.quantity);
            }

            [Fact]
            public void TestItemMoveNonEmptyBagOutOfSlot()
            {
                const ulong BagId = 534577777;
                const ulong ItemInBagId = 5117;
                const int Quantity = 1;

                ItemInstance bag = new ItemInstance(BagId);
                bag.bagSize = 8;
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(bagLoc, bag);

                ItemInstance itemInBag = new ItemInstance(ItemInBagId);
                ItemLocation itemInBagLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(itemInBagLoc, itemInBag);

                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(bagLoc, toLoc, Quantity));

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
                const ulong InstanceId = 756366;
                const int Quantity = 5;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                itemInstance.quantity = Quantity;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, Quantity);

                Assert.Equal(0, result.quantity);
                Assert.Equal(InstanceId, result.instanceId);
                Assert.Equal(ItemLocation.InvalidLocation, result.location);
            }

            [Fact]
            public void TestItemRemoveSome()
            {
                const ulong InstanceId = 756366;
                const int QuantityToRemove = 5;
                const int QuantityAvailable = QuantityToRemove + 5;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                itemInstance.quantity = QuantityAvailable;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, QuantityToRemove);

                Assert.Equal(QuantityAvailable - QuantityToRemove, result.quantity);
                Assert.Equal(InstanceId, result.instanceId);
                Assert.Equal(loc, result.location);
            }

            [Fact]
            public void TestItemRemoveTooMany()
            {
                const ulong InstanceId = 756366;
                const int QuantityAvailable = 5;
                const int QuantityToRemove = QuantityAvailable + 5;
                ItemInstance itemInstance = new ItemInstance(InstanceId);
                itemInstance.quantity = QuantityAvailable;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.itemLocationVerifier.PutItem(loc, itemInstance);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Remove(loc, QuantityToRemove));

                Assert.Equal(ItemExceptionType.Amount, e.type);
            }
        }

    }
}
