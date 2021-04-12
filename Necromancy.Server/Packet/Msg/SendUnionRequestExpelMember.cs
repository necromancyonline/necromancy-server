using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestExpelMember : ClientHandler
    {
        public SendUnionRequestExpelMember(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_union_request_expel_member;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint expelledCharacterInstanceId = packet.data.ReadUInt32();
            NecClient explelledclient = server.clients.GetByCharacterInstanceId(expelledCharacterInstanceId);
            explelledclient.union = null;

            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);
            router.Send(client, (ushort)MsgPacketId.recv_union_request_expel_member_r, res, ServerType.Msg);

            router.Send(explelledclient, (ushort)MsgPacketId.recv_union_notify_expelled_member,
                BufferProvider.Provide(), ServerType.Msg);
        }
    }
}
