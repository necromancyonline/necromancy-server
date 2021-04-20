using Arrowgene.Buffers;
using Arrowgene.Logging;
using Necromancy.Server.Common;
using Necromancy.Server.Logging;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;
using Necromancy.Server.Packet.Receive.Msg;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyInvite : ClientHandler
    {
        private static readonly NecLogger _Logger = LogProvider.Logger<NecLogger>(typeof(SendPartyInvite));

        public SendPartyInvite(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_invite;

        public override void Handle(NecClient client, NecPacket packet)
        {
            /*int32, unknown. probably party mode.
              int32, target client(character) id[i think this is the sends structure]*/
            int unknown = packet.data.ReadInt32(); //party mode
            uint targetInstanceId = packet.data.ReadUInt32();
            NecClient targetClient = server.clients.GetByCharacterInstanceId(targetInstanceId);
            Party party = server.instances.GetInstance(client.character.partyId) as Party;
            targetClient.character.partyRequest = client.character.instanceId;

            if (targetInstanceId == 0) targetInstanceId = client.character.instanceId;

            _Logger.Debug($"ID {client.character.instanceId} {client.character.name} sent a party: ${party.instanceId} invite to {targetClient.character.name} with instance ID {targetInstanceId}");

            //acknowledge the send, tell it the recv logic is complete.
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //error check
            router.Send(client, (ushort)AreaPacketId.recv_party_invite_r, res, ServerType.Area);

            RecvPartyNotifyInvite recvPartyNotifyInvite = new RecvPartyNotifyInvite(client, party);
            router.Send(recvPartyNotifyInvite, targetClient);
        }
    }
}
