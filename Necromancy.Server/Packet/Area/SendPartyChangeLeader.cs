using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_change_leader : ClientHandler
    {
        public send_party_change_leader(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_change_leader;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint newLeaderInstanceId = packet.Data.ReadUInt32(); // use to make logic to set leader
            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); //set to 0 to mean "success" or inject an error code from str_table.csv
            Router.Send(client, (ushort) AreaPacketId.recv_party_change_leader_r, res, ServerType.Area);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(newLeaderInstanceId); //must be newLeaderInstanceId
            Router.Send(myParty.PartyMembers, (ushort)MsgPacketId.recv_party_notify_change_leader, res2, ServerType.Msg);

            myParty.PartyLeaderId = newLeaderInstanceId;

        }
    }
}
