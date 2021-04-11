using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyDisband : ClientHandler
    {
        public SendPartyDisband(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_party_disband;

        public override void Handle(NecClient client, NecPacket packet)
        {
            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_party_disband_r, res, ServerType.Area); ;
            router.Send(myParty.partyMembers, (ushort) AreaPacketId.recv_party_disband_r, res, ServerType.Area);


            IBuffer res2 = BufferProvider.Provide();
            router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_disband, res2, ServerType.Msg);

            foreach (NecClient partyClient in myParty.partyMembers)
            {
                IBuffer res3 = BufferProvider.Provide();
                res3.WriteUInt32(partyClient.character.instanceId);
                router.Send(partyClient.map, (ushort)AreaPacketId.recv_charabody_notify_party_leave, res3, ServerType.Area);
                router.Send(partyClient, (ushort)AreaPacketId.recv_party_leave_r, res3, ServerType.Area);
            }

            myParty.partyMembers.Clear();
        }
    }
}
