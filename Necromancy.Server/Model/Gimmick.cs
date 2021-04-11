using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class Gimmick : IInstance
    {
        public uint instanceId { get; set; }
        public int id { get; set; }
        public int mapId { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public byte heading { get; set; }
        public int modelId { get; set; }
        public int state { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public Gimmick()
        {
            created = DateTime.Now;
            updated = DateTime.Now;
        }
    }
}
