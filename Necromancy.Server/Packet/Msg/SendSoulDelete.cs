using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_soul_delete : ClientHandler
    {
        public send_soul_delete(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_soul_delete;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            res.WriteInt32(0);


            Router.Send(client, (ushort) MsgPacketId.recv_soul_delete_r, res, ServerType.Msg);

            //TODO
            //L"network::proto_msg_implement_client::recv_refusallist_notify_remove_user_souldelete()
        }
    }
}
