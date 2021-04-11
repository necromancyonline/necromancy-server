using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System.Collections.Generic;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodyAccessStart : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharabodyAccessStart));
        public SendCharabodyAccessStart(NecServer server) : base(server)
        {
            //ToDo :   If TargetId = Self.InstanceID,  warp to res statue.   if TargetId = Party member, Collect body.   else,  Loot
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_access_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint instanceId = packet.data.ReadUInt32();
            _Logger.Debug($"Accessing Body ID {instanceId}");
            client.character.eventSelectReadyCode = instanceId; // store the Instance of the body you are looting on your character.

            //DeadBody deadBody = Server.Instances.GetInstance(instanceId) as DeadBody; //add case logic to detect different instance types.  monster, deadbody, other

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Insert logic gate here. should not always succeed
            router.Send(client.map, (ushort)AreaPacketId.recv_charabody_access_start_r, res, ServerType.Area);
        //SALVAGE_DEADBODY,-510,It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //SALVAGE_DEADBODY,-513, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        // SALVAGE_DEADBODY,-514, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //  SALVAGE_DEADBODY,-528, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //   SALVAGE_DEADBODY,-526, It cannot be stolen from party members., SYSTEM_IMPORTANCE,
        //   SALVAGE_DEADBODY,-519, The soul is about to revive..., SYSTEM_NOTIFY,
        //   SALVAGE_DEADBODY,-507, No more corpses can be recovered., SYSTEM_NOTIFY,


            if (instanceId == 0)
                return;
            if (instanceId == client.character.deadBodyInstanceId)
            {
                _Logger.Debug($"You've met with a terrible fate haven't you!");
                /////////
                //////  Insert Warp to Ressurection statue logic here.
                ///////
                return;
            }

            IInstance instance = server.instances.GetInstance(instanceId);

            switch (instance)
            {
                case DeadBody deadBody1:
                    client.map.deadBodies.TryGetValue(deadBody1.instanceId, out deadBody1);
                    _Logger.Debug($"Lootin  {deadBody1.soulName}? You Criminal!!");
                    ItemService itemService = new ItemService(client.character);
                    List<ItemInstance> lootableItems = itemService.GetLootableItems(deadBody1.characterInstanceId);
                    foreach (ItemInstance itemInstance in lootableItems)
                    {
                        RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                        router.Send(client, recvItemInstanceUnidentified.ToPacket());
                    }
                    break;
                case MonsterSpawn monsterSpawn:
                    client.map.monsterSpawns.TryGetValue(monsterSpawn.instanceId, out monsterSpawn);
                    _Logger.Debug($"Lootin a {monsterSpawn.name}? Hope you get some good stuff");
                    /////
                    ///     ---- Insert Monster Loot table logic here.  including draw box for parties
                    /////
                    break;
                case Character character:
                    //NecClient targetClient = client.Map.ClientLookup.GetByCharacterInstanceId(instance.InstanceId);
                    _Logger.Error($"Lootin a {character.name}? Shouldn't be doin that.  only deadbodies are lootable");
                    break;
                case NpcSpawn npcSpawn:
                    client.map.npcSpawns.TryGetValue(npcSpawn.instanceId, out npcSpawn);
                    _Logger.Error($"how are you looting an NPC?");
                    break;
                default:
                    _Logger.Error($"Instance with InstanceId: {instance.instanceId} does not exist");
                    break;
            }

        }
    }
}
