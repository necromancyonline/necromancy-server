using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_expel_member : ClientHandler
    {
        public send_union_request_expel_member(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_expel_member;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint expelledCharacterInstanceId = packet.Data.ReadUInt32();
            NecClient explelledclient = Server.Clients.GetByCharacterInstanceId(expelledCharacterInstanceId);
            explelledclient.Union = null;

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_union_request_expel_member_r, res, ServerType.Msg);

            Router.Send(explelledclient, (ushort) MsgPacketId.recv_union_notify_expelled_member,
                BufferProvider.Provide(), ServerType.Msg);
        }
    }
}
