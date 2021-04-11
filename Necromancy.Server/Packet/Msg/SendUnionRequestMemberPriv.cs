using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_member_priv : ClientHandler
    {
        public send_union_request_member_priv(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_member_priv;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetMemberInstanceId = packet.Data.ReadUInt32();
            int newPermissionBitmask = packet.Data.ReadInt32();
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetMemberInstanceId);


            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(newPermissionBitmask);
            Router.Send(targetClient, (ushort) MsgPacketId.recv_union_request_member_priv_r, res, ServerType.Msg);
        }
    }
}
