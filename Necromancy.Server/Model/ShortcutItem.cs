using System;
using System.Collections.Generic;
using System.Text;

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
        public long id { get; }
        public ShortcutType type { get; }

        public ShortcutItem(long id, ShortcutType shortcutType)
        {
            this.id = id;
            type = shortcutType;
        }

    }



}
