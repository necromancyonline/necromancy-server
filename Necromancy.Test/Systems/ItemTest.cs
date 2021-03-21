
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
                    itemInstance.BaseID = baseId[i];
                    itemInstance.Location = locs[i];
                    itemInstance.Quantity = spawnParams[i].Quantity;
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
                const ulong instanceId = 756366;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                itemInstance.Quantity = quantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, quantity);

                Assert.Equal(MoveType.Place, moveResult.Type);
                Assert.Null(moveResult.OriginItem);
                Assert.Equal(instanceId, moveResult.DestItem.InstanceID);

                Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
                Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
                Assert.Equal(itemInstance.Location, toLoc);
                Assert.Equal(quantity, itemInstance.Quantity);
            }

            [Fact]
            public void TestItemMoveNoItemAtLocation()
            {
                const byte quantity = 2;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, quantity));

                Assert.Equal(ItemExceptionType.Generic, e.ExceptionType);
            }

            [Fact]
            public void TestItemMovePlaceInvalidQuantity()
            {
                const ulong instanceId = 756366;
                const byte startQuantity = 5;
                const byte moveQuantity = 10;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                itemInstance.Quantity = startQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, itemInstance);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, moveQuantity));

                Assert.Equal(ItemExceptionType.Amount, e.ExceptionType);
            }

            [Fact]
            public void TestItemMovePlaceQuantity()
            {
                const ulong instanceId = 756366;
                const int startQuantity = 10;
                const int moveQuantity = 6;
                ItemInstance itemOriginal = new ItemInstance(instanceId);
                itemOriginal.Quantity = startQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, itemOriginal);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, moveQuantity);

                Assert.Equal(MoveType.PlaceQuantity, moveResult.Type);
                Assert.Equal(instanceId, moveResult.OriginItem.InstanceID);

                Assert.Equal(fromLoc, itemOriginal.Location);
                Assert.Equal(startQuantity - moveQuantity, itemOriginal.Quantity);
                Assert.NotNull(_dummyCharacter.ItemManager.GetItem(fromLoc));
                Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);

                Assert.Equal(itemOriginal.BaseID, moveResult.DestItem.BaseID);
                Assert.Equal(toLoc, moveResult.DestItem.Location);
                Assert.Equal(moveQuantity, moveResult.DestItem.Quantity);
                Assert.NotNull(_dummyCharacter.ItemManager.GetItem(toLoc));
            }

            [Theory]
            [InlineData(1, 1)]
            [InlineData(2, 1)]
            [InlineData(100, 50)]
            [InlineData(50, 67)]
            public void TestItemMoveSwap(byte fromQuantity, byte toQuantity)
            {
                const ulong fromId = 234987915;
                const ulong toId = 33388888;

                ItemInstance fromItem = new ItemInstance(fromId);
                fromItem.Quantity = fromQuantity;
                fromItem.MaxStackSize = fromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(toId);
                toItem.Quantity = toQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, fromQuantity);

                Assert.Equal(MoveType.Swap, moveResult.Type);
                Assert.Equal(fromId, moveResult.DestItem.InstanceID);
                Assert.Equal(toId, moveResult.OriginItem.InstanceID);

                Assert.Equal(fromItem.Location, toLoc);
                Assert.Equal(fromId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
                Assert.Equal(fromQuantity, fromItem.Quantity);
                Assert.Equal(toItem.Location, fromLoc);
                Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);
                Assert.Equal(toQuantity, toItem.Quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantity()
            {
                const ulong toId = 234987915;
                const ulong fromId = 34151555;
                const int fromQuantity = 10;
                const int toQuantity = 30;
                const int moveQuantity = fromQuantity - 1;

                ItemInstance fromItem = new ItemInstance(fromId);
                fromItem.Quantity = fromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(toId);
                toItem.MaxStackSize = toQuantity + moveQuantity + 5;
                toItem.Quantity = toQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, moveQuantity);

                Assert.Equal(MoveType.AddQuantity, moveResult.Type);
                Assert.Equal(fromId, moveResult.OriginItem.InstanceID);
                Assert.Equal(toId, moveResult.DestItem.InstanceID);

                Assert.Equal(fromId, _dummyCharacter.ItemManager.GetItem(fromLoc).InstanceID);
                Assert.Equal(fromQuantity - moveQuantity, fromItem.Quantity);
                Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
                Assert.Equal(toQuantity + moveQuantity, toItem.Quantity);
            }

            [Fact]
            public void TestItemMoveAddQuantityWrongItem()
            {
                const ulong toId = 234987915;
                const int baseToId = 1234;
                const ulong fromId = 34151555;
                const int baseFromId = 5678;
                const int fromQuantity = 10;
                const int toQuantity = 30;
                const int moveQuantity = fromQuantity - 1;

                ItemInstance fromItem = new ItemInstance(fromId);
                fromItem.Quantity = fromQuantity;
                fromItem.BaseID = baseFromId;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(toId);
                toItem.MaxStackSize = toQuantity + moveQuantity + 5;
                toItem.Quantity = toQuantity;
                toItem.BaseID = baseToId;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(fromLoc, toLoc, moveQuantity));

                Assert.Equal(ItemExceptionType.BagLocation, e.ExceptionType);
            }

            [Fact]
            public void TestItemMovePlaceAllQuantity()
            {
                const ulong fromId = 34151555;
                const ulong toId = 234987915;
                const int fromQuantity = 10;
                const int toQuantity = 30;

                ItemInstance fromItem = new ItemInstance(fromId);
                fromItem.Quantity = fromQuantity;
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, fromItem);

                ItemInstance toItem = new ItemInstance(toId);
                toItem.MaxStackSize = toQuantity + fromQuantity + 5;
                toItem.Quantity = toQuantity;
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);
                _dummyCharacter.ItemManager.PutItem(toLoc, toItem);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, fromQuantity);

                Assert.Equal(MoveType.AllQuantity, moveResult.Type);
                Assert.Null(moveResult.OriginItem);
                Assert.Equal(toId, moveResult.DestItem.InstanceID);

                Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
                Assert.Equal(fromQuantity + toQuantity, toItem.Quantity);
                Assert.Equal(toId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
            }

            [Fact]
            public void TestItemMovePlaceInEquippedBag()
            {
                const ulong instanceId = 756366;
                const ulong bagId = 534577777;
                const byte bagSize = 10;
                const int quantity = 1;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                ItemLocation fromLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(fromLoc, itemInstance);

                ItemInstance equippedBag = new ItemInstance(bagId);
                equippedBag.BagSize = bagSize;
                ItemLocation bagLocation = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.ItemManager.PutItem(bagLocation, equippedBag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 1);

                MoveResult moveResult = _itemService.Move(fromLoc, toLoc, quantity);

                Assert.Equal(MoveType.Place, moveResult.Type);
                Assert.Equal(instanceId, moveResult.DestItem.InstanceID);

                Assert.Null(_dummyCharacter.ItemManager.GetItem(fromLoc));
                Assert.Equal(instanceId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
                Assert.Equal(itemInstance.Location, toLoc);
                Assert.Equal(quantity, itemInstance.Quantity);
                Assert.Equal(bagSize - 1, _dummyCharacter.ItemManager.GetTotalFreeSpace(ItemZoneType.EquippedBags));
            }

            [Fact]
            public void TestItemMoveEmptyBagOutOfSlot()
            {
                const ulong bagId = 534577777;
                const int quantity = 1;
                ItemInstance bag = new ItemInstance(bagId);
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.ItemManager.PutItem(bagLoc, bag);
                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                MoveResult moveResult = _itemService.Move(bagLoc, toLoc, quantity);

                Assert.Equal(ItemService.MoveType.Place, moveResult.Type);
                Assert.Equal(bagId, moveResult.DestItem.InstanceID);

                Assert.Null(_dummyCharacter.ItemManager.GetItem(bagLoc));
                Assert.Equal(bagId, _dummyCharacter.ItemManager.GetItem(toLoc).InstanceID);
                Assert.Equal(bag.Location, toLoc);
                Assert.Equal(quantity, bag.Quantity);
            }

            [Fact]
            public void TestItemMoveNonEmptyBagOutOfSlot()
            {
                const ulong bagId = 534577777;
                const ulong itemInBagId = 5117;
                const int quantity = 1;

                ItemInstance bag = new ItemInstance(bagId);
                bag.BagSize = 8;
                ItemLocation bagLoc = new ItemLocation(ItemZoneType.BagSlot, 0, 0);
                _dummyCharacter.ItemManager.PutItem(bagLoc, bag);

                ItemInstance itemInBag = new ItemInstance(itemInBagId);
                ItemLocation itemInBagLoc = new ItemLocation(ItemZoneType.EquippedBags, 0, 0);
                _dummyCharacter.ItemManager.PutItem(itemInBagLoc, itemInBag);

                ItemLocation toLoc = new ItemLocation(ItemZoneType.AdventureBag, 0, 1);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Move(bagLoc, toLoc, quantity));

                Assert.Equal(ItemExceptionType.BagLocation, e.ExceptionType);
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
                const ulong instanceId = 756366;
                const int quantity = 5;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                itemInstance.Quantity = quantity;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, quantity);

                Assert.Equal(0, result.Quantity);
                Assert.Equal(instanceId, result.InstanceID);
                Assert.Equal(ItemLocation.InvalidLocation, result.Location);
            }

            [Fact]
            public void TestItemRemoveSome()
            {
                const ulong instanceId = 756366;
                const int quantityToRemove = 5;
                const int quantityAvailable = quantityToRemove + 5;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                itemInstance.Quantity = quantityAvailable;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(loc, itemInstance);

                ItemInstance result = _itemService.Remove(loc, quantityToRemove);

                Assert.Equal(quantityAvailable - quantityToRemove, result.Quantity);
                Assert.Equal(instanceId, result.InstanceID);
                Assert.Equal(loc, result.Location);
            }

            [Fact]
            public void TestItemRemoveTooMany()
            {
                const ulong instanceId = 756366;
                const int quantityAvailable = 5;
                const int quantityToRemove = quantityAvailable + 5;
                ItemInstance itemInstance = new ItemInstance(instanceId);
                itemInstance.Quantity = quantityAvailable;
                ItemLocation loc = new ItemLocation(ItemZoneType.AdventureBag, 0, 0);
                _dummyCharacter.ItemManager.PutItem(loc, itemInstance);

                ItemException e = Assert.Throws<ItemException>(() => _itemService.Remove(loc, quantityToRemove));

                Assert.Equal(ItemExceptionType.Amount, e.ExceptionType);
            }
        }

    }
}
