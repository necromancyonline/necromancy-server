using Arrowgene.Buffers;
using Necromancy.Server.Common;
using Necromancy.Server.Model;
using Necromancy.Server.Packet.Id;

namespace Necromancy.Server.Packet.Area
{
    public class SendRaisescaleViewCloseRequest : ClientHandler
    {
        public SendRaisescaleViewCloseRequest(NecServer server) : base(server)
        {
        }

        public override ushort id => (ushort) AreaPacketId.send_raisescale_view_close_request;

        public override void Handle(NecClient client, NecPacket packet)
        {
            IBuffer res = BufferProvider.Provide();

            router.Send(client, (ushort) AreaPacketId.recv_raisescale_view_close, res, ServerType.Area);

            IBuffer res7 = BufferProvider.Provide();
            res7.WriteByte(0);
            router.Send(client, (ushort)AreaPacketId.recv_event_end, res7, ServerType.Area);
        }
    }
}
