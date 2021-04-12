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
        public bool InvertedTransition;
        public Vector3 LeftPos;
        public int MapId;
        public int MaplinkColor;
        public byte MaplinkHeading;
        public int MaplinkOffset;
        public int MaplinkWidth;
        public int RefDistance;
        public Vector3 ReferencePos;
        public Vector3 RightPos;
        public MapPosition ToPos;
        public int TransitionMapId;

        public MapTransition()
        {
            ToPos = new MapPosition();
        }

        public int id { get; set; }
        public bool state { get; set; }
        public DateTime created { get; set; }
        public DateTime updated { get; set; }
        public uint instanceId { get; set; }

        public void Start(NecServer server, Map map)
        {
            _server = server;
            _transitionTask = new MapTransitionTask(_server, map, TransitionMapId, ReferencePos, RefDistance, LeftPos,
                RightPos, instanceId, InvertedTransition, ToPos, id);
            _transitionTask.Start();
        }

        public void Stop()
        {
            _transitionTask.Stop();
        }
    }
}
