using System.Collections.Generic;
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
    public class SendPartyAcceptToApply : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendPartyAcceptToApply));

        public SendPartyAcceptToApply(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_accept_to_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint applicantInstanceId = packet.data.ReadUInt32(); //Could be a Party ID value hidden as character-who-made-it's value
            _Logger.Debug($"character {client.character.name} accepted Application to party from character Instance ID {applicantInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_party_accept_to_apply_r, res, ServerType.Area);

            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;
            NecClient applicantClient = server.clients.GetByCharacterInstanceId(applicantInstanceId);
            myParty.Join(applicantClient);

            applicantClient.character.partyId = myParty.instanceId;

            RecvPartyNotifyEstablish recvPartyNotifyEstablish = new RecvPartyNotifyEstablish(applicantClient, myParty);
            router.Send(recvPartyNotifyEstablish, applicantClient); // Only establish the party for the applicant. everyone else is already established.


            foreach (NecClient partyClient in myParty.partyMembers)
            {
                //Sanity check.  Who is in the party List at the time of Accepting the invite?
                _Logger.Debug(
                    $"my party with instance ID {myParty.instanceId} contains members {partyClient.character.name}");
                List<NecClient> disposableList = new List<NecClient>();
                foreach (NecClient disposablePartyClient in myParty.partyMembers)
                {
                    disposableList.Add(disposablePartyClient);
                    _Logger.Debug($"Added {disposablePartyClient.character.name} to disposable list");
                }

                RecvPartyNotifyAddMember recvPartyNotifyAddMember = new RecvPartyNotifyAddMember(partyClient);
                router.Send(recvPartyNotifyAddMember, disposableList);

                _Logger.Debug($"Adding member {partyClient.character.name} to Roster ");
            }

            RecvCharaBodyNotifyPartyJoin recvCharaBodyNotifyPartyJoin = new RecvCharaBodyNotifyPartyJoin(client.character.instanceId, myParty.instanceId, myParty.partyType);
            router.Send(client.map, recvCharaBodyNotifyPartyJoin); //Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.
        }
    }
}
