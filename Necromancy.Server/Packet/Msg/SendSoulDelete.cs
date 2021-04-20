using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class SendSoulDelete : ClientHandler
    {
        public SendSoulDelete(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort)MsgPacketId.send_soul_delete;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);


            router.Send(client, (ushort)MsgPacketId.recv_soul_delete_r, res, ServerType.Msg);

            //TODO
            //L"network::proto_msg_implement_client::recv_refusallist_notify_remove_user_souldelete()
        }
    }
}
