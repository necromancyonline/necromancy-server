using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;

namespace Necromancy.Server.Packet.Area
{
    public class SendReviveExecute : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendReviveExecute));
        public SendReviveExecute(NecServer server) : base(server)
        {
        }


        public override ushort id => (ushort)AreaPacketId.send_revive_execute;

        public override void Handle(NecClient client, NecPacket packet)
        {


            client.character.hp.SetCurrent(client.character.hp.max);
            client.character.LoginCheckDead();
            client.character.state = Model.CharacterModel.CharacterState.NormalForm;
            client.map = server.maps.Get(client.character.mapId);

            RecvReviveExecute reviveExecute = new RecvReviveExecute();
            router.Send(client, reviveExecute.ToPacket());


            if (client.map != null && client.map.deadBodies.ContainsKey(client.character.deadBodyInstanceId))//Don't have access to last map on a reset of client, this fails.
            {
                RecvObjectDisappearNotify recvObjectDisappearNotify = new RecvObjectDisappearNotify(client.character.deadBodyInstanceId);
                router.Send(client.map, recvObjectDisappearNotify.ToPacket(), client);
                client.map.deadBodies.Remove(client.character.deadBodyInstanceId);
            }

            client.character.mapId = (1001001);//ilfalo port, main city, temple building.
            client.character.x = -3175;
            client.character.y = -1749;
            client.character.z = 6;
            client.character.heading = 141;

            if (!server.database.UpdateCharacter(client.character)) _Logger.Error("Could not update character after revive.");

        }
    }
}
