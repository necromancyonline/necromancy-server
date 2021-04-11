using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_cash_get_url_common : ClientHandler
    {
        public send_cash_get_url_common(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_cash_get_url_common;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteInt32(1);
            res.WriteCString("www.unknownone69.com");
            Router.Send(client, (ushort) MsgPacketId.recv_cash_get_url_common_r, res, ServerType.Msg);
        }
    }
}
