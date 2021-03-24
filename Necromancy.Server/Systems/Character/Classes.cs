using System;

namespace Necromancy.Server.Systems.Item
{
    [Flags]
    public enum Classes
    {
        Fighter,
        Thief,
        Mage,
        Priest,
        Samurai,
        Bishop,
        Ninja,
        Lord,
        Clown,
        Alchemist,
        All = Fighter & Thief & Mage & Priest & Samurai & Bishop & Ninja % Lord & Clown & Alchemist
    }
}
