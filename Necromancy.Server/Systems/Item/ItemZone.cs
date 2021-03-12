using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    class ItemZone
    {
        public const int NO_CONTAINERS_WITH_SPACE = -1;
        public Container[] _containers { get; }
        public int MaxContainers { get; }
        public int MaxContainerSize { get; }
        public int Size { get; private set; }
        public int Count
        {
            get
            {
                int count = 0;
                foreach(Container container in _containers)
                {
                    if(container != null)
                    count += container.Count;
                }
                return count;
            }
        }

        public int TotalFreeSpace
        {
            get
            {
                return Size - Count;
            }
        }

        public bool IsFull
        {
            get
            {
                foreach(Container container in _containers)
                {
                    if (container != null && !container.IsFull) return false;
                }
                return true;
            }
        }
        
        public ItemZone(int maxContainers, int maxContainerSize)
        {
            MaxContainers = maxContainers;
            MaxContainerSize = maxContainerSize;
            _containers = new Container[MaxContainers];
        }

        public int NextContainerWithSpace
        {
            get
            {
                for (int i = 0; i < MaxContainers; i++)
                {
                    if (_containers[i] != null && _containers[i].NextOpenSlot != Container.NO_OPEN_SLOTS) return i;
                }
                return NO_CONTAINERS_WITH_SPACE;
            }
        }
        public void PutContainer(int index, int size)
        {
            _containers[index] = new Container(size);
            Size += size;
        }
        public Container GetContainer(int index)
        {
            return _containers[index];
        }
        public void RemoveContainer(int index)
        {
            Size -= _containers[index].Size;
            _containers[index] = null;
        }
        public bool HasContainer(int index)
        {
            return _containers[index] != null;
        }

        public ItemLocation[] GetNextXFreeSpaces(ItemZoneType itemZoneType, int amount)
        {
            ItemLocation[] freeSpaces = new ItemLocation[amount];
            int index = 0;
            for (int i = 0; i < MaxContainers; i++)
            {
                if (_containers[i] != null && !_containers[i].IsFull)
                {
                    int nextOpenSlot = _containers[i].NextOpenSlot;
                    while (index < amount)
                    {
                        freeSpaces[index] = new ItemLocation(itemZoneType, (byte)i, (short)nextOpenSlot);
                        index++;
                        nextOpenSlot = _containers[i].GetNextOpenSlot(nextOpenSlot);
                    }
                }
            }
            return freeSpaces;
        }

    }
}
