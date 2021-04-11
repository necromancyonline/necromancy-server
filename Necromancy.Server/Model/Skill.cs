using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Skill : IInstance
    {
        public uint instanceId { get; set; }
        public int id { get; set; }
        public string name { get; set; }

        public Skill()
        {
        }

    }
}
