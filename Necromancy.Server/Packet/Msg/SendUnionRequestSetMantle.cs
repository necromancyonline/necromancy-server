using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendUnionRequestSetMantle : ClientHandler
    {
        public SendUnionRequestSetMantle(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) MsgPacketId.send_union_request_set_mantle;


        public override void Handle(NecClient client, NecPacket packet)
        {
            ushort mantleDesign = packet.data.ReadUInt16();

            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            router.Send(client, (ushort) MsgPacketId.recv_union_request_set_mantle_r, res, ServerType.Msg);

            IBuffer res2 = BufferProvider.Provide();

            res2.WriteUInt16(mantleDesign); //design

            router.Send(client.map /*myUnion.UnionMembers*/, (ushort) MsgPacketId.recv_union_notify_mantle, res2,
                ServerType.Msg);
        }
    }
}
