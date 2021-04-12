namespace Necromancy.Server.Systems.Item
{
    internal class ItemZone
    {
        public const int NO_CONTAINERS_WITH_SPACE = -1;

        public ItemZone(int maxContainers, int maxContainerSize)
        {
            this.maxContainers = maxContainers;
            this.maxContainerSize = maxContainerSize;
            containers = new Container[this.maxContainers];
        }

        public Container[] containers { get; }
        public int maxContainers { get; }
        public int maxContainerSize { get; }
        public int size { get; private set; }

        public int count
        {
            get
            {
                int count = 0;
                foreach (Container container in containers)
                    if (container != null)
                        count += container.count;
                return count;
            }
        }

        public int totalFreeSpace => size - count;

        public bool isFull
        {
            get
            {
                foreach (Container container in containers)
                    if (container != null && !container.isFull)
                        return false;
                return true;
            }
        }

        public int nextContainerWithSpace
        {
            get
            {
                for (int i = 0; i < maxContainers; i++)
                    if (containers[i] != null && containers[i].nextOpenSlot != Container.NO_OPEN_SLOTS)
                        return i;
                return NO_CONTAINERS_WITH_SPACE;
            }
        }

        public void PutContainer(int index, int size)
        {
            containers[index] = new Container(size);
            this.size += size;
        }

        public Container GetContainer(int index)
        {
            return containers[index];
        }

        public void RemoveContainer(int index)
        {
            size -= containers[index].size;
            containers[index] = null;
        }

        public bool HasContainer(int index)
        {
            return containers[index] != null;
        }

        public ItemLocation[] GetNextXFreeSpaces(ItemZoneType itemZoneType, int amount)
        {
            ItemLocation[] freeSpaces = new ItemLocation[amount];
            int index = 0;
            for (int i = 0; i < maxContainers; i++)
                if (containers[i] != null && !containers[i].isFull)
                {
                    int nextOpenSlot = containers[i].nextOpenSlot;
                    while (index < amount)
                    {
                        freeSpaces[index] = new ItemLocation(itemZoneType, (byte)i, (short)nextOpenSlot);
                        index++;
                        nextOpenSlot = containers[i].GetNextOpenSlot(nextOpenSlot);
                    }
                }

            return freeSpaces;
        }
    }
}
