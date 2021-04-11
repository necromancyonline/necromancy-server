using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendCharabodySalvageRequest : ClientHandler
    {
        public SendCharabodySalvageRequest(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_charabody_salvage_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetId = packet.data.ReadUInt32();
            client.map.deadBodies.TryGetValue(targetId, out DeadBody deadbody);
            NecClient necClient = server.clients.GetByCharacterInstanceId(deadbody.characterInstanceId);

            if (deadbody != null) deadbody.salvagerId = client.character.instanceId;

            //ask the soul if they want to be collected. gotta have consent!
            RecvCharaBodySelfSalvageNotify recvCharaBodySelfSalvageNotify = new RecvCharaBodySelfSalvageNotify(client.character.name, client.soul.name);
            if (necClient != null)
            {
                router.Send(necClient, recvCharaBodySelfSalvageNotify.ToPacket());
                necClient.character.eventSelectExecCode = (int)client.character.instanceId;
            }

        }
    }
}
