using System;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class NpcSpawn : IInstance
    {
        public uint instanceId { get; set; }
        public int id { get; set; }
        public int npcId { get; set; }
        public int modelId { get; set; }
        public byte level { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public int mapId { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public float z { get; set; }
        public bool active { get; set; }
        public byte heading { get; set; }
        public short size { get; set; }
        public int visibility { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public int icon { get; set; }
        public int status { get; set; }
        public int statusX { get; set; }
        public int statusY { get; set; }
        public int statusZ { get; set; }
        public int radius { get; set; }



        public NpcSpawn()
        {
            created = DateTime.Now;
            updated = DateTime.Now;
            radius = 100;
        }
    }
}
