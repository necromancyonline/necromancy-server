using System.Collections.Generic;

namespace Necromancy.Server.Systems.Item
{
    internal class ItemComparer : IComparer<ItemInstance>
    {
        public static readonly ItemComparer Instance = new ItemComparer();

        public int Compare(ItemInstance x, ItemInstance y)
        {
            //move empty to the back
            if (x != null && y is null) return -1;
            if (x is null && y != null) return 1;
            if (x is null && y is null) return 0;
            //put unidentified items last
            if (!x.isIdentified && y.isIdentified) return -1;
            if (x.isIdentified && !y.isIdentified) return 1;
            //then sort by type
            if (x.type < y.type) return -1;
            if (x.type > y.type) return 1;
            //then sort by base id, can't sort alphabetically as names are stored client side
            if (x.baseId > y.baseId) return -1;
            if (x.baseId < y.baseId) return 1;
            return 0;
        }
    }
}
