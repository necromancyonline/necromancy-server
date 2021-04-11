using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class SkillTreeItem
    {
        public int id { get; set; }
        public int skillId { get; set; }
        public int charId { get; set; }
        public int level { get; set; }

        public SkillTreeItem()
        {
            id = -1;
            skillId = -1;
            charId = -1;
            level = -1;
        }

    }
}
