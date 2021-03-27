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
    public class send_loot_access_object : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_loot_access_object));

        private readonly NecServer _server;

        public send_loot_access_object(NecServer server) : base(server)
        {
            _server = server;
        }

        public override ushort Id => (ushort) AreaPacketId.send_loot_access_object;

        public override void Handle(NecClient client, NecPacket packet)
        {
            ItemService itemService = new ItemService(client.Character);
            int result = 0;
            int instanceID = packet.Data.ReadInt32();
            MonsterSpawn monster = client.Map.GetMonsterByInstanceId((uint) instanceID);
            Logger.Debug($"{client.Character.Name} is trying to loot object {instanceID}.  Inventory Space {client.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag)}");

            if (monster == null) result = -10;
            else if (monster.loot.ItemCountRNG == 0) result = -1;
            else if (client.Character.ItemManager.GetTotalFreeSpace(ItemZoneType.AdventureBag) < 2) result = -207; //expand to all inventory. TODO            

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(result);
            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res2, ServerType.Area);
            //LOOT, -1, I don't have anything. , SYSTEM_WARNING,
            //LOOT, -10, no route target, SYSTEM_WARNING,
            //LOOT, -207, There is no space in the inventory. , SYSTEM_WARNING,
            //LOOT, -1500, no root authority. , SYSTEM_WARNING,

            if (result == 0)
            {
                IBuffer res = BufferProvider.Provide();
                res.WriteInt32(2);
                Router.Send(client, (ushort)AreaPacketId.recv_situation_start, res, ServerType.Area);

                int itemId = monster.loot.DropTableItemSerialIds[monster.loot.ItemCountRNG];
                ItemSpawnParams spawmParam = new ItemSpawnParams();
                spawmParam.ItemStatuses = ItemStatuses.Identified;
                ItemInstance itemInstance = itemService.SpawnItemInstance(ItemZoneType.AdventureBag, itemId, spawmParam);
                Logger.Debug(itemInstance.Type.ToString());
                RecvItemInstance recvItemInstance = new RecvItemInstance(client, itemInstance);
                Router.Send(client, recvItemInstance.ToPacket());
                monster.loot.ItemCountRNG--; //decrement available items

                res = BufferProvider.Provide();
                Router.Send(client, (ushort)AreaPacketId.recv_situation_end, res, ServerType.Area);
            }


        }
    }
}
