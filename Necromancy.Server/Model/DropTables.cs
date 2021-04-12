using System.Collections.Generic;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Data.Setting;
using Necromancy.Server.Logging;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Model
{
    public class DropTables
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(DropTables));

        private readonly NecServer _server;
        private readonly List<DropTable> _dropTables;

        public DropTables(NecServer server)
        {
            _server = server;
            _dropTables = new List<DropTable>();
            DropTable beetle = new DropTable(40101);
            DropTableItem item1 = new DropTableItem();
            item1.itemId = 1;
            item1.rarity = 1;
            item1.minItems = 1;
            item1.maxitems = 5;
            DropTableItem item2 = new DropTableItem();
            item2.itemId = 50100101;
            item2.rarity = 2;
            item2.minItems = 1;
            item2.maxitems = 3;
            DropTableItem item3 = new DropTableItem();
            item3.itemId = 10200101;
            item3.rarity = 3;
            item3.minItems = 1;
            item3.maxitems = 1;

            beetle.AddItem(item1);
            beetle.AddItem(item2);
            beetle.AddItem(item3);
            _dropTables.Add(beetle);
        }

        public DropItem GetLoot(int monsterId)
        {
            monsterId = 40101; //   All monsters are beetles for now!!
            int roll = LootRoll();
            DropTable monsterDrop = _dropTables.Find(x => x.monsterId == monsterId);
            DropItem dropItem = null;

            if (monsterDrop != null)
            {
                List<DropTableItem> itemDrop = monsterDrop.FindAll(roll);
                if (itemDrop.Count == 1)
                {
                    _Logger.Debug($"ItemId [ItemDrop ItemId {itemDrop[0].itemId}]");
                    if (!_server.settingRepository.itemInfo.TryGetValue(itemDrop[0].itemId, out ItemInfoSetting itemSetting))
                    {
                        _Logger.Error($"Could not retrieve ItemSettings for ItemId [{itemDrop[0].itemId}]");
                        return null;
                    }

                    //         Logger.Debug($"ItemId [ItemDrop ItemId {ItemDrop[0].ItemId}]");
                    //         if (itemSetting.Id == 10200101)
                    //         {
                    //             itemSetting.IconType = 2;
                    //         }
                    //         else if (itemSetting.Id == 80000101)
                    //         {
                    //             itemSetting.IconType = 55;
                    //         }

                    //     Item item = _server.Instances
                    //         .CreateInstance<Item>(); //  Need to get fully populated Item repository
                    //     item.AddItemSetting(itemSetting);
                    //     int numItems = GetNumberItems(ItemDrop[0].MinItems, ItemDrop[0].Maxitems + 1);
                    //     dropItem = new DropItem(numItems, item);
                }
            }
            else
            {
                if (!_server.settingRepository.itemInfo.TryGetValue(50100301, out ItemInfoSetting itemSetting))
                {
                    _Logger.Error("Could not retrieve ItemSettings for default Item Camp");
                    return null;
                }

                ItemBase item = new ItemBase(); //  Need to get fully populated Item repository
                //     item.IconType = 45;
                //     item.ItemType = 1;
                dropItem = new DropItem(1, item);
            }

            return dropItem;
        }

        private int LootRoll()
        {
            int lootRoll = Util.GetRandomNumber(1, 1001);

            if (lootRoll < 631)
                return 1;
            if (lootRoll < 911)
                return 2;
            if (lootRoll < 971)
                return 3;
            if (lootRoll < 999) return 4;

            return 5;
        }

        public int GetNumberItems(int min, int max)
        {
            return Util.GetRandomNumber(min, max);
            ;
        }
    }

    public class DropTable
    {
        public DropTable(int monsterId)
        {
            this.monsterId = monsterId;
            dropTableItems = new List<DropTableItem>();
        }

        public int monsterId { get; }
        public List<DropTableItem> dropTableItems { get; }

        public void AddItem(DropTableItem item)
        {
            dropTableItems.Add(item);
        }

        public List<DropTableItem> FindAll(int rarity)
        {
            return dropTableItems.FindAll(x => x.rarity == rarity);
        }
    }

    public class DropTableItem
    {
        public int itemId;
        public int maxitems;
        public int minItems;
        public int rarity;
    }

    public class DropItem
    {
        public ItemBase item;
        public int numItems;

        public DropItem(int numItems, ItemBase item)
        {
            this.item = item;
            this.numItems = numItems;
        }
    }
}
