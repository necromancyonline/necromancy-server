using System.Numerics;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Object : IInstance
    {
        public int id { get; set; }
        public string name { get; set; }
        public Vector3 objectCoord { get; set; }
        public Vector3 triggerCoord { get; set; }
        public int bitmap1 { get; set; }
        public int bitmap2 { get; set; }
        public byte heading { get; set; }
        public int unknown1 { get; set; }
        public int unknown2 { get; set; }
        public int unknown3 { get; set; }
        public uint instanceId { get; set; }
    }
}
