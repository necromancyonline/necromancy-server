using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_self_salvage_abort : ClientHandler
    {
        public send_charabody_self_salvage_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_self_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            NecClient necClient = Server.Clients.GetByCharacterInstanceId((uint)client.Character.eventSelectExecCode);

            necClient.BodyCollection.Remove(client.Character.DeadBodyInstanceId);
            DeadBody deadBody = Server.Instances.GetInstance(client.Character.DeadBodyInstanceId) as DeadBody;

            RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(0);
            Router.Send(client, recvCharaBodySelfSalvageEnd.ToPacket());

            deadBody.X = necClient.Character.X;
            deadBody.Y = necClient.Character.Y;
            deadBody.Z = necClient.Character.Z;
            client.Character.X = necClient.Character.X;
            client.Character.Y = necClient.Character.Y;
            client.Character.Z = necClient.Character.Z;

            deadBody.MapId = necClient.Character.MapId;
            client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
            if (client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
            {
                Server.Router.Send(client.Map, cBodyData.ToPacket());
            }

            RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(client.Character.DeadBodyInstanceId, 0);
            Router.Send(necClient, recvCharaBodySalvageEnd.ToPacket());

            //send your soul to all the other souls runnin around
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(client.Character, client.Soul.Name);
            foreach (NecClient soulStateClient in client.Map.ClientLookup.GetAll())
            {
                if (soulStateClient.Character.State == CharacterState.SoulForm) Server.Router.Send(soulStateClient, cData.ToPacket());
            }
        }
    }
}
