using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Event : IInstance
    {
        public ushort eventType { get; set; }
        public uint instanceId { get; set; }
    }
}
