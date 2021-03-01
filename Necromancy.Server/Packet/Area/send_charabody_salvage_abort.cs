using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_abort : ClientHandler
    {
        public send_charabody_salvage_abort(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_charabody_salvage_abort;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetId = packet.Data.ReadUInt32();
            client.BodyCollection.TryGetValue(targetId, out NecClient necClient);
            client.BodyCollection.Remove(targetId);
            DeadBody deadBody = Server.Instances.GetInstance(targetId) as DeadBody;

            RecvCharaBodySelfSalvageEnd recvCharaBodySelfSalvageEnd = new RecvCharaBodySelfSalvageEnd(0);
            Router.Send(necClient, recvCharaBodySelfSalvageEnd.ToPacket());
            RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(0, targetId);
            Router.Send(client, recvCharaBodySalvageEnd.ToPacket());

            //tell the salvager about your choice.
            RecvCharaBodySalvageRequest recvCharaBodySalvageRequest = new RecvCharaBodySalvageRequest(1);
            Router.Send(client, recvCharaBodySalvageRequest.ToPacket());

            deadBody.X = client.Character.X;
            deadBody.Y = client.Character.Y;
            deadBody.Z = client.Character.Z;
            necClient.Character.X = client.Character.X;
            necClient.Character.Y = client.Character.Y;
            necClient.Character.Z = client.Character.Z;

            deadBody.MapId = client.Character.MapId;
            client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
            Server.Router.Send(client.Map, cBodyData.ToPacket());
            //send your soul to all the other souls runnin around
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(necClient.Character, necClient.Soul.Name);
            foreach (NecClient soulStateClient in client.Map.ClientLookup.GetAll())
            {
                if (soulStateClient.Character.State == CharacterState.SoulForm) Server.Router.Send(soulStateClient, cData.ToPacket());
            }

        }
    }
}
