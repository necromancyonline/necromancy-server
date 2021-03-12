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
            ZoneType = zoneType;
            Container = container;
            Slot = slot;
            _hashcode = ((int) zoneType << 24) + (container << 16) + slot;
        }

        public ItemZoneType ZoneType { get; }
        public byte Container { get; }
        public short Slot { get; }

        private readonly int _hashcode;

        public bool Equals(ItemLocation other)
        {
            if(other.ZoneType != ZoneType)  return false;
            if(other.Container  != Container)   return false;
            if(other.Slot != Slot)  return false;
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
