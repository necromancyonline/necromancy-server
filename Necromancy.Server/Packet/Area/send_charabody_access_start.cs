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
    public class send_charabody_access_start : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_charabody_access_start));
        public send_charabody_access_start(NecServer server) : base(server)
        {
            //ToDo :   If TargetId = Self.InstanceID,  warp to res statue.   if TargetId = Party member, Collect body.   else,  Loot
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_access_start;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint instanceId = packet.Data.ReadUInt32();
            Logger.Debug($"Accessing Body ID {instanceId}");
            client.Character.eventSelectReadyCode = instanceId; // store the Instance of the body you are looting on your character.

            //DeadBody deadBody = Server.Instances.GetInstance(instanceId) as DeadBody; //add case logic to detect different instance types.  monster, deadbody, other

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Insert logic gate here. should not always succeed
            Router.Send(client.Map, (ushort)AreaPacketId.recv_charabody_access_start_r, res, ServerType.Area);
        //SALVAGE_DEADBODY,-510,It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //SALVAGE_DEADBODY,-513, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        // SALVAGE_DEADBODY,-514, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //  SALVAGE_DEADBODY,-528, It is protected by a mysterious power., SYSTEM_IMPORTANCE,
        //   SALVAGE_DEADBODY,-526, It cannot be stolen from party members., SYSTEM_IMPORTANCE,
        //   SALVAGE_DEADBODY,-519, The soul is about to revive..., SYSTEM_NOTIFY,
        //   SALVAGE_DEADBODY,-507, No more corpses can be recovered., SYSTEM_NOTIFY,
   

            if (instanceId == 0)
                return;
            if (instanceId == client.Character.DeadBodyInstanceId)
            {
                Logger.Debug($"You've met with a terrible fate haven't you!");
                /////////
                //////  Insert Warp to Ressurection statue logic here.
                ///////
                return;
            }

            IInstance instance = Server.Instances.GetInstance(instanceId);

            switch (instance)
            {
                case DeadBody deadBody1:
                    client.Map.DeadBodies.TryGetValue(deadBody1.InstanceId, out deadBody1);
                    Logger.Debug($"Lootin  {deadBody1.SoulName}? You Criminal!!");
                    ItemService itemService = new ItemService(client.Character);
                    List<ItemInstance> lootableItems = itemService.getLootableItems(deadBody1.CharacterInstanceId);
                    foreach (ItemInstance itemInstance in lootableItems)
                    {
                        RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance);
                        Router.Send(client, recvItemInstanceUnidentified.ToPacket());
                    }
                    break;
                case MonsterSpawn monsterSpawn:
                    client.Map.MonsterSpawns.TryGetValue(monsterSpawn.InstanceId, out monsterSpawn);
                    Logger.Debug($"Lootin a {monsterSpawn.Name}? Hope you get some good stuff");
                    /////
                    ///     ---- Insert Monster Loot table logic here.  including draw box for parties
                    /////
                    break;
                case Character character:
                    //NecClient targetClient = client.Map.ClientLookup.GetByCharacterInstanceId(instance.InstanceId);
                    Logger.Error($"Lootin a {character.Name}? Shouldn't be doin that.  only deadbodies are lootable");
                    break;
                case NpcSpawn npcSpawn:
                    client.Map.NpcSpawns.TryGetValue(npcSpawn.InstanceId, out npcSpawn);
                    Logger.Error($"how are you looting an NPC?");
                    break;
                default:
                    Logger.Error($"Instance with InstanceId: {instance.InstanceId} does not exist");
                    break;
            }

        }
    }
}
