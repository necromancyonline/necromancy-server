using System;
using System.Threading;

namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public const int Count = 10;
        public ShortcutItem[] item { get; }
        public ShortcutBar()
        {
            item = new ShortcutItem[Count];
        }
    }
}
