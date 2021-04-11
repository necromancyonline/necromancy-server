using System;
using System.Collections.Generic;
using System.Text;

namespace Necromancy.Server.Systems.Item
{
    public struct ItemLocation : IEquatable<ItemLocation>
    {
        public static readonly ItemLocation InvalidLocation = new ItemLocation(ItemZoneType.InvalidZone,0,0);
        public ItemLocation(ItemZoneType zoneType, byte container, short slot)
        {
            this.zoneType = zoneType;
            this.container = container;
            this.slot = slot;
            _hashcode = ((int) zoneType << 24) + (container << 16) + slot;
        }

        public ItemZoneType zoneType { get; }
        public byte container { get; }
        public short slot { get; }

        private readonly int _hashcode;

        public bool Equals(ItemLocation other)
        {
            if(other.zoneType != zoneType)  return false;
            if(other.container  != container)   return false;
            if(other.slot != slot)  return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ItemLocation && Equals((ItemLocation) obj);
        }

        public override int GetHashCode() { return _hashcode; }
    }
}
