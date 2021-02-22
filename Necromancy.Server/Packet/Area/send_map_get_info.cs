using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_map_get_info : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_map_get_info));

        private readonly NecServer _server;

        public send_map_get_info(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_map_get_info;

        public override void Handle(NecClient client, NecPacket packet)
        {
            //if you are not dead, do normal stuff.  else...  do dead person stuff
            if (client.Character.State != Model.CharacterModel.CharacterState.SoulForm)
            {
                Logger.Debug($"Not dead.  rendering living stuff.  CharacterState:{client.Character.State}");
                foreach (NecClient otherClient in client.Map.ClientLookup.GetAll())
                {
                    if (otherClient == client) continue;
                    if (otherClient.Character.State != Model.CharacterModel.CharacterState.SoulForm)
                    {
                        RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                        Router.Send(otherCharacterData, client);
                    }     
                    if (otherClient.Union != null)
                    {
                        RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                        Router.Send(otherUnionData, client);
                    }
                }

                foreach (MonsterSpawn monsterSpawn in client.Map.MonsterSpawns.Values)
                {
                    RecvDataNotifyMonsterData monsterData = new RecvDataNotifyMonsterData(monsterSpawn);
                    Logger.Debug($"Monster Id {monsterSpawn.Id} with model {monsterSpawn.ModelId} is loading");
                    Router.Send(monsterData, client);
                }

                foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
                {
                    if (npcSpawn.Visibility != 2) //2 is the magic number for soul state only.  make it an Enum some day
                    {
                        RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                        Router.Send(npcData, client);
                    }
                }


            }
            else //you are dead here.  only getting soul form characters and NPCs.  sorry bro.
            {
                Logger.Debug($"Rendering Dead stuff");
                foreach (NecClient otherClient in client.Map.ClientLookup.GetAll())
                {
                    if (otherClient == client)
                    {
                        // skip myself
                        continue;
                    }
                    //Render all the souls if you are in soul form yourself
                    if (otherClient.Character.State == Model.CharacterModel.CharacterState.SoulForm)
                    {
                        RecvDataNotifyCharaData otherCharacterData = new RecvDataNotifyCharaData(otherClient.Character, otherClient.Soul.Name);
                        Router.Send(otherCharacterData, client);
                    }                   

                    if (otherClient.Union != null)
                    {
                        RecvDataNotifyUnionData otherUnionData = new RecvDataNotifyUnionData(otherClient.Character, otherClient.Union.Name);
                        Router.Send(otherUnionData, client);
                    }
                }
                foreach (NpcSpawn npcSpawn in client.Map.NpcSpawns.Values)
                {
                    if (npcSpawn.Visibility == 2) //2 is the magic number for soul state only.  make it an Enum some day
                    {
                        RecvDataNotifyNpcData npcData = new RecvDataNotifyNpcData(npcSpawn);
                        Router.Send(npcData, client);
                    }
                }

            }
            //Allways render the stuff below this line.
            if (client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
            {
                foreach (DeadBody deadBody in client.Map.DeadBodies.Values)
                {
                    RecvDataNotifyCharaBodyData deadBodyData = new RecvDataNotifyCharaBodyData(deadBody);
                    Router.Send(deadBodyData, client);
                }
            }

            foreach (Gimmick gimmickSpawn in client.Map.GimmickSpawns.Values)
            {
                RecvDataNotifyGimmickData gimmickData = new RecvDataNotifyGimmickData(gimmickSpawn);
                Router.Send(gimmickData, client);
            }

            foreach (GGateSpawn gGateSpawn in client.Map.GGateSpawns.Values)
            {
                RecvDataNotifyGGateData gGateSpawnData = new RecvDataNotifyGGateData(gGateSpawn);
                Router.Send(gGateSpawnData, client);
            }

            foreach (MapTransition mapTran in client.Map.MapTransitions.Values)
            {
                RecvDataNotifyMapLink mapLink = new RecvDataNotifyMapLink(mapTran.InstanceId, mapTran.ReferencePos, mapTran.MaplinkOffset, mapTran.MaplinkWidth, mapTran.MaplinkColor, mapTran.MaplinkHeading);
                Router.Send(mapLink, client);

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
            RecvMapFragmentFlag mapFragments = new RecvMapFragmentFlag(client.Map.Id, 0xff);
            Router.Send(mapFragments, client);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_map_get_info_r, res, ServerType.Area);
        }
    }
}
