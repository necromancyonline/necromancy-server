using Necromancy.Server.Data.Setting;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Event : IInstance
    {
        public uint instanceId { get; set; }
        public ushort eventType { get; set; }
        public Event()
        {
        }
    }
}
