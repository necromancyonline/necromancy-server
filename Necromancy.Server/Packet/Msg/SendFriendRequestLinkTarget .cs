using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_request_link_target : ClientHandler
    {
        public send_friend_request_link_target(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_friend_request_link_target;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.Character.friendRequest = packet.Data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = sucess
            res.WriteUInt32(client.Character.InstanceId);
            Router.Send(client, (ushort) MsgPacketId.recv_friend_request_link_target_r, res, ServerType.Msg);
            NotifyFriendInvite(client, client.Character.friendRequest);
        }

        private void NotifyFriendInvite(NecClient client, uint targetInstanceId)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.Character
                .InstanceId); // Change nothing visibaly ?  Friend Relationship instance ID??? for database lookup?
            res2.WriteUInt32(client.Character.InstanceId); //?
            res2.WriteFixedString($"{client.Soul.Name}", 0x31); //size is 0x31
            res2.WriteFixedString($"{client.Character.Name}", 0x5B); //size is 0x5B
            res2.WriteUInt32(client.Character.InstanceId); //?
            res2.WriteByte(1);
            res2.WriteByte(0);
            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort) MsgPacketId.recv_friend_notify_link_invite, res2, ServerType.Msg);
            Server.Clients.GetByCharacterInstanceId(targetInstanceId).Character.friendRequest =
                client.Character.InstanceId;
        }
    }
}
