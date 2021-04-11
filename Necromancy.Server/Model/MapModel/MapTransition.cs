using System;
using System.Numerics;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Tasks;

namespace Necromancy.Server.Model
{
    public class MapTransition : IInstance
    {
        public uint instanceId { get; set; }
        public int id { get; set; }

        private NecServer _server;
        private MapTransitionTask _transitionTask;
        public Vector3 referencePos;
        public int refDistance;
        public Vector3 leftPos;
        public Vector3 rightPos;
        public byte maplinkHeading;
        public int maplinkColor;
        public int maplinkOffset;
        public int maplinkWidth;
        public int mapId;
        public MapPosition toPos;
        public int transitionMapId;
        public bool state { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public bool invertedTransition;

        public MapTransition()
        {
            toPos = new MapPosition();
        }

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
