using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestInviteTarget : ClientHandler
    {
        public SendUnionRequestInviteTarget(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_invite_target;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetInstanceId = packet.data.ReadUInt32();
            NecClient targetClient = server.clients.GetByCharacterInstanceId(targetInstanceId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.character.unionId); //union instance id?
            res.WriteUInt32(targetInstanceId); //TargetInstanceId

            router.Send(client, (ushort) MsgPacketId.recv_union_request_invite_target_r, res, ServerType.Msg);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.character.unionId); //ID of the Union
            res2.WriteUInt32(client.character.instanceId); //Reply to Instance Id for the invite
            res2.WriteFixedString($"by member {client.character.name}", 0x31); //size is 0x31
            res2.WriteFixedString(client.union.name, 0x5B); //size is 0x5B
            res2.WriteInt32(client.character.unionId); //Unknown
            res2.WriteByte(99); //Unknown
            res2.WriteCString(client.union.name); //max size 0x31

            router.Send(targetClient, (ushort) MsgPacketId.recv_union_notify_invite, res2, ServerType.Msg);
        }
    }
}
