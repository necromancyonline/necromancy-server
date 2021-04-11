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
    public class SendPartyAcceptToInvite : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendPartyAcceptToInvite));

        public SendPartyAcceptToInvite(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_accept_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint partyInstanceId = packet.data.ReadUInt32();
            _Logger.Debug($"character {client.character.name} accepted invite to party ID {partyInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0); //error check?
            router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            Party myParty = server.instances.GetInstance(partyInstanceId) as Party;
            if (!myParty.partyMembers.Contains(client))
            {
                myParty.Join(client);
            } //add Accepting client to party list if it doesn't exist

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

            client.character.partyId = myParty.instanceId;

            RecvPartyNotifyEstablish recvPartyNotifyEstablish = new RecvPartyNotifyEstablish(client, myParty);
            router.Send(recvPartyNotifyEstablish, client); // Only establish the party for the acceptee. everyone else is already established.

            RecvCharaBodyNotifyPartyJoin recvCharaBodyNotifyPartyJoin = new RecvCharaBodyNotifyPartyJoin(client.character.instanceId, myParty.instanceId, myParty.partyType);
            router.Send(client.map, recvCharaBodyNotifyPartyJoin);//Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.

            RecvCharaNotifyPartyJoin recvCharaNotifyPartyJoin = new RecvCharaNotifyPartyJoin(client.character.instanceId, myParty.instanceId, myParty.partyType);
            router.Send(recvCharaNotifyPartyJoin, client); //Only send the Join of the Accepting Client to the Accepting Client.
        }

    }
}
