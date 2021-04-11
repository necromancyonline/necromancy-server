using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    class ItemComparer : IComparer<ItemInstance>
    {
        public static readonly ItemComparer Instance = new ItemComparer();
        public int Compare(ItemInstance x, ItemInstance y)
        {
            //move empty to the back
            if (x != null && y is null) return -1;
            else if (x is null && y != null) return 1;
            else if (x is null && y is null) return 0;
            //put unidentified items last
            else if (!x.isIdentified && y.isIdentified) return -1;
            else if (x.isIdentified && !y.isIdentified) return 1;
            //then sort by type
            else if (x.type < y.type) return -1;
            else if (x.type > y.type) return 1;
            //then sort by base id, can't sort alphabetically as names are stored client side
            else if (x.baseId > y.baseId) return -1;
            else if (x.baseId < y.baseId) return 1;
            else return 0;
        }
    }
}

