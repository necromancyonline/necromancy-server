using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyKick : ClientHandler
    {
        public SendPartyKick(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_kick;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint kickTargetInstanceId = packet.data.ReadUInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //Kick Reason?  error check?  probably error check
            router.Send(client, (ushort) AreaPacketId.recv_party_kick_r, res, ServerType.Area);


            NecClient targetClient = server.clients.GetByCharacterInstanceId(kickTargetInstanceId);

            router.Send(targetClient, (ushort)MsgPacketId.recv_party_notify_kick, BufferProvider.Provide(), ServerType.Msg);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(targetClient.character.instanceId);
            router.Send(targetClient.map, (ushort)AreaPacketId.recv_charabody_notify_party_leave, res2, ServerType.Area);


            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteInt32(1); //Remove Reason
            res3.WriteUInt32(targetClient.character.instanceId); //Instance ID
            router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_remove_member, res3, ServerType.Msg);

            /*
            PARTY_REMOVE	0	%s has left the party
            PARTY_REMOVE	1	You have kicked %s from the party
            PARTY_REMOVE	2	%s has been buried
             */

            myParty.partyMembers.Remove(targetClient);

        }
    }
}
