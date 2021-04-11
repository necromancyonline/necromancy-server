using Arrowgene.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Logging;


namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodySalvageAbort : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendCharabodySalvageAbort));
        public SendCharabodySalvageAbort(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetId = packet.data.ReadUInt32();
            client.bodyCollection.TryGetValue(targetId, out NecClient necClient);
            client.bodyCollection.Remove(targetId);
            DeadBody deadBody = server.instances.GetInstance(targetId) as DeadBody;

            RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(0);
            router.Send(necClient, recvCharaBodySelfSalvageEnd.ToPacket());


            deadBody.x = client.character.x;
            deadBody.y = client.character.y;
            deadBody.z = client.character.z;
            necClient.character.x = client.character.x;
            necClient.character.y = client.character.y;
            necClient.character.z = client.character.z;

            deadBody.mapId = client.character.mapId;
            client.map.deadBodies.Add(deadBody.instanceId, deadBody);
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
            if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
            {
                server.router.Send(client.map, cBodyData.ToPacket());
            }

            //must occur after the charaBody notify.
            RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(deadBody.instanceId, 1);
            router.Send(client, recvCharaBodySalvageEnd.ToPacket());

            //send your soul to all the other souls runnin around
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(necClient.character, necClient.soul.name);
            foreach (NecClient soulStateClient in client.map.clientLookup.GetAll())
            {
                if (soulStateClient.character.state == CharacterState.SoulForm) server.router.Send(soulStateClient, cData.ToPacket());
            }

        }
    }
}
