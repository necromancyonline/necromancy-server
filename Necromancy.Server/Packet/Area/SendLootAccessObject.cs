using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Systems.Item;
using System;

namespace Necromancy.Server.Packet.Area
{
    public class SendLootAccessObject : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendLootAccessObject));

        private readonly NecServer _server;

        public SendLootAccessObject(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort id => (ushort) AreaPacketId.send_loot_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemService itemService = new ItemService(client.character);
            int result = 0;
            int instanceId = packet.data.ReadInt32();
            MonsterSpawn monster = client.map.GetMonsterByInstanceId((uint) instanceId);
            _Logger.Debug($"{client.character.name} is trying to loot object {instanceId}.  Inventory Space {client.character.itemLocationVerifier.GetTotalFreeSpace(ItemZoneType.AdventureBag)}");
            ItemLocation nextOpenLocation = client.character.itemLocationVerifier.NextOpenSlot(ItemZoneType.AdventureBag);

            if (monster == null) result = -10;
            else if (monster.loot.itemCountRng == 0) result = -1;
            else if (nextOpenLocation.zoneType == ItemZoneType.InvalidZone) result = -207; //expand to all inventory. TODO

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(result);
            router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res2, ServerType.Area);
            //LOOT, -1, I don't have anything. , SYSTEM_WARNING,
            //LOOT, -10, no route target, SYSTEM_WARNING,
            //LOOT, -207, There is no space in the inventory. , SYSTEM_WARNING,
            //LOOT, -1500, no root authority. , SYSTEM_WARNING,

            if (result == 0)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(2);
                router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

                int itemId = monster.loot.dropTableItemSerialIds[monster.loot.itemCountRng];
                ItemSpawnParams spawmParam = new ItemSpawnParams();
                spawmParam.itemStatuses = ItemStatuses.Identified;
                ItemInstance itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, itemId, spawmParam);
                _Logger.Debug(itemInstance.type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                router.Send(client, recvItemInstance.ToPacket());
                monster.loot.itemCountRng--; //decrement available items

                res = BufferProvider.Provide();
                router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
            }


        }
    }
}
