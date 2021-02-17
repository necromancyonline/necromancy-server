using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_access_start : ClientHandler
    {
        private ItemInstance _itemInstance;
        private byte _ItemZoneTypeOverride;
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

            //DeadBody deadBody = Server.Instances.GetInstance(instanceId) as DeadBody; //add case logic to detect different instance types.  monster, deadbody, other

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_charabody_access_start_r, res, ServerType.Area);


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

                    foreach (ItemInstance itemInstance in deadBody1.ItemManager.GetLootableItems())
                    {
                        _itemInstance = itemInstance;
                        _itemInstance.Statuses = ItemStatuses.Unidentified;
                        if (_itemInstance.Location.ZoneType == ItemZoneType.AdventureBag) _ItemZoneTypeOverride = (byte)ItemZoneType.BCAdventureBag;
                        if (_itemInstance.Location.ZoneType == ItemZoneType.EquippedBags) _ItemZoneTypeOverride = (byte)ItemZoneType.BCEquippedBag;
                        if (_itemInstance.Location.ZoneType == ItemZoneType.PremiumBag) _ItemZoneTypeOverride = (byte)ItemZoneType.BCPremiumBag;
                        RecvItemInstanceUnidentified recvItemInstanceUnidentified = new RecvItemInstanceUnidentified(client, itemInstance, _ItemZoneTypeOverride);
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
