using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    /// <summary>
    //Tell the entering client about all the other objects on the map being entered.
    /// </summary>
    public class SendMapGetInfo : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendMapGetInfo));

        private readonly NecServer _server;

        public SendMapGetInfo(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort)AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //you are dead here.  only getting soul form characters and NPCs.  sorry bro.
            if (client.character.state.HasFlag(CharacterState.SoulForm))
            {
                _Logger.Debug("Rendering Dead stuff");
                foreach (NecClient otherClient in client.map.clientLookup.GetAll())
                {
                    if (otherClient == client)
                        // skip myself
                        continue;
                    //Render all the souls if you are in soul form yourself
                    if (otherClient.character.state.HasFlag(CharacterState.SoulForm))
                    {
                        RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.character, otherClient.soul.name);
                        router.Send(otherCharacterData, client);
                    }

                    if (otherClient.union != null)
                    {
                        RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.character, otherClient.union.name);
                        router.Send(otherUnionData, client);
                    }
                }

                foreach (NpcSpawn npcSpawn in client.map.npcSpawns.Values)
                    if (npcSpawn.visibility == 2 | npcSpawn.visibility == 3) //2 is the magic number for soul state only.  make it an Enum some day
                    {
                        RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                        router.Send(npcData, client);
                    }
            }
            else //if you are not dead, do normal stuff.  else...  do dead person stuff
            {
                _Logger.Debug($"Not dead.  rendering living stuff.  CharacterState:{client.character.state}");
                foreach (NecClient otherClient in client.map.clientLookup.GetAll())
                {
                    if (otherClient == client) continue;
                    if (!otherClient.character.state.HasFlag(CharacterState.SoulForm))
                    {
                        RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.character, otherClient.soul.name);
                        router.Send(otherCharacterData, client);
                    }

                    if (otherClient.union != null)
                    {
                        RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.character, otherClient.union.name);
                        router.Send(otherUnionData, client);
                    }
                }

                foreach (MonsterSpawn monsterSpawn in client.map.monsterSpawns.Values)
                {
                    RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                    _Logger.Debug($"Monster Id {monsterSpawn.id} with model {monsterSpawn.modelId} is loading");
                    router.Send(monsterData, client);
                }

                foreach (NpcSpawn npcSpawn in client.map.npcSpawns.Values)
                    if (npcSpawn.visibility == 1 | npcSpawn.visibility == 3) //2 is the magic number for soul state only.  make it an Enum some day
                    {
                        RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                        router.Send(npcData, client);
                    }
            }

            //Allways render the stuff below this line.
            if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
                foreach (DeadBody deadBody in client.map.deadBodies.Values)
                {
                    RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                    router.Send(deadBodyData, client);
                }

            foreach (Gimmick gimmickSpawn in client.map.gimmickSpawns.Values)
            {
                RecvDataNotifyGimmickData gimmickData = new RecvDataNotifyGimmickData(gimmickSpawn);
                router.Send(gimmickData, client);
            }

            foreach (GGateSpawn gGateSpawn in client.map.gGateSpawns.Values)
            {
                RecvDataNotifyGGateStoneData gGateSpawnData = new RecvDataNotifyGGateStoneData(gGateSpawn);
                router.Send(gGateSpawnData, client);
            }

            foreach (MapTransition mapTran in client.map.mapTransitions.Values)
            {
                RecvDataNotifyMapLink mapLink = new RecvDataNotifyMapLink(mapTran.instanceId, mapTran.referencePos, mapTran.maplinkOffset, mapTran.maplinkWidth, mapTran.maplinkColor, mapTran.maplinkHeading);
                router.Send(mapLink, client);

                //un-comment for debugging maplinks to visualize the left and right Reference Positions
                /*
                GGateSpawn gGateSpawn = new GGateSpawn();
                Server.Instances.AssignInstance(gGateSpawn);
                gGateSpawn.X = mapTran.LeftPos.X;
                gGateSpawn.Y = mapTran.LeftPos.Y;
                gGateSpawn.Z = mapTran.LeftPos.Z;
                gGateSpawn.Heading = mapTran.MaplinkHeading;
                gGateSpawn.Name = $"This is the Left Side of the transition";
                gGateSpawn.Title = $"X: {mapTran.LeftPos.X}  Y:{mapTran.LeftPos.Y} ";
                gGateSpawn.MapId = mapTran.MapId;
                gGateSpawn.ModelId = 1805000;
                gGateSpawn.Active = 0;
                gGateSpawn.SerialId = 1900001;

                RecvDataNotifyGGateData gGateData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateData, client);

                gGateSpawn = new GGateSpawn();
                Server.Instances.AssignInstance(gGateSpawn);
                gGateSpawn.X = mapTran.RightPos.X;
                gGateSpawn.Y = mapTran.RightPos.Y;
                gGateSpawn.Z = mapTran.RightPos.Z;
                gGateSpawn.Heading = mapTran.MaplinkHeading;
                gGateSpawn.Name = $"This is the Right Side of the transition";
                gGateSpawn.Title = $"X: {mapTran.RightPos.X}  Y:{mapTran.RightPos.Y} ";
                gGateSpawn.MapId = mapTran.MapId;
                gGateSpawn.ModelId = 1805000;
                gGateSpawn.Active = 0;
                gGateSpawn.SerialId = 1900002;

                gGateData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateData, client);
                */
            }

            // ToDo this should be a database lookup
            RecvMapFragmentFlag mapFragments = new RecvMapFragmentFlag(client.map.id, 0xff);
            router.Send(mapFragments, client);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_map_get_info_r, res, ServerType.Area);
        }
    }
}
