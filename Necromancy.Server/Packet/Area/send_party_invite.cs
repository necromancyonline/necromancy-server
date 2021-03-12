using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_invite : ClientHandler
    {
        private static readonly NecLogger Logger = LogProvider.Logger<NecLogger>(typeof(send_party_invite));

        public send_party_invite(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_party_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, unknown. probably party mode.
              int32, target client(character) id[i think this is the sends structure]*/
            int unknown = packet.Data.ReadInt32();//party mode
            uint targetInstanceId = packet.Data.ReadUInt32();
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);
            Party party = Server.Instances.GetInstance(client.Character.partyId) as Party;
            targetClient.Character.partyRequest = client.Character.InstanceId;

            if (targetInstanceId == 0)
            {
                targetInstanceId = client.Character.InstanceId;
            } //band-aid for null reference errors while testing. to-do Delete this line.

            Logger.Debug($"ID {client.Character.InstanceId} {client.Character.Name} sent a party: ${party.InstanceId} invite to {targetClient.Character.Name} with instance ID {targetInstanceId}");

            //acknowledge the send, tell it the recv logic is complete.
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check
            Router.Send(client, (ushort)AreaPacketId.recv_party_invite_r, res, ServerType.Area);

            RecvPartyNotifyInvite recvPartyNotifyInvite = new RecvPartyNotifyInvite(client, party);
            Router.Send(recvPartyNotifyInvite, targetClient);
        }

    }
}
