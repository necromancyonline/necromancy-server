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
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(DropTables));

        private readonly NecServer _server;
        private List<DropTable> dropTables;

        public DropTables(NecServer server)
        {
            _server = server;
            dropTables = new List<DropTable>();
            DropTable beetle = new DropTable(40101);
            DropTableItem item1 = new DropTableItem();
            item1.ItemId = 1;
            item1.Rarity = 1;
            item1.MinItems = 1;
            item1.Maxitems = 5;
            DropTableItem item2 = new DropTableItem();
            item2.ItemId = 50100101;
            item2.Rarity = 2;
            item2.MinItems = 1;
            item2.Maxitems = 3;
            DropTableItem item3 = new DropTableItem();
            item3.ItemId = 10200101;
            item3.Rarity = 3;
            item3.MinItems = 1;
            item3.Maxitems = 1;

            beetle.AddItem(item1);
            beetle.AddItem(item2);
            beetle.AddItem(item3);
            dropTables.Add(beetle);
        }

        public DropItem GetLoot(int monsterId)
        {
            monsterId = 40101; //   All monsters are beetles for now!!
            int roll = LootRoll();
            DropTable monsterDrop = dropTables.Find(x => x.MonsterId == monsterId);
            DropItem dropItem = null;

            if (monsterDrop != null)
            {
                List<DropTableItem> ItemDrop = monsterDrop.FindAll(roll);
                if (ItemDrop.Count == 1)
                {
                    Logger.Debug($"ItemId [ItemDrop ItemId {ItemDrop[0].ItemId}]");
                    if (!_server.SettingRepository.ItemInfo.TryGetValue(ItemDrop[0].ItemId, out ItemInfoSetting itemSetting))
                    {
                        Logger.Error($"Could not retrieve ItemSettings for ItemId [{ItemDrop[0].ItemId}]");
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
                if (!_server.SettingRepository.ItemInfo.TryGetValue(50100301, out ItemInfoSetting itemSetting))
                {
                    Logger.Error($"Could not retrieve ItemSettings for default Item Camp");
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
            {
                return 1;
            }
            else if (lootRoll < 911)
            {
                return 2;
            }
            else if (lootRoll < 971)
            {
                return 3;
            }
            else if (lootRoll < 999)
            {
                return 4;
            }

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
        public int MonsterId { get; }
        public List<DropTableItem> DropTableItems { get; }

        public DropTable(int monsterId)
        {
            MonsterId = monsterId;
            DropTableItems = new List<DropTableItem>();
        }

        public void AddItem(DropTableItem item)
        {
            DropTableItems.Add(item);
        }

        public List<DropTableItem> FindAll(int rarity)
        {
            return DropTableItems.FindAll(x => x.Rarity == rarity);
        }
    }

    public class DropTableItem
    {
        public int Rarity;
        public int ItemId;
        public int MinItems;
        public int Maxitems;
    }

    public class DropItem
    {
        public ItemBase Item;
        public int NumItems;

        public DropItem(int numItems, ItemBase item)
        {
            Item = item;
            NumItems = numItems;
        }
    }
}
