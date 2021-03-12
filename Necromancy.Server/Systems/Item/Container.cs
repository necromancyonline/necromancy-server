using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    class Container
    {
        public const int NO_OPEN_SLOTS = -1;
        public readonly ItemInstance[] _slots;
        public int Size { get; }
        public int Count { get; private set; }
        public bool IsSorted { get; private set; }

        public bool IsFull
        {
            get
            {
                return (Size - Count) <= 0;
            }
        }

        public int TotalFreeSlots
        {
            get
            {
                return Size - Count;
            }
        }
        public Container(int size)
        {
            _slots = new ItemInstance[size];
            Size = size;
        }

        public int NextOpenSlot
        {
            get
            {
                for (int i = 0; i < Size; i++)
                {
                    if (_slots[i] is null) return i;
                }
                return NO_OPEN_SLOTS;
            }
        }
        public void PutItem(int slot, ItemInstance item)
        {
            if (_slots[slot] != null)
                _slots[slot].Location = ItemLocation.InvalidLocation;
            _slots[slot] = item;
            Count++;
            IsSorted = false;
        }
        public ItemInstance GetItem(int slot)
        {
            return _slots[slot];
        }
        public void RemoveItem(int slot)
        {
            if (_slots[slot] != null)
                _slots[slot].Location = ItemLocation.InvalidLocation;
            _slots[slot] = null;
            Count--;
            IsSorted = false;
        }
        public bool HasItem(int slot)
        {
            return _slots[slot] != null;
        }

        public int GetNextOpenSlot(int startSlot)
        {
            for (int i = startSlot + 1; i < Size; i++)
            {
                if (_slots[i] is null) return i;
            }
            return NO_OPEN_SLOTS;
        }
    }
}
