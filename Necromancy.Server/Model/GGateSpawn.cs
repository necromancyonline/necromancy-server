using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class GGateSpawn : IInstance
    {
        public uint instanceId { get; set; }
        public int id { get; set; }
        public int serialId { get; set; }
        public byte interaction { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int mapId { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public byte heading { get; set; }
        public int modelId { get; set; }
        public short size { get; set; }
        public int active { get; set; }
        public int glow { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }

        public GGateSpawn()
        {
            created = DateTime.Now;
            updated = DateTime.Now;
            interaction = 0;
            size = 100;
            glow = 0b0001;
            modelId = 1900001;
            active = 0;
            serialId = 1900001;

        }
    }
}
