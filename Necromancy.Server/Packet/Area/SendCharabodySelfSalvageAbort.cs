using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodySelfSalvageAbort : ClientHandler
    {
        public SendCharabodySelfSalvageAbort(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_self_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient necClient = server.clients.GetByCharacterInstanceId((uint)client.character.eventSelectExecCode);

            necClient.bodyCollection.Remove(client.character.deadBodyInstanceId);
            DeadBody deadBody = server.instances.GetInstance(client.character.deadBodyInstanceId) as DeadBody;

            RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(0);
            router.Send(client, recvCharaBodySelfSalvageEnd.ToPacket());

            deadBody.x = necClient.character.x;
            deadBody.y = necClient.character.y;
            deadBody.z = necClient.character.z;
            client.character.x = necClient.character.x;
            client.character.y = necClient.character.y;
            client.character.z = necClient.character.z;

            deadBody.mapId = necClient.character.mapId;
            client.map.deadBodies.Add(deadBody.instanceId, deadBody);
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
            if (client.map.id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
            {
                server.router.Send(client.map, cBodyData.ToPacket());
            }

            RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(client.character.deadBodyInstanceId, 0);
            router.Send(necClient, recvCharaBodySalvageEnd.ToPacket());

            //send your soul to all the other souls runnin around
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.character, client.soul.name);
            foreach (NecClient soulStateClient in client.map.clientLookup.GetAll())
            {
                if (soulStateClient.character.state == CharacterState.SoulForm) server.router.Send(soulStateClient, cData.ToPacket());
            }
        }
    }
}
