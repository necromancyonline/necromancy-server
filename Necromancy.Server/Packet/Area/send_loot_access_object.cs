using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive;
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
            int instanceID = packet.Data.ReadInt32();
            //MonsterSpawn monster = client.Map.GetMonsterByInstanceId((uint) instanceID);
            Logger.Debug($"{client.Character.Name} is trying to loot object {instanceID}");

            IBuffer res = BufferProvider.Provide();
            res.WriteInt64(1000);
            res.WriteByte(0);//bool
            Router.Send(client, (ushort)AreaPacketId.recv_self_exp_notify, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0);
            Router.Send(client, (ushort) AreaPacketId.recv_loot_access_object_r, res2, ServerType.Area);
            //LOOT, -1, I don't have anything. , SYSTEM_WARNING,
            //LOOT, -10, no route target, SYSTEM_WARNING,
            //LOOT, -207, There is no space in the inventory. , SYSTEM_WARNING,
            //LOOT, -1500, no root authority. , SYSTEM_WARNING,

     
        }
    }
}
