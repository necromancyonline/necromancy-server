using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_disband : ClientHandler
    {
        public send_party_disband(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) AreaPacketId.send_party_disband;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_disband_r, res, ServerType.Area); ;
            Router.Send(myParty.PartyMembers, (ushort) AreaPacketId.recv_party_disband_r, res, ServerType.Area);


            IBuffer res2 = BufferProvider.Provide();
            Router.Send(myParty.PartyMembers, (ushort)MsgPacketId.recv_party_notify_disband, res2, ServerType.Msg);

            foreach (NecClient partyClient in myParty.PartyMembers)
            {
                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(partyClient.Character.InstanceId);
                Router.Send(partyClient.Map, (ushort)AreaPacketId.recv_charabody_notify_party_leave, res3, ServerType.Area);
                Router.Send(partyClient, (ushort)AreaPacketId.recv_party_leave_r, res3, ServerType.Area);
            }

            myParty.PartyMembers.Clear(); 
        }
    }
}
