using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class send_login_news_get_url : ClientHandler
    {
        public send_login_news_get_url(NecServer server) : base(server)
        {
        }


        public override ushort Id => (ushort) AreaPacketId.send_login_news_get_url;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteByte(0); //Bool
            res.WriteCString("");
            Router.Send(client, (ushort) AreaPacketId.recv_login_news_url_notify, res, ServerType.Area);
        }
    }
}
