using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_request : ClientHandler
    {
        public send_charabody_salvage_request(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_salvage_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetId = packet.Data.ReadUInt32();
            client.Map.DeadBodies.TryGetValue(targetId, out DeadBody deadbody);
            NecClient necClient = Server.Clients.GetByCharacterInstanceId(deadbody.CharacterInstanceId);

            if (deadbody != null) deadbody.SalvagerId = client.Character.InstanceId;

            //ask the soul if they want to be collected. gotta have consent!
            RecvCharaBodySelfSalvageNotify recvCharaBodySelfSalvageNotify = new RecvCharaBodySelfSalvageNotify(client.Character.Name, client.Soul.Name);
            if (necClient != null)
            {
                Router.Send(necClient, recvCharaBodySelfSalvageNotify.ToPacket());
                necClient.Character.eventSelectExecCode = (int)client.Character.InstanceId;
            }

        }
    }
}
