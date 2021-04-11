using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_friend_accept_request_link : ClientHandler
    {
        public send_friend_accept_request_link(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_friend_accept_request_link;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint friendInstanceId = packet.Data.ReadUInt32();
            int acceptOrDenyResponse = packet.Data.ReadByte();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(acceptOrDenyResponse); // 0 = Deny, 1 = Accept
            res.WriteUInt32(friendInstanceId); // Object ID
            Router.Send(client, (ushort) MsgPacketId.recv_friend_reply_to_link_r, res, ServerType.Msg);
            SendFriendNotifyAddMember(client);
            SendFriendNotifyAddMember(client, friendInstanceId);
        }

        private void SendFriendNotifyAddMember(NecClient client)
        {
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(client.Character.friendRequest);
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.friendRequest);
            res.WriteUInt32(targetClient.Character.InstanceId);
            res.WriteFixedString($"{targetClient.Soul.Name}", 0x31); //soul name
            res.WriteFixedString($"{targetClient.Character.Name}", 0x5B); //character name
            res.WriteUInt32(targetClient.Character.ClassId); // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(targetClient.Character.Level); // Level of the friend
            res.WriteByte(0);
            res.WriteInt32(targetClient.Character.MapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFixedString($"Channel {targetClient.Character.Channel}", 0x61); // Channel location
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(client, (ushort) MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }

        private void SendFriendNotifyAddMember(NecClient client, uint targetInstanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteUInt32(client.Character.InstanceId);
            res.WriteFixedString($"{client.Soul.Name}", 0x31); //soul name
            res.WriteFixedString($"{client.Character.Name}", 0x5B); //character name
            res.WriteUInt32(client.Character.ClassId); // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(client.Character.Level); // Level of the friend
            res.WriteByte(0);
            res.WriteInt32(client.Character.MapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFixedString($"Channel {client.Character.Channel}",
                0x61); // When i put the channel it doesn't add the friend, need to fix that.
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            Router.Send(Server.Clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort) MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }
    }
}
