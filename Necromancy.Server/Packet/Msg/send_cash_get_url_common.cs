using Arrowgene.Services.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Msg
{
    public class send_cash_get_url_common : Handler
    {
        public send_cash_get_url_common(NecServer server) : base(server)
        {
        }

        public override ushort Id => (ushort) MsgPacketId.send_cash_get_url_common;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();
            res.WriteInt32(0);
            res.WriteByte(1);
            //Router.Send(client, (ushort) MsgPacketId.recv_soul_select_r, res);
        }
    }
}