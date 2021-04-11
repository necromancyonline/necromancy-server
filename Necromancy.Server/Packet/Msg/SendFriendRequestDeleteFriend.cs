using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_request_delete_friend : ClientHandler
    {
        public send_friend_request_delete_friend(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_friend_request_delete_friend;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetInstanceId = packet.Data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);
            Router.Send(client, (ushort) MsgPacketId.recv_friend_request_delete_friend_r, res, ServerType.Msg);

            IBuffer res3 = BufferProvider.Provide();
            res3.WriteUInt32(targetInstanceId);
            Router.Send(client, (ushort) MsgPacketId.recv_friend_notify_delete_member, res3, ServerType.Msg);

            IBuffer res4 = BufferProvider.Provide();
            res4.WriteUInt32(client.Character.InstanceId);
            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort) MsgPacketId.recv_friend_notify_delete_member, res4, ServerType.Msg);
        }
    }
}
