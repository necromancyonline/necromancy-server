using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Arrowgene.Logging;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Tasks.Core;

namespace Necromancy.Server.Tasks
{
    // create a map transition
    // Sets a refernce point for the thread to use -  _referencePos
    // Set 2 points and draw a line between them, if character crosses it change map  -  _transitionPos1, _transitionPos2
    // The transition is reversed on some maps - _invertedTransition
    // Once triggered set character.mapChange = true    this prevents updating character.X,Y and Z until transition is complete.

    public class MapTransitionTask : PeriodicTask
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(MapTransitionTask));
        private readonly int _id;
        private readonly uint _instanceId;
        private readonly bool _invertedTransition;

        private readonly Map _map;
        private readonly int _refDistance;
        private readonly Vector3 _referencePos;
        private readonly int _tickTime;
        private readonly MapPosition _toPos;
        private readonly int _transitionMapId;
        private readonly Vector3 _transitionPos1;
        private readonly Vector3 _transitionPos2;
        private bool _taskActive;

        public MapTransitionTask(NecServer server, Map map, int transitionMapId, Vector3 referencePos, int refDistance,
            Vector3 transitionPos1, Vector3 transitionPos2, uint instanceId, bool invertedTransition, MapPosition toPos,
            int id)
        {
            this.server = server;
            _map = map;
            _transitionPos1 = transitionPos1;
            _transitionPos2 = transitionPos2;
            _referencePos = referencePos;
            _refDistance = refDistance;
            _toPos = toPos;
            _instanceId = instanceId;
            _transitionMapId = transitionMapId;
            _taskActive = false;
            _invertedTransition = invertedTransition;
            _tickTime = 500;
            _id = id;
        }

        private NecServer server { get; }

        public override string taskName => $"MapTransitionTask : {_map.id}";
        public override TimeSpan taskTimeSpan { get; }
        protected override bool taskRunAtStart => false;


        protected override void Execute()
        {
            _taskActive = true;
            Thread.Sleep(1000);
            while (_taskActive)
            {
                List<Character> characters = _map.GetCharactersRange(_referencePos, _refDistance);
                foreach (Character character in characters)
                {
                    if (character.mapChange) continue;

                    Vector3 characterPos = new Vector3(character.x, character.y, character.z);
                    bool transition = TransitionCheck(characterPos);
                    if (transition)
                    {
                        if (!server.maps.TryGet(_transitionMapId, out Map transitionMap)) return;

                        NecClient client = _map.clientLookup.GetByCharacterInstanceId(character.instanceId);
                        client.character.mapChange = true;
                        transitionMap.EnterForce(client, _toPos);
                    }

                    _Logger.Debug(
                        $"{character.name} in range [transition] id {_id} Instance {_instanceId} to destination {_transitionMapId}[{transition}].");
                }

                Thread.Sleep(_tickTime);
            }

            Stop();
        }

        private bool TransitionCheck(Vector3 characterPos)
        {
            bool trasition = false;
            trasition = (characterPos.X - _transitionPos1.X) * (_transitionPos2.Y - _transitionPos1.Y) -
                (characterPos.Y - _transitionPos1.Y) * (_transitionPos2.X - _transitionPos1.X) <= 0;
            return trasition != _invertedTransition;
        }
    }
}
