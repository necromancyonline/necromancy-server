using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendFriendRequestLinkTarget : ClientHandler
    {
        public SendFriendRequestLinkTarget(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_friend_request_link_target;

        public override void Handle(NecClient client, NecPacket packet)
        {
            client.character.friendRequest = packet.data.ReadUInt32();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0); // 0 = sucess
            res.WriteUInt32(client.character.instanceId);
            router.Send(client, (ushort) MsgPacketId.recv_friend_request_link_target_r, res, ServerType.Msg);
            NotifyFriendInvite(client, client.character.friendRequest);
        }

        private void NotifyFriendInvite(NecClient client, uint targetInstanceId)
        {
            IBuffer res2 = BufferProvider.Provide();
            res2.WriteUInt32(client.character
                .instanceId); // Change nothing visibaly ?  Friend Relationship instance ID??? for database lookup?
            res2.WriteUInt32(client.character.instanceId); //?
            res2.WriteFixedString($"{client.soul.name}", 0x31); //size is 0x31
            res2.WriteFixedString($"{client.character.name}", 0x5B); //size is 0x5B
            res2.WriteUInt32(client.character.instanceId); //?
            res2.WriteByte(1);
            res2.WriteByte(0);
            router.Send(server.clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort) MsgPacketId.recv_friend_notify_link_invite, res2, ServerType.Msg);
            server.clients.GetByCharacterInstanceId(targetInstanceId).character.friendRequest =
                client.character.instanceId;
        }
    }
}
