using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestMemberPriv : ClientHandler
    {
        public SendUnionRequestMemberPriv(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_member_priv;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetMemberInstanceId = packet.data.ReadUInt32();
            int newPermissionBitmask = packet.data.ReadInt32();
            NecClient targetClient = server.clients.GetByCharacterInstanceId(targetMemberInstanceId);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(newPermissionBitmask);
            router.Send(targetClient, (ushort) MsgPacketId.recv_union_request_member_priv_r, res, ServerType.Msg);
        }
    }
}
