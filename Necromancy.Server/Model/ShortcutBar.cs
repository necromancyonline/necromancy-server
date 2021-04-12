namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public const int Count = 10;

        public ShortcutBar()
        {
            item = new ShortcutItem[Count];
        }

        public ShortcutItem[] item { get; }
    }
}
