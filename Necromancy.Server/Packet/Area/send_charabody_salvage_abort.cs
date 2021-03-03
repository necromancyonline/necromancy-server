using Arrowgene.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Model.CharacterModel;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Logging;


namespace Necromancy.Server.Packet.Area
{
    public class send_charabody_salvage_abort : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_charabody_salvage_abort));
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


            deadBody.X = client.Character.X;
            deadBody.Y = client.Character.Y;
            deadBody.Z = client.Character.Z;
            necClient.Character.X = client.Character.X;
            necClient.Character.Y = client.Character.Y;
            necClient.Character.Z = client.Character.Z;

            deadBody.MapId = client.Character.MapId;
            client.Map.DeadBodies.Add(deadBody.InstanceId, deadBody);
            RecvDataNotifyCharaBodyData cBodyData = new RecvDataNotifyCharaBodyData(deadBody);
            if (client.Map.Id.ToString()[0] != "1"[0]) //Don't Render dead bodies in town.  Town map ids all start with 1
            {
                Server.Router.Send(client.Map, cBodyData.ToPacket());
            }

            //must occur after the charaBody notify.
            RecvCharaBodySalvageEnd recvCharaBodySalvageEnd = new RecvCharaBodySalvageEnd(deadBody.InstanceId, 1);
            Router.Send(client, recvCharaBodySalvageEnd.ToPacket());

            //send your soul to all the other souls runnin around
            RecvDataNotifyCharaData cData = new RecvDataNotifyCharaData(necClient.Character, necClient.Soul.Name);
            foreach (NecClient soulStateClient in client.Map.ClientLookup.GetAll())
            {
                if (soulStateClient.Character.State == CharacterState.SoulForm) Server.Router.Send(soulStateClient, cData.ToPacket());
            }

        }
    }
}
