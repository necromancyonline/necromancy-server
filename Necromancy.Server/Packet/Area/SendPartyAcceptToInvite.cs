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
    public class send_party_accept_to_invite : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_accept_to_invite));

        public send_party_accept_to_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_accept_to_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint partyInstanceId = packet.Data.ReadUInt32();
            Logger.Debug($"character {client.Character.Name} accepted invite to party ID {partyInstanceId}");

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(0); //error check?
            Router.Send(client, (ushort) AreaPacketId.recv_party_accept_to_invite_r, res, ServerType.Area);

            Party myParty = Server.Instances.GetInstance(partyInstanceId) as Party;
            if (!myParty.PartyMembers.Contains(client))
            {
                myParty.Join(client);
            } //add Accepting client to party list if it doesn't exist

            foreach (NecClient partyClient in myParty.PartyMembers)
            {
                //Sanity check.  Who is in the party List at the time of Accepting the invite?
                Logger.Debug(
                    $"my party with instance ID {myParty.InstanceId} contains members {partyClient.Character.Name}");
                List<NecClient> DisposableList = new List<NecClient>();
                foreach (NecClient DisposablePartyClient in myParty.PartyMembers)
                {
                    DisposableList.Add(DisposablePartyClient);
                    Logger.Debug($"Added {DisposablePartyClient.Character.Name} to disposable list");
                }

                RecvPartyNotifyAddMember recvPartyNotifyAddMember = new RecvPartyNotifyAddMember(partyClient);
                Router.Send(recvPartyNotifyAddMember, DisposableList);

                Logger.Debug($"Adding member {partyClient.Character.Name} to Roster ");
            }

            client.Character.partyId = myParty.InstanceId;

            RecvPartyNotifyEstablish recvPartyNotifyEstablish = new RecvPartyNotifyEstablish(client, myParty);
            Router.Send(recvPartyNotifyEstablish, client); // Only establish the party for the acceptee. everyone else is already established.

            RecvCharaBodyNotifyPartyJoin recvCharaBodyNotifyPartyJoin = new RecvCharaBodyNotifyPartyJoin(client.Character.InstanceId, myParty.InstanceId, myParty.PartyType);
            Router.Send(client.Map, recvCharaBodyNotifyPartyJoin);//Only send the Join Notify of the Accepting Client to the Map.  Existing members already did that when they joined.

            RecvCharaNotifyPartyJoin recvCharaNotifyPartyJoin = new RecvCharaNotifyPartyJoin(client.Character.InstanceId, myParty.InstanceId, myParty.PartyType);
            Router.Send(recvCharaNotifyPartyJoin, client); //Only send the Join of the Accepting Client to the Accepting Client. 
        }

    }
}
