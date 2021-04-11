
namespace Necromancy.Server.Systems.Item
{
    class Container
    {
        public const int NoOpenSlots = -1;
        public readonly ItemInstance[] slots;
        public int size { get; }
        public int count { get; private set; }
        public bool isSorted { get; private set; }

        public bool isFull
        {
            get
            {
                return (size - count) <= 0;
            }
        }

        public int totalFreeSlots
        {
            get
            {
                return size - count;
            }
        }
        public Container(int size)
        {
            slots = new ItemInstance[size];
            this.size = size;
        }

        public int nextOpenSlot
        {
            get
            {
                for (int i = 0; i < size; i++)
                {
                    if (slots[i] is null) return i;
                }
                return NoOpenSlots;
            }
        }
        public void PutItem(int slot, ItemInstance item)
        {
            if (slots[slot] != null)
                slots[slot].location = ItemLocation.InvalidLocation;
            slots[slot] = item;
            count++;
            isSorted = false;
        }
        public ItemInstance GetItem(int slot)
        {
            return slots[slot];
        }
        public void RemoveItem(int slot)
        {
            if (slots[slot] != null)
                slots[slot].location = ItemLocation.InvalidLocation;
            slots[slot] = null;
            count--;
            isSorted = false;
        }
        public bool HasItem(int slot)
        {
            return slots[slot] != null;
        }

        public int GetNextOpenSlot(int startSlot)
        {
            for (int i = startSlot + 1; i < size; i++)
            {
                if (slots[i] is null) return i;
            }
            return NoOpenSlots;
        }
    }
}
