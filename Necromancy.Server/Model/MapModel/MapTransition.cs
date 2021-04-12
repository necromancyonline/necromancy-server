using System;
using System.Numerics;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class MapTransition : IInstance
    {
        private NecServer _server;
        private MapTransitionTask _transitionTask;
        public bool invertedTransition;
        public Vector3 leftPos;
        public int mapId;
        public int maplinkColor;
        public byte maplinkHeading;
        public int maplinkOffset;
        public int maplinkWidth;
        public int refDistance;
        public Vector3 referencePos;
        public Vector3 rightPos;
        public MapPosition toPos;
        public int transitionMapId;

        public MapTransition()
        {
            toPos = new MapPosition();
        }

        public int id { get; set; }
        public bool state { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public uint instanceId { get; set; }

        public void Start(NecServer server, Map map)
        {
            _server = server;
            _transitionTask = new MapTransitionTask(_server, map, transitionMapId, referencePos, refDistance, leftPos,
                rightPos, instanceId, invertedTransition, toPos, id);
            _transitionTask.Start();
        }

        public void Stop()
        {
            _transitionTask.Stop();
        }
    }
}
