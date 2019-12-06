using System;
using System.Numerics;
using Necromancy.Server.Common.Instance;

namespace Necromancy.Server.Model
{
    public class MonsterSpawn : IInstance
    {
        public uint InstanceId { get; set; }
        public int Id { get; set; }
        public int MonsterId { get; set; }
        public int ModelId { get; set; }
        public byte Level { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public int MapId { get; set; }
        public bool Active { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public byte Heading { get; set; }
        public short Size { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public short Radius { get; set; }
        public int CurrentHp { get; set; }
        public int MaxHp { get; set; }

        public MonsterSpawn()
        {
            CurrentHp = 80085;
            MaxHp = 88887355;
            Created = DateTime.Now;
            Updated = DateTime.Now;
        }
    }
    public class MonsterCoord
    {
        public byte Heading { get; set; }
        public Vector3 destination { get; set; }
    }
}
