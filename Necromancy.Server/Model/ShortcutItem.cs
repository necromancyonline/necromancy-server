namespace Necromancy.Server.Model
{
    public class ShortcutItem
    {
        public enum ShortcutType
        {
            Unknown0,
            Unknown1,
            Unknown2,
            Skill,
            System,
            Emote
        }

        public ShortcutItem(long id, ShortcutType shortcutType)
        {
            this.id = id;
            type = shortcutType;
        }

        public long id { get; }
        public ShortcutType type { get; }
    }
}
