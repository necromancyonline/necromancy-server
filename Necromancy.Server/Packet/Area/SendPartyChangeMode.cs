using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_party_change_mode : ClientHandler
    {
        public send_party_change_mode(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort)AreaPacketId.send_party_change_mode;

        public override void Handle(NecClient client, NecPacket packet)
        {
            int partyMode = packet.Data.ReadInt32();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            Router.Send(client, (ushort)AreaPacketId.recv_party_change_mode_r, res, ServerType.Area);

            Party myParty = Server.Instances.GetInstance(client.Character.partyId) as Party;

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(partyMode); //must be newLeaderInstanceId
            Router.Send(myParty.PartyMembers, (ushort)MsgPacketId.recv_party_notify_change_mode, res2, ServerType.Msg);

            myParty.PartyType = partyMode;

        }
    }
}
