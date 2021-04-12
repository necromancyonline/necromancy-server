namespace Necromancy.Server.Model
{
    public class ShortcutBar
    {
        public const int COUNT = 10;

        public ShortcutBar()
        {
            item = new ShortcutItem[COUNT];
        }

        public ShortcutItem[] item { get; }
    }
}
