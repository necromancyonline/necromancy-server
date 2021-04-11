using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_union_request_invite_target : ClientHandler
    {
        public send_union_request_invite_target(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_union_request_invite_target;


        public override void Handle(NecClient client, NecPacket packet)
        {
            uint targetInstanceId = packet.Data.ReadUInt32();
            NecClient targetClient = Server.Clients.GetByCharacterInstanceId(targetInstanceId);

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(client.Character.unionId); //union instance id?
            res.WriteUInt32(targetInstanceId); //TargetInstanceId

            Router.Send(client, (ushort) MsgPacketId.recv_union_request_invite_target_r, res, ServerType.Msg);


            IBuffer res2 = BufferProvider.Provide();
            res2.WriteInt32(client.Character.unionId); //ID of the Union
            res2.WriteUInt32(client.Character.InstanceId); //Reply to Instance Id for the invite
            res2.WriteFixedString($"by member {client.Character.Name}", 0x31); //size is 0x31
            res2.WriteFixedString(client.Union.Name, 0x5B); //size is 0x5B
            res2.WriteInt32(client.Character.unionId); //Unknown
            res2.WriteByte(99); //Unknown
            res2.WriteCString(client.Union.Name); //max size 0x31

            Router.Send(targetClient, (ushort) MsgPacketId.recv_union_notify_invite, res2, ServerType.Msg);
        }
    }
}
