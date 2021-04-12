using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Area;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyEstablish : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendPartyEstablish));

        public SendPartyEstablish(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_establish;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, party type: 1 = open, 0 = closed
              int32, normal item distribution: 1 = random, 0 = do not distribute
              int32, rare item distribution: 1 = draw, 0 = do not distribute
              int32, target client(character) id*/

            int partyType = packet.data.ReadInt32();
            int normItemDist = packet.data.ReadInt32();
            int rareItemDist = packet.data.ReadInt32();
            uint targetInstanceId = packet.data.ReadUInt32();

            Party myFirstParty = new Party();
            server.instances.AssignInstance(myFirstParty);
            myFirstParty.partyType = partyType;
            myFirstParty.normalItemDist = normItemDist;
            myFirstParty.rareItemDist = rareItemDist;
            myFirstParty.targetClientId = targetInstanceId;
            myFirstParty.Join(client);
            myFirstParty.partyLeaderId = client.character.instanceId;
            client.character.partyId = myFirstParty.instanceId;

            RecvPartyNotifyEstablish recvPartyNotifyEstablish = new RecvPartyNotifyEstablish(client, myFirstParty);
            router.Send(recvPartyNotifyEstablish, client); // Only establish the party for the acceptee. everyone else is already established.

            RecvCharaBodyNotifyPartyJoin recvCharaBodyNotifyPartyJoin = new RecvCharaBodyNotifyPartyJoin(client.character.instanceId, myFirstParty.instanceId, myFirstParty.partyType);
            router.Send(client.map, recvCharaBodyNotifyPartyJoin); //Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.

            RecvCharaNotifyPartyJoin recvCharaNotifyPartyJoin = new RecvCharaNotifyPartyJoin(client.character.instanceId, myFirstParty.instanceId, myFirstParty.partyType);
            router.Send(recvCharaNotifyPartyJoin, client); //Only send the Join of the Accepting Client to the Accepting Client.

            if (targetInstanceId != 0)
            {
                NecClient targetClient = server.clients.GetByCharacterInstanceId(targetInstanceId);
                RecvPartyNotifyInvite recvPartyNotifyInvite = new RecvPartyNotifyInvite(targetClient, myFirstParty);
                router.Send(recvPartyNotifyInvite, targetClient);
            }

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_party_establish_r, res, ServerType.Area);
        }
    }
}
