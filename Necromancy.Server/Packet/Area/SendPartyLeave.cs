using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Common.Instance;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyLeave : ClientHandler
    {
        public SendPartyLeave(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_leave;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;



            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);

            router.Send(client, (ushort) AreaPacketId.recv_party_leave_r, res, ServerType.Area);
            router.Send(client, (ushort) AreaPacketId.recv_chara_notify_party_leave, res, ServerType.Area);
            router.Send(client.map, (ushort) AreaPacketId.recv_charabody_notify_party_leave, res, ServerType.Area);

            res = BufferProvider.Provide();
            res.WriteInt32(0); //Remove Reason
            res.WriteUInt32(client.character.instanceId); //Instance ID
            router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_remove_member, res, ServerType.Msg);

            /*
            PARTY_REMOVE	0	%s has left the party
            PARTY_REMOVE	1	You have kicked %s from the party
            PARTY_REMOVE	2	%s has been buried
             */

            myParty.Leave(client);

            //if the leader leaves appoint a new leader
            if (myParty.partyLeaderId == client.character.instanceId)
            {
                uint newLeaderInstanceId = myParty.partyMembers.ToArray()[0].character.instanceId;
                res = BufferProvider.Provide();
                res.WriteUInt32(newLeaderInstanceId); //must be newLeaderInstanceId
                router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_change_leader, res, ServerType.Msg);

                myParty.partyLeaderId = newLeaderInstanceId;
            }
        }
    }
}
