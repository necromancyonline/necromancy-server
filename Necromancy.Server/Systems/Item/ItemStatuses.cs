using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum ItemStatuses
    {
        Unidentified = 1 << 0,
        Identified = 1 << 1,
        Broken = 1 << 2,
        Cursed = 1 << 3,
        Blessed = 1 << 4
    }
}
