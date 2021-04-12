using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Skill : IInstance
    {
        public int id { get; set; }
        public string name { get; set; }
        public uint instanceId { get; set; }
    }
}
