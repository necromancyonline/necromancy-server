using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendPartyDeclineToApply : ClientHandler
    {
        public SendPartyDeclineToApply(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)AreaPacketId.send_party_decline_to_apply;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint applicantInstanceId = packet.data.ReadUInt32(); //so you can write code to tell the applicant they were declined
            uint errorOrSuccessCode = packet.data.ReadUInt32(); //error code?

            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(errorOrSuccessCode);
            router.Send(client, (ushort)AreaPacketId.recv_party_decline_to_apply_r, res, ServerType.Area);

            NecClient myPartyDeclinedClient = server.clients.GetByCharacterInstanceId(applicantInstanceId);

            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(0); //What does this do? nothing visible
            router.Send(myPartyDeclinedClient, (ushort)MsgPacketId.recv_party_notify_decline_to_apply, res2, ServerType.Msg);
        }
    }
}
