using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model.Union
{
    public class UnionNewsEntry : IInstance
    {
        public UnionNewsEntry()
        {
            instanceId = 0;
            characterSoulName = "";
            characterName = "";
            string3 = "";
            string4 = "";
            itemCount = 0;
        }

        public string characterSoulName { get; set; }

        public string characterName { get; set; }
        public uint activity { get; set; }
        public string string3 { get; set; }
        public string string4 { get; set; }
        public int itemCount { get; set; }
        public uint instanceId { get; set; }
    }
}
