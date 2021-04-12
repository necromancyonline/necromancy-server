namespace Necromancy.Server.Systems.Item
{
    internal class Container
    {
        public const int NO_OPEN_SLOTS = -1;
        public readonly ItemInstance[] Slots;

        public Container(int size)
        {
            Slots = new ItemInstance[size];
            this.size = size;
        }

        public int size { get; }
        public int count { get; private set; }
        public bool isSorted { get; private set; }

        public bool isFull => size - count <= 0;

        public int totalFreeSlots => size - count;

        public int nextOpenSlot
        {
            get
            {
                for (int i = 0; i < size; i++)
                    if (Slots[i] is null)
                        return i;
                return NO_OPEN_SLOTS;
            }
        }

        public void PutItem(int slot, ItemInstance item)
        {
            if (Slots[slot] != null)
                Slots[slot].location = ItemLocation.InvalidLocation;
            Slots[slot] = item;
            count++;
            isSorted = false;
        }

        public ItemInstance GetItem(int slot)
        {
            return Slots[slot];
        }

        public void RemoveItem(int slot)
        {
            if (Slots[slot] != null)
                Slots[slot].location = ItemLocation.InvalidLocation;
            Slots[slot] = null;
            count--;
            isSorted = false;
        }

        public bool HasItem(int slot)
        {
            return Slots[slot] != null;
        }

        public int GetNextOpenSlot(int startSlot)
        {
            for (int i = startSlot + 1; i < size; i++)
                if (Slots[i] is null)
                    return i;
            return NO_OPEN_SLOTS;
        }
    }
}
