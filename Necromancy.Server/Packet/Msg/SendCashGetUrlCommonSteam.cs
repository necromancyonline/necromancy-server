using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_cash_get_url_common_steam : ClientHandler
    {
        public send_cash_get_url_common_steam(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_cash_get_url_common_steam;


        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();


            Router.Send(client, (ushort) MsgPacketId.recv_base_login_r, res, ServerType.Msg);
        }
    }
}
