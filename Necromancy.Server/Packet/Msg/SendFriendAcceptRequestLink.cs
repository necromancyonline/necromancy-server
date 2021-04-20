using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendFriendAcceptRequestLink : ClientHandler
    {
        public SendFriendAcceptRequestLink(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_friend_accept_request_link;

        public override void Handle(NecClient client, NecPacket packet)
        {
            uint friendInstanceId = packet.data.ReadUInt32();
            int acceptOrDenyResponse = packet.data.ReadByte();
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(acceptOrDenyResponse); // 0 = Deny, 1 = Accept
            res.WriteUInt32(friendInstanceId); // Object ID
            router.Send(client, (ushort)MsgPacketId.recv_friend_reply_to_link_r, res, ServerType.Msg);
            SendFriendNotifyAddMember(client);
            SendFriendNotifyAddMember(client, friendInstanceId);
        }

        private void SendFriendNotifyAddMember(NecClient client)
        {
            NecClient targetClient = server.clients.GetByCharacterInstanceId(client.character.friendRequest);
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.friendRequest);
            res.WriteUInt32(targetClient.character.instanceId);
            res.WriteFixedString($"{targetClient.soul.name}", 0x31); //soul name
            res.WriteFixedString($"{targetClient.character.name}", 0x5B); //character name
            res.WriteUInt32(targetClient.character.classId); // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(targetClient.character.level); // Level of the friend
            res.WriteByte(0);
            res.WriteInt32(targetClient.character.mapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFixedString($"Channel {targetClient.character.channel}", 0x61); // Channel location
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            router.Send(client, (ushort)MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }

        private void SendFriendNotifyAddMember(NecClient client, uint targetInstanceId)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteUInt32(client.character.instanceId);
            res.WriteUInt32(client.character.instanceId);
            res.WriteFixedString($"{client.soul.name}", 0x31); //soul name
            res.WriteFixedString($"{client.character.name}", 0x5B); //character name
            res.WriteUInt32(client.character.classId); // Class 0 = Fighter, 1 = thief, ect....
            res.WriteByte(client.character.level); // Level of the friend
            res.WriteByte(0);
            res.WriteInt32(client.character.mapId); // Location of your friend
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteByte(0);
            res.WriteFixedString($"Channel {client.character.channel}",
                0x61); // When i put the channel it doesn't add the friend, need to fix that.
            res.WriteInt32(0);
            res.WriteInt32(0);
            res.WriteInt32(0);
            router.Send(server.clients.GetByCharacterInstanceId(targetInstanceId),
                (ushort)MsgPacketId.recv_friend_notify_add_member_r, res, ServerType.Msg);
        }
    }
}
