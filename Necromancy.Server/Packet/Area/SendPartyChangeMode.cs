using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyChangeMode : ClientHandler
    {
        public SendPartyChangeMode(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_change_mode;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int partyMode = packet.data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort)AreaPacketId.recv_party_change_mode_r, res, ServerType.Area);

            Party myParty = server.instances.GetInstance(client.character.partyId) as Party;

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(partyMode); //must be newLeaderInstanceId
            router.Send(myParty.partyMembers, (ushort)MsgPacketId.recv_party_notify_change_mode, res2, ServerType.Msg);

            myParty.partyType = partyMode;

        }
    }
}
