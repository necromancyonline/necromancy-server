using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyChangeLeader : ClientHandler
    {
        public SendPartyChangeLeader(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_change_leader;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint newLeaderInstanceId = packet.data.ReadUInt32(); // use to make logic to set leader
            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //set to 0 to mean "success" or inject an error code from str_table.csv
            router.Send(client, (ushort) AreaPacketId.recv_party_change_leader_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(newLeaderInstanceId); //must be newLeaderInstanceId
            router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_change_leader, res2, ServerType.Msg);

            myParty.partyLeaderId = newLeaderInstanceId;

        }
    }
}
